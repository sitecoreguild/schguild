/********************************************************************************
 * Sitecore Content Hub CD/CI
 * 
 * This script loads the MClient to allow you to run scripts locally as if they were in Content Hub.
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

#r "nuget:Stylelabs.M.Sdk.WebClient"

#load "Settings.csx"            // Contains the instance specific settings for your environment

using System;
using Stylelabs.M.Sdk.WebClient;
using Stylelabs.M.Sdk.WebClient.Authentication;

var MClient = GetMClient(baseUrl, clientId, clientSecret, userName, password);

public IWebMClient GetMClient(string baseUrl, string clientId, string clientSecret, string userName, string password) 
{
    var oauth = new OAuthPasswordGrant
    {
        ClientId = clientId,
        ClientSecret = clientSecret,
        UserName = userName,
        Password = password
    };

    return MClientFactory.CreateMClient(new Uri(baseUrl), oauth);
}