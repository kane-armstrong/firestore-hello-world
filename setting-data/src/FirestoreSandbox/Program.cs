using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FirestoreSandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    services.AddOptions();
                    services.Configure<FirebaseOptions>(configuration.GetSection("firebase"));

                    services.AddSingleton(c =>
                    {
                        var config = configuration.GetSection("firebase").Get<FirebaseOptions>();
                        var credentials = GoogleCredential.FromJson(Encoding.UTF8.GetString(Convert.FromBase64String(config.Credentials)));
                        var channel = new Channel(FirestoreClient.DefaultEndpoint.Host, FirestoreClient.DefaultEndpoint.Port, credentials.ToChannelCredentials());
                        var client = FirestoreClient.Create(channel);
                        return FirestoreDb.Create(config.ProjectId, client);
                    });

                    services.AddHostedService<MyService>();
                })
                .RunConsoleAsync();
        }
    }
}
