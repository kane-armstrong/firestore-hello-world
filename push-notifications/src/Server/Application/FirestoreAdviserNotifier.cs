using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Server.Application
{
    public class FirestoreAdviserNotifier : IAdviserNotifier
    {
        private readonly FirestoreDb _db;

        public FirestoreAdviserNotifier(FirestoreDb db)
        {
            _db = db;
        }

        public async Task NotifyVisitorArrived(int adviserId, VisitorDetails visitor)
        {
            var document = _db.Collection("advisers").Document($"adviser-{adviserId}-visitor-arrivals");
            var content = new Dictionary<string, object>
            {
                { "Name", visitor.Name },
                { "ReasonForVisit", visitor.ReasonForVisit },
                { "RequiresParkingExitPass", visitor.RequiresParkingExitPass}
            };
            await document.SetAsync(content);
        }
    }
}