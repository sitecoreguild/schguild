/********************************************************************************
 * Sitecore Content Hub C# Script Samples
 * 
 * This script provides a way to run the sample C# Scripts to 
 * perform tasks in Content Hub. The script samples can be copied to content hub 
 * and used or you can adapt them slightly for use in your C# projects.
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
 * - Copy the contents of this folder to your local directory.
 * - Next you open a command prompt and cd to your local directory.
 * - Uncomment the Sample you wish to run.
 * - Ensure your the nuget folder has your packages in it for WebClient SDK.
 * - Next you execute the script using this command:
 *
 * Dotnet-Script -d "RunMe.csx" -s nuget
 *
 * - Alternatively you can run this in Visual Studio Code by selecting this file and choosing Debug and Run.
 *   To debug you will need to set up Visual Studio Code for debugging.
 * 
 * KNOWN ISSUES: 
 * 
 ********************************************************************************/

#load "Settings.csx"            // Contains the instance specific settings for your environment
#load "MClient.csx"             // Instantiates the MClient object for use by other scripts

// - These C# script files contain the samples so include the ones you wish to run.
// - Within each .csx file at the top are the sample calls to the functions. 
// - Uncomment the ones you want to try.
// - Some modules do not have API samples but discuss what these items are and 
//   direct you to documentation on how they work.
