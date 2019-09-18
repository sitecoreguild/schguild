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

#load "Settings.csx"            // Contains the instance specific settings for your environment
#load "MClient.csx"             // Instantiates the MClient object for use by other scripts
#load "ScriptFunctions.csx"     // Functions that call MClient to manipulate scripts

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Stylelabs.M.Base.Querying.Filters;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.WebClient;

Console.WriteLine("Exporting scripts to: " + scriptDirectoryPath);

var idList = GetAllScriptIds();
foreach(var id in idList)
{
    var script = GetScriptDefaultCultureFull(id);
    var friendlyScriptName = script.Id + "_" + script.Identifier.Replace("-", "_");

    // Get script as Json and save to script directory
    var scriptJson = GetScriptAsJson(baseUrl, id);
    if (Directory.Exists(scriptDirectoryPath) == false) Directory.CreateDirectory(scriptDirectoryPath);
    File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + friendlyScriptName + ".json", scriptJson);

    // Within the relations you can look up the reference to the actual script
    var scriptObject = JObject.Parse(scriptJson);
    if (scriptObject.ContainsKey("relations"))
    {
        // TO DO: Should do more validation not assume structure
        var scriptToActiveScriptContentHref = scriptObject["relations"]["ScriptToActiveScriptContent"]["href"].ToString();

        var headers = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "X-ApiVersion", "3" },
            { "minimalSchema", "true" }
        };
        var scriptToActiveScriptContentJson = MClient.Raw.GetAsync(scriptToActiveScriptContentHref, headers).Result.Content.ReadAsStringAsync().Result;
        var csxEntityHrefObject = JObject.Parse(scriptToActiveScriptContentJson);

        // TO DO: Add more validation here for broken JSON for example script may have name but no C# script yet.
        if (csxEntityHrefObject.ContainsKey("child"))
        {
            var csxHref = csxEntityHrefObject["child"]["href"].ToString();

            // Get the actual C# Script and save it to the .csx file
            var csxScriptJson = MClient.Raw.GetAsync(csxHref, headers).Result.Content.ReadAsStringAsync().Result;
            var csxScriptObject = JObject.Parse(csxScriptJson);
            var csxScript = csxScriptObject["properties"]["M.ScriptContent.Script"].ToString();
            File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + friendlyScriptName + ".csx", csxScript);
        }

        // Generate the Execute Local {scriptname}.csx script
        var executeLocalScript = new StringBuilder();
        executeLocalScript.AppendLine("#load \"Settings.csx\"");
        executeLocalScript.AppendLine("#load \"MClient.csx\"");
        executeLocalScript.AppendFormat("#load \"{0}\"\r\n", friendlyScriptName);
        File.WriteAllText(scriptDirectoryPath + (scriptDirectoryPath.EndsWith("\\") ? string.Empty : "\\") + "Execute locally " + friendlyScriptName + ".csx", executeLocalScript.ToString());

        // TO DO: Add a checksup to the script and reimport it (Thanks charles turtino for the idea)
    }
}