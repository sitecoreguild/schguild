/********************************************************************************
 * Sitecore Content Hub - Action Types
 * 
 * This script contains functions that call MClient to access and manipulate Action Types.
 * 
 * FOR MORE INFORMATION LOOK AT OUT:
 *  
 * - Glossary Entry: https://schguild.blogspot.com/p/nomenclature.html#GLOSSARY_ACTION_TYPES
 * - Official Documentation: https://docs.stylelabs.com/content/integrations/intergration-components/actions/action-types.html?v=3.2.0
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to one of the 
 *          developers. This will ensure others using the application will benefit from this as well.
 *          
 *          christopher_williams@epam.com
 *          jose_dominguez@epam.com
 *          brent_pinkstaff@epam.com
 *          joe_bissol@epam.com
 *          jon_fairchild@epam.com
 *          
 * INSTALL AND USAGE
 * 
 * - Before using ensure you edit the Settings.csx to match your instance of Sitecore Content Hub
 * - This C# script file contains sample calls to functions at the top. Uncomment the ones you want to try.
 *
 ********************************************************************************/

#load "Settings.csx"            // Contains the instance specific settings for your environment
#load "MClient.csx"             // This script initializes the MClient

using System;

using Stylelabs.M.Base.Querying.Filters;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.WebClient;

// Sample Description: Gets all the action types from Content Hub as JSON and displays the name to console.
GetAllActionTypesRaw();

// ----------------------------------------------------------------------------------

// Gets all the DataSources and writes their names to the console
public void GetAllActionTypesRaw()
{
    var absouluteUrl = string.Format("{0}/api/datasources/ActionTypes", baseUrl);

    var headers = new Dictionary<string, string>();
    headers.Add("Accept", "application/json");
    headers.Add("X-ApiVersion", "3");
    headers.Add("minimalSchema", "true");

    var actionTypeJson = MClient.Raw.GetAsync(absouluteUrl, headers).Result.Content.ReadAsStringAsync().Result;
        
    Console.WriteLine(actionTypeJson);
}
