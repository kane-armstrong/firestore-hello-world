using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kiosk
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            while (true)
            {
                Console.WriteLine("Press any key except 'q' to checkin. Press 'q' to quit.");
                var input = Console.ReadKey();
                if (input.KeyChar == 'q')
                {
                    break;
                }
                Console.WriteLine();

                await HandleCheckin();
            }
        }

        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:54333")
        };

        private static async Task HandleCheckin()
        {
            var name = PromptForName();
            var reasonForVisit = PromptForReasonForVisit();
            var requiresExitPass = PromptForRequiresExitPass();
            Console.Write("Checking in... ");

            var payload = new
            {
                Name = name,
                ReasonForVisit = reasonForVisit,
                RequiresParkingExitPass = requiresExitPass
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "api/checkin?adviserId=1")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request failed with status {response.StatusCode}: {content}");
            }
            Console.WriteLine("done.");
            Console.WriteLine();
        }

        private static bool PromptForRequiresExitPass()
        {
            bool requiresExitPass = default;
            while (true)
            {
                Console.Write("Do you require a parking exist pass? (y/n): ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (input == 'y')
                {
                    requiresExitPass = true;
                    break;
                }

                if (input == 'n')
                {
                    break;
                }

                Console.WriteLine();
            }

            return requiresExitPass;
        }

        private static string PromptForReasonForVisit()
        {
            Console.Write("Reason for visit: ");
            var reasonForVisit = Console.ReadLine();
            return reasonForVisit;
        }

        private static string PromptForName()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            return name;
        }
    }
}
