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

using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Stylelabs.M.Base.Querying.Filters;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Framework.Essentials.LoadOptions;

Console.WriteLine("Exporting scripts to: " + scriptDirectoryPath);

var activeContentLoadConfig = new EntityLoadConfiguration(CultureLoadOption.Default, PropertyLoadOption.All, RelationLoadOption.None);

var scripts = await GetAllScripts();

if (!Directory.Exists(scriptDirectoryPath))
{
    Directory.CreateDirectory(scriptDirectoryPath);
}

foreach (var script in scripts)
{
    var friendlyScriptName = GetScriptFriendlyName(script);
    
    // Get script as Json and save to script directory
    var scriptJson = JsonConvert.SerializeObject(script);
    File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + friendlyScriptName + ".json", scriptJson);

    var scriptToActiveContentRelationId = await script.GetRelationAsync<IParentToOneChildRelation>("ScriptToActiveScriptContent").ConfigureAwait(false);
    if(!scriptToActiveContentRelationId.Child.HasValue)
    {
        continue;
    }

    var activeContentEntity = await MClient.Entities.GetAsync(scriptToActiveContentRelationId.Child.Value, activeContentLoadConfig);
    var csxScript = activeContentEntity.GetPropertyValue<string>("M.ScriptContent.Script");

    File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + friendlyScriptName + ".csx", csxScript);

    // Generate the Execute Local {scriptname}.csx script
    var executeLocalScript = new StringBuilder();
    executeLocalScript.AppendLine("#load \"References.csx\"");
    executeLocalScript.AppendLine("#load \"Settings.csx\"");
    executeLocalScript.AppendLine("#load \"MConnector.Client.csx\"");
    executeLocalScript.AppendFormat("#load \"{0}\"\r\n", friendlyScriptName);
    File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + "Execute locally " + friendlyScriptName + ".csx", executeLocalScript.ToString());

    // TO DO: Add a checksup to the script and reimport it (Thanks charles turtino for the idea)
}
