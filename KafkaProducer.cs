using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal static class KafkaProducer
    {
        
            internal static async Task SendTemperatureMessageAsync()
            {
                string ksqlServerUrl = "http://localhost:8088/ksql";

                // List of towns
                List<string> towns = new List<string> { "Varazdin", "Zagreb", "Split", "Rijeka" };

                // Select a random town and generate a random temperature
                string randomTown = towns[new Random().Next(towns.Count)];
                double randomTemperature = new Random().NextDouble() * 40 - 20; // Generating a random temperature between -20 and 20

                // Set the ksql query and headers
                string ksqlQuery = $"INSERT INTO temperature (town, temperature) VALUES ('{randomTown}', {randomTemperature});";

                // Create a class to represent the payload
                var payload = new
                {
                    ksql = ksqlQuery,
                    streamsProperties = new Dictionary<string, string>()
                };

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/vnd.ksql.v1+json");

                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        // Make a POST request
                        HttpResponseMessage response = await httpClient.PostAsync(ksqlServerUrl, content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            //Console.WriteLine("Success");
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

