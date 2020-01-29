using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace FirestoreSandbox
{
    [FirestoreData]
    public class Family
    {
        [FirestoreProperty(Name = "id", ConverterType = typeof(GuidConverter))]
        public Guid Id { get; set; }

        [FirestoreProperty(Name = "name")]
        public string Name { get; set; }

        [FirestoreProperty(Name = "members")]
        public List<FamilyMember> Members { get; set; }
    }
}
