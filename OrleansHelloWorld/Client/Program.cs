using System;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System.Threading.Tasks;
using GrainInterfaces;

namespace Client
{
    public class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nВиняток пiд час спроби запустити Client: {e.Message}");
                Console.WriteLine("Переконайтеся, що працює Silo, до якого намагається пiдключитися Client.");
                Console.WriteLine("\nНатиснiть будь-яку клавiшу для виходу.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Успiшне пiдключення до silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Привiт!");
            Console.WriteLine("\n\n{0}\n\n", response);
        }
    }
}
