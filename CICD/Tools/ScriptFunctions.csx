/********************************************************************************
 * Sitecore Content Hub CD/CI
 * 
 * This script contains functions that call MClient to manipulate scripts.
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
using Stylelabs.M.Framework.Essentials.LoadOptions;

/// <summary>
/// Gets the specific script with the default culture and all the properties and relations.
/// </summary>
/// <param name="id">id of the script to get</param>
/// <returns></returns>
public IEntity GetScriptDefaultCultureFull(long id)
{
    var filter = new IdQueryFilter();
    filter.Value = id;

    var query = new Query { Filter = filter };
    return MClient.Querying.SingleAsync(query, EntityLoadConfiguration.DefaultCultureFull).Result;
}

/// <summary>
/// Get Ids of all the scripts in Sitecore Content Hub 
/// </summary>
/// <returns></returns>
public List<long> GetAllScriptIds()
{
    //  This is the API to get all scripts /api/entities/query?query=Definition.Name=='M.Script'
    var filter = new DefinitionQueryFilter();
    filter.Names = new List<string> { "M.Script" };

    var query = new Query
    {
        Filter = filter
    };

    var scripts = MClient.Querying.QueryIdsAsync(query).Result;
    return scripts.Items.ToList();
}

/// <summary>
/// Gets a single script (does not include references like the actual C# code for the script)
/// </summary>
/// <param name="baseUrl">The part of the url with server name up until the first slash</param>
/// <param name="id">id of the script to get</param>
/// <returns></returns>
/// <remarks>This is an example of how to use the Raw method on the WebClient SDK</remarks>
public string GetScriptAsJson(string baseUrl, long id)
{
    return GetScriptAsJson(baseUrl, id, string.Empty);
}

/// <summary>
/// Gets a single script (does not include references like the actual C# code for the script)
/// </summary>
/// <param name="id">id of the script to get</param>
/// <param name="culture"></param>
/// <returns></returns>
/// <remarks>This is an example of how to use the Raw method on the WebClient SDK</remarks>
public string GetScriptAsJson(string baseUrl, long id, string culture)
{
    var absouluteUrl = (string.IsNullOrEmpty(culture)
                            ? string.Format("{0}/api/entities/{1}?culture=en-US", baseUrl, id, culture)
                            : string.Format("{0}/api/entities/{1}", baseUrl, id));

    var headers = new Dictionary<string, string>();
    headers.Add("Accept", "application/json");
    headers.Add("X-ApiVersion", "3");
    headers.Add("minimalSchema", "true");

    return MClient.Raw.GetAsync(absouluteUrl, headers).Result.Content.ReadAsStringAsync().Result;
}

public async Task<IList<IEntity>> GetAllScripts()
{
    //  This is the API to get all scripts /api/entities/query?query=Definition.Name=='M.Script'
    var filter = new DefinitionQueryFilter();
    filter.Names = new List<string> { "M.Script" };

    var query = new Query
    {
        Filter = filter  
    };

    var relationLoadOption = new RelationLoadOption(new string[] { "ScriptToActiveScriptContent" });
    var loadConfig = new EntityLoadConfiguration(CultureLoadOption.Default, PropertyLoadOption.All, relationLoadOption);
    var result = await MClient.Querying.QueryAsync(query, loadConfig).ConfigureAwait(false);
    return result.Items.Where(x => !x.IsSystemOwned && (x.CreatedBy != 6 && x.ModifiedBy != 6)).ToList(); //filter unchanged system owned scripts
}

public string GetScriptFriendlyName(IEntity script)
{
    if(script == null || script.Id == null)
    {
        return null;
    }

    return script.Id + "_" + script.Identifier.Replace("-", "_");
}
