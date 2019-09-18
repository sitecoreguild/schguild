/********************************************************************************
 * Sitecore Content Hub CD/CI Sc
 * 
 * This script contains the setting necessary to connect to your content hub instance.
 * It may also include other settings required by your scripts such as connection to other API or databases.
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to one of the 
 *          developers. This will ensure others using the application will benefit from this as well.
 *          
 *          christopher_williams@epam.com
 *          jose_dominguez@epam.com
 *          brent_pinkstaff@epam.com
 *          joe_bissol@epam.com
 *          
 * KNOWN ISSUES: 
 *
 *  
 ********************************************************************************/

// This is the base section of the url to your Sitecore Content Hub instance eg. https://contosa.stylelabs.io
var baseUrl = "https://{{INSERT_BASE_URL_HERE}";

// This is the client id provided in the OAuth Client section under Manage in your Sitecore Content Hub
var clientId = "{INSERT_CLIENT_ID_HERE}";

// This is the client secret provided in the OAuth Client section under Manage in your Sitecore Content Hub
var clientSecret = "{INSERT_CLIENT_SECRET_HERE}";

// This is a user able to login to Sitecore Content Hub. The permissions of this user determine data is returned.
var userName = "{INSERT_USER_NAME_HERE}";

// This is the password for the user specified in userName.
var password = "{INSERT_PASSWORD_HERE}";

// This is the location where the scripts are stored locally
var scriptDirectoryPath = "{INSERT_FULL_PATH_TO_DIRECTORY_TO_STORE_SCRIPTS_LOCALLY}";
