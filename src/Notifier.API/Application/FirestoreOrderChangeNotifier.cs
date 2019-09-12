using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifier.API.Application
{
    public class FirestoreOrderChangeNotifier : IOrderChangeNotifier
    {
        private readonly FirestoreDb _db;

        public FirestoreOrderChangeNotifier(FirestoreDb db)
        {
            _db = db;
        }

        public async Task NotifyOrderChanged(int locationId, int orderId)
        {
            var document = _db.Collection("locations").Document(locationId.ToString());
            var content = new Dictionary<string, object>
            {
                {"LastOrderId", orderId}
            };
            await document.SetAsync(content);
        }
    }
}