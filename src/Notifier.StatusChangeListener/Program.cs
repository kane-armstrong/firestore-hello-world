using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace Notifier.StatusChangeListener
{
    class Program
    {
        private const string projectId = "kanes-sandbox";
        
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        static async Task Run()
        {
            var db = await InitializeDb();
            // todo
        }
        
        static async Task<FirestoreDb> InitializeDb()
        {
            var builder = new FirestoreClientBuilder
            {
                JsonCredentials = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "firestore_key.json"))
            };

            var client = await builder.BuildAsync();
            
            var db = await FirestoreDb.CreateAsync(projectId, client);
            return db;
        }
    }
}