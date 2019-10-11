/********************************************************************************
 * Sitecore Content Hub CD/CI
 * 
 * This script connects to a content hub instance and exports scripts to the same folder as this script.
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to one of the 
 *          developers. This will ensure others using the application will benefit from this as well.
 *          
 *          christopher_williams@epam.com
 *          jose_dominguez@epam.com
 *          brent_pinkstaff@epam.com
 *          joe_bissol@epam.com
 *          
 * INSTALL AND USAGE
 * 
 * - Before using ensure you edit the Settings.csx to match your instance of Sitecore Content Hub
 * - Copy the contents of the Tools folder to your local script directory.
 * - Next you open a command prompt and cd to your local script directory.
 * - Next you execute the script using this command:
 *
 * Dotnet-Script -d "Export Scripts Locally.csx" -s nuget
 *
 * KNOWN ISSUES: 
 *  
 ********************************************************************************/

#load "References.csx"          // Library References
#load "Settings.csx"            // Contains the instance specific settings for your environment
#load "MClient.csx"             // Instantiates the MClient object for use by other scripts
#load "ScriptFunctions.csx"     // Functions that call MClient to manipulate scripts

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Framework.Essentials.LoadOptions;


Console.WriteLine("Exporting scripts to: " + scriptDirectoryPath);

var scripts = await GetAllScriptsAndContents();
var scriptRelationLoadOption = new RelationLoadOption(new string[] { "ScriptToActiveScriptContent", "ScriptToDraftScriptContent" });
var scriptLoadConfig = new EntityLoadConfiguration(CultureLoadOption.Default, PropertyLoadOption.None, scriptRelationLoadOption);
var scriptEntities = await MClient.Entities.GetManyAsync(scripts.Keys, scriptLoadConfig).ConfigureAwait(false);

foreach (var scriptEntity in scriptEntities)
{
    //todo: only update changed scripts
    var draftId = await CreateAndSetDraftScript(scriptEntity, scripts[scriptEntity.Id.Value]);
    var scriptCompiled = await CompileScript(draftId);
    if (scriptCompiled)
    {
        var published = await PublishScript(draftId, scriptEntity.Id.Value);
        Console.WriteLine("Scripts published.");
    }
}

async Task<Dictionary<long, string>> GetAllScriptsAndContents()
{
    if (!Directory.Exists(scriptDirectoryPath))
    {
        throw new FileNotFoundException("Scripts directory not found");
    }

    var scripts = new Dictionary<long, string>();
    var scriptFiles = Directory.GetFiles(scriptDirectoryPath, "*.csx");
    byte[] scriptContent;
    foreach (var scriptPath in scriptFiles)
    {
        var fileName = Path.GetFileName(scriptPath);

        if(fileName.StartsWith("Execute locally")) //todo: find cleaner way to ignore local files or move to shared file
        {
            continue;
        }

        using (FileStream scriptFile = File.Open(scriptPath, FileMode.Open, FileAccess.Read))
        {
            scriptContent = new byte[scriptFile.Length];
            await scriptFile.ReadAsync(scriptContent, 0, (int)scriptFile.Length);
        }
        
        var scriptId  = long.Parse(fileName.Split('_')[0]);
        if (!scripts.ContainsKey(scriptId))
        {
            scripts.Add(scriptId, System.Text.Encoding.ASCII.GetString(scriptContent));
        }
    }
    return scripts;
}

async Task<long> CreateAndSetDraftScript(IEntity script, string content)
{
    //todo: should overwrite any draft entity? or always create a new one
    var scriptToDraftRelation = await script.GetRelationAsync<IParentToOneChildRelation>("ScriptToDraftScriptContent", MemberLoadOption.LazyLoading).ConfigureAwait(false);

    IEntity draftEntity;
    if (scriptToDraftRelation.Child.HasValue)
    {
        draftEntity = await MClient.Entities.GetAsync(scriptToDraftRelation.Child.Value);
    }
    else
    {
        draftEntity = await MClient.EntityFactory.CreateAsync("M.ScriptContent", CultureLoadOption.Default).ConfigureAwait(false);
    }

    draftEntity.SetPropertyValue("M.ScriptContent.Script", content);
    long draftEntityId = await MClient.Entities.SaveAsync(draftEntity).ConfigureAwait(false);

    if (!scriptToDraftRelation.Child.HasValue)
    {
        scriptToDraftRelation.SetId(draftEntityId);
        await MClient.Entities.SaveAsync(script).ConfigureAwait(false);
    }
    return draftEntityId;
}

async Task<bool> CompileScript(long draftScriptId)
{
    var url = new Uri($"{baseUrl}/api/scripts/{draftScriptId}/compile");
    await MClient.Raw.PostAsync(url);

    bool done = false;
    bool errors = false;
    int tries = 0;
    int maxTries = 5;
    IEntity entity;
    while (!done && !errors && tries < maxTries) //todo: would it be better to post all scripts and then loop and check status?
    {
        tries++;
        Thread.Sleep(2000); //todo: find suitable time to check for status
        entity = await MClient.Entities.GetAsync(
            draftScriptId, 
            new EntityLoadConfiguration(CultureLoadOption.Default, new PropertyLoadOption(new string[] { "M.ScriptContent.CompileStatus" }), RelationLoadOption.None))
            .ConfigureAwait(false);
        var status = entity.GetPropertyValue<string>("M.ScriptContent.CompileStatus").ToUpperInvariant();
        done = status.Equals("SUCCESS");
        errors = errors.Equals("ERRORS");
    }
    
    return done && !errors;
}

async Task<bool> PublishScript(long draftScriptId, long parentScriptId)
{
    var url = new Uri($"{baseUrl}/api/scripts/{draftScriptId}/publish");
    await MClient.Raw.PostAsync(url);

    bool done = false;
    int tries = 0;
    int maxTries = 5;
    IEntity entity;
    while (!done && tries < maxTries) //todo: would be better to post all scripts and then loop and check status
    {
        tries++;
        Thread.Sleep(2000); //todo: find suitable time to check for status
        entity = await MClient.Entities.GetAsync(
            draftScriptId,
            new EntityLoadConfiguration(CultureLoadOption.Default, PropertyLoadOption.None, new RelationLoadOption(new string[] { "ScriptToActiveScriptContent" })))
            .ConfigureAwait(false);
        var activeScriptRelation = entity.GetRelation<IChildToOneParentRelation>("ScriptToActiveScriptContent");
        if(activeScriptRelation != null && activeScriptRelation.Parent.HasValue && activeScriptRelation.Parent.Value == parentScriptId)
        {
            done = true;
        }
    }

    return done && tries < maxTries;
}