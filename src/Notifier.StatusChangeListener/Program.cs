using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notifier.StatusChangeListener
{
    public class Program
    {
        private const string ProjectId = "kanes-sandbox";

        public static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            Console.Write("Initializing database and collection listener...");
            var db = await InitializeDb();
            var document = db.Collection("locations").Document("1");
            var listener = document.Listen(snapshot =>
            {
                Console.WriteLine($"Received document changed event. Document ID: {snapshot.Id}");
                if (!snapshot.Exists)
                {
                    Console.WriteLine("The document was deleted");
                    return;
                }

                Console.WriteLine($"Document contents:");
                var contents = snapshot.ToDictionary();
                foreach (var (key, value) in contents)
                {
                    Console.WriteLine($"{key}: {value}");
                }
            });

            Console.WriteLine("done. Press any key to exit.");

            Console.ReadKey();

            await listener.StopAsync();
        }

        private static async Task<FirestoreDb> InitializeDb()
        {
            var builder = new FirestoreClientBuilder
            {
                JsonCredentials = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "firestore_key.json"))
            };

            var client = await builder.BuildAsync();

            return await FirestoreDb.CreateAsync(ProjectId, client);
        }
    }
}