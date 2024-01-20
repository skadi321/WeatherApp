﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WeatherApp
{
    internal static class KafkaReciever
    {
            
        internal static async Task RecieveMessageAsync(TextBlock textBlock)
        {
            while (true)
            {
                string ksqlServerUrl = "http://localhost:8088/query";

                // Set the ksql query and headers
                string ksqlQuery = "{\"ksql\":\"SELECT * FROM temperature EMIT CHANGES LIMIT 1;\",\"streamsProperties\":{}}";
                var content = new StringContent(ksqlQuery, Encoding.UTF8, "application/vnd.ksql.v1+json");

                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    string result = "";
                    try
                    {
                        // Make a POST request
                        HttpResponseMessage response = await httpClient.PostAsync(ksqlServerUrl, content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the content as a string

                            result = await response.Content.ReadAsStringAsync();
                            result = result.Replace("\n", "");
                            JArray jsonArray = JArray.Parse(result);
                            textBlock.Text = jsonArray[1]["row"]["columns"][1].ToString();


                        }
                        else
                        {
                            Console.WriteLine($"Request failed with status code {response.StatusCode}. Response content:");
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                }
            }
        }
    }
}
