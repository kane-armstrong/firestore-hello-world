using System;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace Client
{
    public class Program
    {
        private const string ProjectId = "kanes-sandbox";

        public static void Main(string[] args)
        {
            Run(args[0]).GetAwaiter().GetResult();
        }

        private static async Task Run(string credentials)
        {
            Console.Write("Initializing...");
            var db = await InitializeDb(credentials);
            Console.WriteLine("done. Press any key to exit");
            Console.WriteLine();

            var document = db.Collection("advisers").Document("adviser-1-visitor-arrivals");
            var listener = document.Listen(snapshot =>
            {
                if (!snapshot.Exists)
                {
                    return;
                }
                Console.WriteLine("You have a visitor! Their details:");
                var contents = snapshot.ToDictionary();
                foreach (var (key, value) in contents)
                {
                    Console.WriteLine($"- {key}: {value}");
                }
            });
            
            Console.ReadKey();

            await listener.StopAsync();
        }

        private static async Task<FirestoreDb> InitializeDb(string credentials)
        {
            var builder = new FirestoreClientBuilder
            {
                JsonCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials))
            };

            var client = await builder.BuildAsync();

            return await FirestoreDb.CreateAsync(ProjectId, client);
        }
    }
}