/********************************************************************************
 * Sitecore Content Hub C# Script Samples
 * 
 * This script contains functions that call MClient to access and manipulate taxonomy.
 * 
 * FOR MORE INFORMATION LOOK AT OUT:
 *  
 * - Glossary Entry: https://schguild.blogspot.com/p/nomenclature.html#GLOSSARY_TAXONOMY
 * - Official Documentation: https://docs.stylelabs.com/content/user-documentation/administration/data/taxonomies/taxonomy.html?q=taxonomy
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

using System.Net.Http;
using System.Text;
using Stylelabs.M.Base.Querying.Filters;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.WebClient;

// Sample Description: Creates a taxonomy item with the name TEST.TAG.{year}.{month}.{day}.{hour}.{minute}
// CreateSampleTaxonomy();

// Sample Description: Gets all the taxonomy as JSON from Content Hub and displays the name to console.
// GetAllTaxonomyAsJson();

// Sample Description: Gets a sample taxonomy item and displays it in the console.
// GetSampleTaxonomyItem();

// ----------------------------------------------------------------------------------

/// <summary>
/// Creates a taxonomy sample item
/// </summary>
public void CreateSampleTaxonomy()
{
    // NOTE: Taxonomy names can only have letters, dots and underscores (not numbers)
    var taxonomyItemName = "TEST.TAG";
    var taxonomyItemLabelEnUs = "Test Tag " + DateTime.Now.ToString();

    var absouluteUrl = string.Format("{0}/api/taxonomy", baseUrl);
    var contentJson = "{\"name\":\"" +taxonomyItemName + "\",\"isManualSortingAllowed\":false,\"labels\":{\"en-US\":\"" + taxonomyItemLabelEnUs + "\"}}";

    // IMPORTANT: You need to make sure you pass the media type of json or it will give you invalid media type error.
    var content = new StringContent(contentJson,Encoding.UTF8, "application/json");

    content.Headers.Add("X-ApiVersion", "3");
    content.Headers.Add("minimalSchema", "true");

    var result = MClient.Raw.PostAsync(absouluteUrl, content).Result;
    Console.WriteLine(result.IsSuccessStatusCode 
                        ? " taxonomy item created successfully with id = " + result.Content.ReadAsStringAsync().Result
                        : " failed to create taxonomy item with error: " + result.Content.ReadAsStringAsync().Result);
}

public void GetSampleTaxonomyItem()
{
    // TO DO: Need to create a sample
}

/// <summary>
/// Sample on how to get all the taxonomy items as a JSON
/// </summary>
public void GetAllTaxonomyAsJson()
{
    var absouluteUrl = string.Format("{0}/api/taxonomy", baseUrl);

    var headers = new Dictionary<string, string>();
    headers.Add("Accept", "application/json");
    headers.Add("X-ApiVersion", "3");
    headers.Add("minimalSchema", "true");

    var taxonomyJson = MClient.Raw.GetAsync(absouluteUrl, headers).Result.Content.ReadAsStringAsync().Result;
        
    Console.WriteLine(taxonomyJson);
}
