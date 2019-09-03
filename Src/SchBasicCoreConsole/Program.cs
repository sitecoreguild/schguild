/********************************************************************************
 * Sitecore Content Hub API Demo .NET Core Console Application
 * 
 * This application provides samples on how to call various REST APIs related to Sitecore Content Hub.
 * In order to use this application you require the endpoint or base url to your instance of Sitecore Content Hub.
 * You will also require the API token. This can be obtained by doing the following:
 * 
 * - Log into your Sitecore Content Hub instance.
 * - Clicking on the Manage menu option
 * - Click on the Users button.
 * - Beside your user click on the KEY icon. If you do not have a key then you can generate one. If you have one this will prompt you to create a new one.
 *   NOTE: Creating a new one will invalidate the previous one so be careful if other applications are using this token.
 * - Copy this token and that is what you will pass to the argument list of the application.
 * 
 * The code in this application is meant to be a sample and may not have all the protection code 
 * necessary for production environments.  
 * 
 * APIS SAMPLES INCLUDED (More will be included in future versions):
 * 
 *  - STATUS: https://docs.stylelabs.com/content/integrations/rest-api/status.html
 *  - OPTIONLIST: https://docs.stylelabs.com/content/integrations/rest-api/option-lists/intro.html
 *                   
 ********************************************************************************/

