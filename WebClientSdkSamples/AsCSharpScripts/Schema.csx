/********************************************************************************
 * Sitecore Content Hub C# Script Samples
 * 
 * This script contains functions that call MClient to access and manipulate schema or entity definitions.
 * 
 * FOR MORE INFORMATION LOOK AT OUT:
 *  
 * - Glossary Entry: https://schguild.blogspot.com/p/nomenclature.html#GLOSSARY_SCHEMA
 * - Official Documentation: https://docs.stylelabs.com/content/integrations/sdk-common-documentation/clients/entity-definitions-client.html
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

// Sample Description: Gets many schemas based on ids from Content Hub as JSON and displays the name to console.
GetManySchemasByIds();

// Sample Description: Gets many schemas based on names from Content Hub as JSON and displays the name to console.
GetManySchemasByName();

// ----------------------------------------------------------------------------------

// Gets multiple schemas based on the ids
public void GetManySchemasByIds()
{
    var ids = new List<long> { 3 };
    var schemas = MClient.EntityDefinitions.GetManyCachedAsync(ids).Result;
    foreach(var schema in schemas)
    {
        Console.WriteLine("Schema Id:" + schema.Id);
        Console.WriteLine("Schema Name:" + schema.Name);
    }
}

// Gets multiple schemas based on the name
public void GetManySchemasByName()
{
    var names = new List<string>
    {
        "M.BudgetLineItem",
        "M.Project.Block",
        "M.Fragment",
        "M.Asset",
        "M.Project.Task",
        "M.Project.Block",
        "M.Discussion",
        "M.Project.Block",
        "M.Fragment",
        "M.Asset"
    };

    var schemas = MClient.EntityDefinitions.GetManyCachedAsync(names).Result;
    foreach(var schema in schemas)
    {
        Console.WriteLine("Schema Id:" + schema.Id);
        Console.WriteLine("Schema Name:" + schema.Name);
    }
}
