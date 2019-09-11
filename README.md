# firestore-hello-world
 
1. Create a Firebase project
2. Provision a Firestore database (NOT test mode)
3. Browse to the *IAM & admin* 'Service accounts' page ([here](https://console.developers.google.com/projectselector/iam-admin/serviceaccounts)
4. (Optional) Create a new user
5. Create a json key (using the Actions context menu) for the new user (or the existing firebase-adminsdk user)
6. Manually copy the file to the root directory of the API and command line projects

Firestore database rules:

````
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    match /{document=**} {
      allow read, write: if false;
    }
  }
}
````

This is pretty unhelpful for a JavaScript client though

# Handling credentials on the server side

This is configured to take a json key file as exported from the Google console.

* If using Kubernetes, you could set this as a secret in there
* Could also inject/use volumes/etc to get the json key to the container
* Put the json blob in KeyVault (max of 25k bytes, key seems to generally be less than 5k bytes)
* Avoid environment variables unless those can be permanently masked in whatever you're shipping to
