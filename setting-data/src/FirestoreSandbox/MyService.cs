using Google.Cloud.Firestore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirestoreSandbox
{
    public class MyService : IHostedService
    {
        private readonly FirestoreDb _db;

        public MyService(FirestoreDb db)
        {
            _db = db;
        }

        private static readonly Family Family = new Family
        {
            Id = Guid.Parse("88fca3c0-0374-495b-97ee-b401a3cc8cbb"),
            Name = "Armstrong"
        };

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await CreateFamily();
            await UpdateFamilyMembers();
            await UpdateSpecificFamilyMember();
        }

        private async Task CreateFamily()
        {
            await _db.RunTransactionAsync(async transaction =>
            {
                var doc = _db.Collection("families").Document(Family.Id.ToString());
                var snapshot = await doc.GetSnapshotAsync();
                if (snapshot.Exists) await doc.DeleteAsync();
                transaction.Set(doc, Family);
            });
        }

        private async Task UpdateFamilyMembers()
        {
            await _db.RunTransactionAsync(async transaction =>
            {
                var doc = _db.Collection("families").Document(Family.Id.ToString());

                var members = new List<FamilyMember>
                {
                    new FamilyMember
                    {
                        Name = "Person1",
                        Age = 30
                    },
                    new FamilyMember
                    {
                        Name = "Person2",
                        Age = 31
                    }
                };
                var documentUpdates = new Dictionary<string, object>
                {
                    { "members", members }
                };

                transaction.Update(doc, documentUpdates);
            });
        }

        private async Task UpdateSpecificFamilyMember()
        {
            await _db.RunTransactionAsync(async transaction =>
            {
                var doc = _db.Collection("families").Document(Family.Id.ToString());
                var snapshot = await doc.GetSnapshotAsync();
                var family = snapshot.GetValue<FamilyMember[]>("members");
                var toUpdate = family[1];
                toUpdate.Age++;
                family[1] = toUpdate;

                var documentUpdates = new Dictionary<string, object>
                {
                    { "members", family }
                };

                transaction.Update(doc, documentUpdates);
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}