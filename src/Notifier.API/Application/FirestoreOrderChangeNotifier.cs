using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Notifier.API.Application
{
    public class FirestoreOrderChangeNotifier : IOrderChangeNotifier
    {
        private readonly FirestoreOrderNotifierOptions _options;

        public FirestoreOrderChangeNotifier(IOptions<FirestoreOrderNotifierOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            if (!_options.UseDevelopmentCredentials && string.IsNullOrWhiteSpace(_options.JsonCredentialsPath))
            {
                throw new ArgumentException(
                    "A JSON credentials file is required to authenticate with the Firebase project.",
                    nameof(FirestoreOrderNotifierOptions.JsonCredentialsPath));
            }
        }

        public async Task NotifyOrderChanged(int locationId, int orderId)
        {
            var db = await InitializeDb();
            var document = db.Collection("locations").Document(locationId.ToString());
            var content = new Dictionary<string, object>
            {
                {"LatestOrderId", orderId}
            };
            await document.SetAsync(content);
        }

        private async Task<FirestoreDb> InitializeDb()
        {
            var builder = new FirestoreClientBuilder
            {
                JsonCredentials = _options.UseDevelopmentCredentials
                    ? await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "firestore_key.json"))
                    : await File.ReadAllTextAsync(_options.JsonCredentialsPath)
            };

            var client = await builder.BuildAsync();

            return await FirestoreDb.CreateAsync(_options.ProjectId, client);
        }
    }
}