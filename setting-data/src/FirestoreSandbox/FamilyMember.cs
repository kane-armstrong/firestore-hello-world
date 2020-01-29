using Google.Cloud.Firestore;

namespace FirestoreSandbox
{
    [FirestoreData]
    public class FamilyMember
    {
        [FirestoreProperty("name")]
        public string Name { get; set; }
        [FirestoreProperty("age")]
        public int Age { get; set; }
    }
}