namespace SchBasicCoreConsole
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            var baseUrl = string.Empty;
            var token = string.Empty;
            var taskname = string.Empty;

            Console.WriteLine(@"________      _____      _____      ________      .__.__       .___ ");
            Console.WriteLine(@"\______ \    /  _  \    /     \    /  _____/ __ __|__|  |    __| _/ ");
            Console.WriteLine(@" |    |  \  /  /_\  \  /  \ /  \  /   \  ___|  |  \  |  |   / __ |  ");
            Console.WriteLine(@" |    `   \/    |    \/    Y    \ \    \_\  \  |  /  |  |__/ /_/ |  ");
            Console.WriteLine(@"/_______  /\____|__  /\____|__  /  \______  /____/|__|____/\____ |  ");
            Console.WriteLine(@"        \/         \/         \/          \/                    \/  ");

            if (DateTime.Now.Year < 2020)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to 2019 Year of the Marketing Flood!");
                Console.WriteLine("Relief will happen in 2020 Year of the DAM!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to 2020 Year of the DAM!");
                Console.WriteLine();
            }

            // if no arguments then prompt for the base url
            if ((args.Length < 2))
            {
                Console.WriteLine("Please pass the base url for Sitecore Content Hub eg http://mycompany.stylelabs.io as argument.");
                Console.WriteLine("Argument should be in one of these forms:");
                Console.WriteLine();
                Console.WriteLine("SchBasicCoreConsole {base_url} {token}");
                Console.WriteLine("SchBasicCoreConsole {base_url} {token} {task_name}");
                Console.ReadLine();
                return;
            }

            baseUrl = args[0];
            token = args[1];

            if (args.Length > 2)
            {
                taskname = args[2];
            }

            // if the user didn't supply a token or a username and password then we cannot continue
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please pass the base url for Sitecore Content Hub eg http://mycompany.stylelabs.io as argument.");
                Console.WriteLine("Argument should be in one of these forms:\r\n");
                Console.WriteLine();
                Console.WriteLine("SchBasicCoreConsole {base_url} {token}");
                Console.WriteLine("SchBasicCoreConsole {base_url} {token} {task_name}");
                Console.ReadLine();
                return;
            }

            // if we have a task then run it
            if (string.IsNullOrEmpty(taskname) == false)
            {
                ExecuteTask(baseUrl, token, taskname);
            }
            // else we show the menu and let them choose different tasks
            else
            {
                while (string.IsNullOrEmpty(taskname))
                {
                    taskname = ShowMenu();
                    taskname = ExecuteTask(baseUrl, token, taskname);
                }
            }
        }

        /// <summary>
        /// Executes a task against the Sitecore Content Hub API
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="token"></param>
        /// <param name="taskname"></param>
        /// <returns></returns>
        public static string ExecuteTask(string baseUrl, string token, string taskname)
        {
            var jsonResults = string.Empty;

            if (taskname.ToUpperInvariant().Trim() == "EXIT") return taskname;

            // This is because the menus may change the task and then we look to see if the choice means an execution
            var actualTaskName = taskname.ToUpperInvariant().Trim();

            if (actualTaskName.StartsWith("STATUS_MENU")) actualTaskName = ShowStatusMenu();
            if (actualTaskName.StartsWith("OPTION_LIST_MENU")) actualTaskName = ShowOptionListMenu();
            if (actualTaskName.StartsWith("QUERYING_MENU")) actualTaskName = ShowQueryingMenu();

            if (actualTaskName == "EXIT") return actualTaskName;

            if (actualTaskName.StartsWith("STATUS_")) return ExecuteStatusTask(baseUrl, token, actualTaskName);
            if (actualTaskName.StartsWith("OPTION_LIST_")) return ExecuteOptionListTask(baseUrl, token, actualTaskName);
            if (actualTaskName.StartsWith("QUERYING_")) return ExecuteQueryingTask(baseUrl, token, actualTaskName);

            return string.Empty;
        }

        /// <summary>
        /// This function outputs the menu for tasks
        /// </summary>
        public static string ShowMenu()
        {
            Console.WriteLine("Please enter the number beside each task or EXIT to quit:");
            Console.WriteLine("");
            Console.WriteLine("-1. Previous Menu");
            Console.WriteLine("1. STATUS_MENU - Status Menu");
            Console.WriteLine("2. OPTION_LIST_MENU - Option List Menu");
            Console.WriteLine("3. QUERYING_MENU - Querying Menu");

            switch (Console.ReadLine())
            {
                case "-1": return "PREVIOUS";
                case "1": return "STATUS_MENU";
                case "2": return "OPTION_LIST_MENU";
                case "3": return "QUERYING_MENU";
                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }
        }

        #region Status Related - https://docs.stylelabs.com/content/integrations/rest-api/status.html

        /// <summary>
        /// This function outputs the menu for Status API related tasks
        /// </summary>
        public static string ShowStatusMenu()
        {
            Console.WriteLine("Please enter the number beside each task or EXIT to quit:");
            Console.WriteLine("");
            Console.WriteLine("-1. Previous Menu");
            Console.WriteLine("1. STATUS_STATUS - Status Overview");
            Console.WriteLine("2. STATUS_DATA_STORAGE - Status and statistics on the storage layer");
            Console.WriteLine("3. STATUS_GRAPH - Status of graph server and all its services");
            Console.WriteLine("4. STATUS_JOBS - Status of the processing jobs");
            Console.WriteLine("5. STATUS_KPIS - Key performance indicator state");
            Console.WriteLine("6. STATUS_LICENSES - State of your license");
            Console.WriteLine("7. STATUS_QUEUE - Status and statistics related to message queue");
            Console.WriteLine("8. STATUS_SEARCH - Status of the search services");
            Console.WriteLine("9. STATUS_SERVICE_STATUS - Detailed service status explaining the aggregated status");
            Console.WriteLine("10. STATUS_USERS - Status of the user base");
            Console.WriteLine("");

            switch (Console.ReadLine())
            {
                case "1": return "STATUS_STATUS";
                case "2": return "STATUS_DATA_STORAGE";
                case "3": return "STATUS_GRAPH";
                case "4": return "STATUS_JOBS";
                case "5": return "STATUS_KPIS";
                case "6": return "STATUS_LICENSES";
                case "7": return "STATUS_QUEUES";
                case "8": return "STATUS_SEARCH";
                case "9": return "STATUS_SERVICE_STATUS";
                case "10": return "STATUS_USERS";
                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Executes a task related to the Status API
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname">Status related API to call</param>
        /// <returns></returns>
        public static string ExecuteStatusTask(string baseUrl, string token, string taskname)
        {
            var jsonResults = string.Empty;

            // Currently only has status related tasks but later will have other types of tasks
            switch (taskname.ToUpperInvariant().Trim())
            {
                case "STATUS_STATUS":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_DATA_STORAGE":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_GRAPH":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_JOBS":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_KPIS":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_LICENSES":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_QUEUES":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_SEARCH":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_SERVICE_STATUS":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "STATUS_USERS":
                    jsonResults = CheckStatus(baseUrl, token, taskname.ToUpperInvariant().Trim());
                    break;
                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }

            // Look at Json and return any properties found. 
            // This is where you would put your own logic. This is here for an example.
            var root = JObject.Parse(jsonResults);
            var allEntityTokens = new List<JToken>();

            if (root.Children().Count() > 1)
            {
                var allChildren = root.Children().ToList();

                for (int fieldIndex = 0; fieldIndex < allChildren.Count(); fieldIndex++)
                {
                    if (allChildren[fieldIndex].Type == JTokenType.Property)
                    {
                        var property = allChildren[fieldIndex] as JProperty;
                        Console.WriteLine("PROPERTY:" + property.Name + " = " + property.Value);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="statusType">The type of status to return</param>
        /// <returns>JSON result containing the status</returns>
        public static string CheckStatus(string baseUrl, string token, string statusType)
        {
            bool requiresToken = false;
            var absouluteUrl = baseUrl;
            switch(statusType.ToUpperInvariant().Trim())
            {
                case "STATUS_DATA_STORAGE":
                    absouluteUrl = baseUrl + "/api/status/datastorage";
                    requiresToken = true;
                    break;
                case "STATUS_GRAPH":
                    absouluteUrl = baseUrl + "/api/status/graph";
                    requiresToken = true;
                    break;
                case "STATUS_JOBS":
                    absouluteUrl = baseUrl + "/api/status/jobs";
                    requiresToken = true;
                    break;
                case "STATUS_KPIS":
                    absouluteUrl = baseUrl + "/api/status/kpis";
                    requiresToken = true;
                    break;
                case "STATUS_LICENSES":
                    absouluteUrl = baseUrl + "/api/status/licenses";
                    requiresToken = true;
                    break;
                case "STATUS_QUEUES":
                    absouluteUrl = baseUrl + "/api/status/queues";
                    requiresToken = true;
                    break;
                case "STATUS_SEARCH":
                    absouluteUrl = baseUrl + "/api/status/search";
                    requiresToken = true;
                    break;
                case "STATUS_SERVICE_STATUS":
                    absouluteUrl = baseUrl + "/api/status/servicestatus";
                    requiresToken = true;
                    break;
                case "STATUS_USERS":
                    absouluteUrl = baseUrl + "/api/status/users";
                    requiresToken = true;
                    break;
                default:
                    absouluteUrl = baseUrl + "/api/status";
                    break;
            }

            var request = (HttpWebRequest)WebRequest.Create(absouluteUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            if (requiresToken) request.Headers.Add("X-Auth-Token", token);

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        #endregion

        #region Option List Related - https://docs.stylelabs.com/content/integrations/rest-api/option-lists/intro.html

        /// <summary>
        /// This function outputs the menu for tasks
        /// </summary>
        public static string ShowOptionListMenu()
        {
            Console.WriteLine("Please enter the number beside each task or EXIT to quit:");
            Console.WriteLine("");
            Console.WriteLine("-1. Previous Menu");
            Console.WriteLine("1. OPTION_LIST_GET - Gets a list of Option Lists");
            Console.WriteLine("2. OPTION_LIST_GET_ACTION_TYPES - Gets ActionType Option List");
            Console.WriteLine("3. OPTION_LIST_GET_PACKAGING_TYPE - Gets PackagingType Option List");
            Console.WriteLine("4. OPTION_LIST_CREATE - Creates an Option List");
            Console.WriteLine("");
            switch (Console.ReadLine())
            {
                case "1": return "OPTION_LIST_GET";
                case "2": return "OPTION_LIST_GET_ACTION_TYPES";
                case "3": return "OPTION_LIST_GET_PACKAGING_TYPE";
                case "4": return "OPTION_LIST_CREATE";

                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Executes a task related to the Option List API
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname">Option List related API to call</param>
        /// <returns></returns>
        public static string ExecuteOptionListTask(string baseUrl, string token, string taskname)
        {
            var jsonResults = string.Empty;

            // Currently only has status related tasks but later will have other types of tasks
            switch (taskname.ToUpperInvariant().Trim())
            {
                // (COMING SOON) case "OPTION_LIST_CREATE":
                //    jsonResults = CreateOptionLists(baseUrl, token, "TestOption" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute);
                //    break;
                case "OPTION_LIST_GET":
                    jsonResults = GetOptionLists(baseUrl, token);
                    break;
                case "OPTION_LIST_GET_ACTION_TYPES":
                    jsonResults = GetOptionList(baseUrl, token, "ActionTypes");
                    break;
                case "OPTION_LIST_GET_PACKAGING_TYPE":
                    jsonResults = GetOptionList(baseUrl, token, "PackagingType");
                    break;
                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }

            // Look at Json and return any properties found. 
            // This is where you would put your own logic. This is here for an example.
            var root = JObject.Parse(jsonResults);
            var allEntityTokens = new List<JToken>();

            if (root.Children().Count() > 1)
            {
                var allChildren = root.Children().ToList();

                for (int fieldIndex = 0; fieldIndex < allChildren.Count(); fieldIndex++)
                {
                    if (allChildren[fieldIndex].Type == JTokenType.Property)
                    {
                        var property = allChildren[fieldIndex] as JProperty;
                        Console.WriteLine("PROPERTY:" + property.Name + " = " + property.Value);
                    }
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// Creates an option list given the name (NOT TESTED YET)
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="token"></param>
        /// <param name="optionListName"></param>
        /// <returns></returns>
        public static string CreateOptionList(string baseUrl, string token, string optionListName)
        {
            throw new NotImplementedException("This method is on the roadmap but is not tested yet");

            //var absouluteUrl = baseUrl + "/api/datasources";

            //var optionListToCreate = "{\"name\": \"" + optionListName + "\",\"labels\": { \"en-US\": \"Groceries\",\"nl-BE\": \"Boodschappen\"},\"type\": \"Hierarchical\",\"values\":[{\"identifier\" : \"Bread\",\"labels\": {\"en-US\": \"Bread\",\"nl-BE\": \"Brood\"},\"values\" : []},{\"identifier\" : \"Spreads\",\"values\" : [{\"identifier\" : \"Peanut butter\",\"values\" : []},{\"identifier\" : \"Jam\",\"values\" : []}]}],\"is_system_owned\": false}";

            //// Need to write that to body
            //var request = (HttpWebRequest)WebRequest.Create(absouluteUrl);
            //request.Method = "POST";
            //request.Accept = "application/json";
            //request.ContentType = "application/json";
            //request.Headers.Add("X-Auth-Token", token);

            //// Get the data that is being posted (or sent) to the server
            //var bytes = Encoding.ASCII.GetBytes(optionListToCreate);
            //request.ContentLength = bytes.Length;

            //// Get an output stream from the request object
            //var outputStream = request.GetRequestStream();

            //if (outputStream == null) return string.Empty;

            //// Post the data out to the stream
            //outputStream.Write(bytes, 0, bytes.Length);

            //// Close the output stream and send the data out to the web server
            //outputStream.Close();

            //var response = (HttpWebResponse)request.GetResponse();
            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //return responseString;

        }

        /// <summary>
        /// Gets an option list based on its name
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="optionListName">Name of the option list to get</param>
        /// <returns>If not found will return empty string</returns>
        public static string GetOptionLists(string baseUrl, string token)
        {
            var absouluteUrl = baseUrl + "/api/datasources";

            var request = (HttpWebRequest)WebRequest.Create(absouluteUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("X-Auth-Token", token);

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        /// <summary>
        /// Gets an option list based on its name
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="optionListName">Name of the option list to get</param>
        /// <returns>If not found will return empty string</returns>
        public static string GetOptionList(string baseUrl, string token, string optionListName)
        {
            if (string.IsNullOrEmpty(optionListName)) throw new ArgumentException("ERROR: optionListName is required to look up option list");
            var absouluteUrl = baseUrl + "/api/datasources/" + optionListName;

            var request = (HttpWebRequest)WebRequest.Create(absouluteUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("X-Auth-Token", token);

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        #endregion

        #region Querying Related - https://docs.stylelabs.com/content/integrations/rest-api/query/introduction.html

        /// <summary>
        /// This function outputs the menu for Querying for Entities API related tasks
        /// </summary>
        public static string ShowQueryingMenu()
        {
            Console.WriteLine("Please enter the number beside each task or EXIT to quit:");
            Console.WriteLine("");
            Console.WriteLine("-1. Previous Menu");
            Console.WriteLine("1. QUERYING_FOR_TITLE - Querying entities with a specific title");
            Console.WriteLine("2. QUERYING_FOR_INTEGER - Querying entities with a specific integer value");
            Console.WriteLine("3. QUERYING_FOR_LONG - Querying entities with a specific long value");
            Console.WriteLine("4. QUERYING_FOR_DECIMAL - Querying entities with a specific decimal value");
            Console.WriteLine("5. QUERYING_FOR_FLOAT - Querying entities with a specific float value");
            Console.WriteLine("6. QUERYING_FOR_BOOLEAN - Querying entities with a specific boolean value");
            Console.WriteLine("7. QUERYING_FOR_CUSTOM_QUERY_CLAUSE - Querying entities via a custom query clause");
            Console.WriteLine("");

            switch (Console.ReadLine())
            {
                case "1": return "QUERYING_FOR_TITLE";                    
                case "2": return "QUERYING_FOR_INTEGER";
                case "3": return "QUERYING_FOR_LONG";
                case "4": return "QUERYING_FOR_DECIMAL";
                case "5": return "QUERYING_FOR_FLOAT";
                case "6": return "QUERYING_FOR_BOOLEAN";
                case "7": return "QUERYING_USING_CUSTOM_QUERY_CLAUSE";
                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Executes a task related to the Querying API
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname">Status related API to call</param>
        /// <returns></returns>
        public static string ExecuteQueryingTask(string baseUrl, string token, string taskname)
        {
            var jsonResults = string.Empty;
            var fieldName = string.Empty;
            var fieldValue = string.Empty;

            // Currently only has status related tasks but later will have other types of tasks
            switch (taskname.ToUpperInvariant().Trim())
            {
                case "QUERYING_FOR_TITLE":

                    Console.WriteLine("Please enter the title you wish to search for and press ENTER:");
                    var title = Console.ReadLine();
                    jsonResults = QueryingForStringField(baseUrl, token, taskname, "Title", title);
                    break;

                case "QUERYING_FOR_INTEGER":

                    Console.WriteLine("Please enter the name of the integer field and press ENTER:");
                    fieldName = Console.ReadLine();
                    Console.WriteLine("Please enter the integer value to search for and press ENTER:");
                    fieldValue = Console.ReadLine();
                    jsonResults = QueryingForIntegerField(baseUrl, token, taskname, fieldName, fieldValue);
                    break;

                case "QUERYING_FOR_LONG":

                    Console.WriteLine("Please enter the name of the long field and press ENTER:");
                    fieldName = Console.ReadLine();
                    Console.WriteLine("Please enter the long value to search for and press ENTER:");
                    fieldValue = Console.ReadLine();
                    jsonResults = QueryingForLongField(baseUrl, token, taskname, fieldName, fieldValue);
                    break;

                case "QUERYING_FOR_DECIMAL":

                    Console.WriteLine("Please enter the name of the decimal field and press ENTER:");
                    fieldName = Console.ReadLine();
                    Console.WriteLine("Please enter the decimal value to search for and press ENTER:");
                    fieldValue = Console.ReadLine();
                    jsonResults = QueryingForDecimalField(baseUrl, token, taskname, fieldName, fieldValue);
                    break;

                case "QUERYING_FOR_FLOAT":

                    Console.WriteLine("Please enter the name of the float field and press ENTER:");
                    fieldName = Console.ReadLine();
                    Console.WriteLine("Please enter the float value to search for and press ENTER:");
                    fieldValue = Console.ReadLine();
                    jsonResults = QueryingForFloatField(baseUrl, token, taskname, fieldName, fieldValue);
                    break;

                case "QUERYING_FOR_BOOLEAN":

                    Console.WriteLine("Please enter the name of the boolean field and press ENTER:");
                    fieldName = Console.ReadLine();
                    Console.WriteLine("Please enter the boolean value to search for and press ENTER:");
                    fieldValue = Console.ReadLine();
                    jsonResults = QueryingForBooleanField(baseUrl, token, taskname, fieldName, fieldValue);
                    break;

                case "QUERYING_USING_CUSTOM_QUERY_CLAUSE":

                    Console.WriteLine("Please enter the custom query clause and press ENTER:");
                    var queryClause = Console.ReadLine();
                    jsonResults = QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
                    break;

                case "EXIT":
                    return "EXIT";
                default:
                    return string.Empty;
            }

            // Look at Json and return any properties found. 
            // This is where you would put your own logic. This is here for an example.
            var root = JObject.Parse(jsonResults);
            var allEntityTokens = new List<JToken>();

            if (root.Children().Count() > 1)
            {
                var allChildren = root.Children().ToList();

                for (int fieldIndex = 0; fieldIndex < allChildren.Count(); fieldIndex++)
                {
                    if (allChildren[fieldIndex].Type == JTokenType.Property)
                    {
                        var property = allChildren[fieldIndex] as JProperty;
                        Console.WriteLine("PROPERTY:" + property.Name + " = " + property.Value);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForStringField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("String('{0}')=='{1}'", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }

        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForIntegerField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("Int('{0}')=={1}", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }

        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForLongField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("Long('{0}')=={1}", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }
        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForDecimalField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("Decimal('{0}')=={1}", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }
        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForFloatField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("Float('{0}')=={1}", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }
        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryingForBooleanField(string baseUrl, string token, string taskname, string fieldName, string fieldValue)
        {
            var queryClause = string.Format("Bool('{0}')=={1}", fieldName, fieldValue);
            return QueryUsingCustomQuery(baseUrl, token, taskname, queryClause);
        }
        /// <summary>
        /// For simplicity this poc has the static method here however it should be in a class
        /// </summary>
        /// <param name="baseUrl">The part of the url with server name up until the first slash</param>
        /// <param name="token">The Authentication Token for the API</param>
        /// <param name="taskname"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>JSON result containing the query results</returns>
        public static string QueryUsingCustomQuery(string baseUrl, string token, string taskname, string queryClause)
        {
            var absouluteUrl = string.Format("{0}/api/entities/query?query={1}", baseUrl, queryClause);

            var request = (HttpWebRequest)WebRequest.Create(absouluteUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("X-Auth-Token", token);

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        #endregion
    }
}
