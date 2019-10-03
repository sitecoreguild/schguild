/********************************************************************************
 * Sitecore Content Hub C# Script Samples
 * 
 * This script contains functions that call MClient to execute queries to get 
 * search results.
 * 
 * FOR MORE INFORMATION LOOK AT OUT:
 *  
 * - Glossary Entry: https://schguild.blogspot.com/p/nomenclature.html#GLOSSARY_ACTION_TYPES
 * - Official Documentation: https://docs.stylelabs.com/content/integrations/sdk-common-documentation/clients/querying-client.html
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

// Sample Description: Gets all assets from Content Hub ordered by created on 
// ascending and displays the name to console.
GetResultsFromQueryingAssetsAllSortAscendingCreatedOn();

// ----------------------------------------------------------------------------------

/// <summary>
/// Executes a query to return all assets in ascending order by CreatedOn field.
/// </summary>
/// <returns></returns>
public void GetResultsFromQueryingAssetsAllSortAscendingCreatedOn()
{
    //  This is the API to get all scripts /api/entities/query?query=Definition.Name=='M.Script'

    var query = Query.CreateQuery(entities =>
        from e in entities
        where e.DefinitionName == "M.Asset"
        orderby e.CreatedOn ascending
        select e);

    var queryResults = MClient.Querying.QueryAsync(query).Result.Items.ToList();
    foreach(var entity in queryResults)
    {
        Console.WriteLine(entity.Id);
        Console.WriteLine(entity.Identifier);
    }
}
