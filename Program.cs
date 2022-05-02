using System;
using Newtonsoft.Json;
using RestSharp;

/*
This program was created by Chananel Zaguri in Google cloud shell editor 
The program get the current weather According to the user's requirement.
 if the program manages to get the data it will return 0 otherwise -  0xA0 (160)
*/

namespace consoleApp1
{
    class Program
    {   
        private const int SUCCESS = 0;
        private const int ERROR_BAD_ARGUMENTS = 0xA0;
        private const int ERROR = 0x667;

        /*
        getWeatherData gets a location and returns the weather in this place.
        This method use hellp class JsonConvert to process the information from the server
        */
        public int getWeatherData(string location, string apiKey , string units = "metric"){
            var client = new RestClient(@"http://api.openweathermap.org/data/2.5/weather?q="+location+"&units="+units+"&APPID="+apiKey);
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

            System.Console.WriteLine( location + "|"+ myDeserializedClass.main.temp + "|" + myDeserializedClass.wind.speed + "|" + myDeserializedClass.main.humidity + "|" + myDeserializedClass.main.pressure);

            Environment.ExitCode = SUCCESS;
            return SUCCESS;

        }

     
         /*
             Main - check the input according to the Instructions and use getWeatherData to print a message if the operation was successful or not       
         */
        static int Main(string[] args)
        {
            Environment.SetEnvironmentVariable("OPENWEATHER_API_KEY", "9b9f3d1fc17173538e8c3bd4abe3941d");

            Program program = new Program();

            if (args.Length != 2 && args.Length != 4)
            {
                System.Console.WriteLine("Error | check the number of the parameters");
                Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                return ERROR_BAD_ARGUMENTS;
            }

            if (args.Length == 2)
            {
               
                if (args[0] == "--city" || args[0] == "-c" )
                {
                    try
                    {
                        return program.getWeatherData(args[1],Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY"));
                    }
                    catch (System.Exception)
                    {
                        System.Console.WriteLine("Error | check the city name");
                        Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                        return ERROR_BAD_ARGUMENTS;
                    }
                }
                else
                {
                    System.Console.WriteLine("Error | enter --city or -c before the city name");
                    Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                    return ERROR_BAD_ARGUMENTS;
                }
            }

            if (args.Length == 4)
            {
                if ((args[0] == "--city" || args[0] == "-c") && (args[2] == "--units" || args[2] == "-u"))
                {
                    if (args[3] != "metric" && args[3] != "imperial")
                    {
                        System.Console.WriteLine("Error | enter metric or imperial after --units or -u ");
                        Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                        return ERROR_BAD_ARGUMENTS;
                    }
                    try
                    {
                        return program.getWeatherData(args[1],Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY"),args[3]);
                    }
                    catch (System.Exception)
                    {
                        System.Console.WriteLine("Error | check the city name");
                        Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                        return ERROR_BAD_ARGUMENTS;
                    }
                }
                else
                {
                    System.Console.WriteLine("Error | enter --city or -c before the city name and --units or -u before units name");
                    Environment.ExitCode = ERROR_BAD_ARGUMENTS;
                    return ERROR_BAD_ARGUMENTS;
                }
            }
            return ERROR;
        }
    }
}


