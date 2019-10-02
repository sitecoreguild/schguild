/********************************************************************************
 * Sitecore Content Hub - DataSource / OptionList
 * 
 * This script contains functions that call MClient to access and manipulate DataSources 
 * also known as OptionList.
 * 
 * FOR MORE INFORMATION LOOK AT OUT:
 *  
 * - Glossary Entry: https://schguild.blogspot.com/p/nomenclature.html#GLOSSARY_OPTION_LISTS
 * - Official Documentation: https://docs.stylelabs.com/content/user-documentation/administration/data/option-lists/option-lists.html
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
 * TO DO: 
 * 
 * - CreateSampleDataSource - Need to code a sample
 * 
 ********************************************************************************/

#load "Settings.csx"            // Contains the instance specific settings for your environment
#load "MClient.csx"             // This script initializes the MClient

using System;
using System.Collections.Generic;

using Stylelabs.M.Base.Querying.Filters;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.WebClient;

// Sample Description: Creates a sample flat data source
// CreateSampleFlatDataSource();

// Sample Description: Creates a sample hierarchial data source
// CreateSampleHierarchialDataSource();

// Sample Description: Gets all the data sources from Content Hub and displays the name to console.
GetAllDataSources();

// Sample Description: Gets a sample data source / option list from Content Hub and displays it in the console.
GetSampleDataSource();

// ----------------------------------------------------------------------------------

/// <summary>
/// Name of an Option List to use for testing. You can confirm it exists in your instance here: https://{baseUrl}/en-us/admin/option-lists/
/// </summary>
public const string TestOptionListName = "NotificationTypes";

/// <summary>
/// The identifier for an option in the test option list. You can confirm it exists in your instance here: https://{baseUrl}/en-us/admin/option-lists/ 
/// then edit the option list and click on an item in the list. The value in the identifer field can be used here.
/// </summary>
public const string TestOptionListDataSourceValueIdentifier = "SearchChanged";

public void CreateSampleFlatDataSource()
{
    // TO DO: Create a sample on creating an option list
    // var flatDataSource = MClient.DataSourceFactory.CreateFlatDataSource("TestFlatDataSource_" + DateTime.Now.ToString());
}

public void CreateSampleHierarchialDataSource()
{
    // var hierarchialDataSource = MClient.DataSourceFactory.CreateHierarchicalDataSource("TestHierarchialDataSource_" + DateTime.Now.ToString());
}

// Gets all the DataSources / OptionLists and writes their names to the console
public void GetAllDataSources()
{
    var dataSourceValues = MClient.DataSources.GetAllAsync().Result;

    Console.WriteLine("GetAllDataSources");

    foreach(var dataSourceValue in dataSourceValues)
    {
        Console.WriteLine("  OptionList:");
        Console.WriteLine("      " + dataSourceValue);
    }
}

// Gets a sample DataSource / OptionList
public void GetSampleDataSource()
{
    var dataSourceValue = MClient.DataSources.GetAsync(TestOptionListName).Result;

    Console.WriteLine("GetSampleDataSource");
    Console.WriteLine("     Identifier:" + dataSourceValue.Name);
    Console.WriteLine("     CreatedBy:" + dataSourceValue.CreatedBy);
    Console.WriteLine("     CreatedOn:" + dataSourceValue.CreatedOn);
    Console.WriteLine("     ModifiedBy:" + dataSourceValue.ModifiedBy);
    Console.WriteLine("     ModifiedOn:" + dataSourceValue.ModifiedOn);
    Console.WriteLine("     Type:" + dataSourceValue.Type);

    foreach(var label in dataSourceValue.Labels)
    {
        Console.WriteLine("     Label:" + label.Key + " = " + label.Value);
    }
}
