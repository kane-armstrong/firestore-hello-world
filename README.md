# firestore-hello-world
 
1. Create a Firebase project
2. Provision a Firestore database (test mode)
3. Browse to the IAM & admin Service accounts page ([here](https://console.developers.google.com/projectselector/iam-admin/serviceaccounts)
4. (Optional) Create a new user
5. Create a json key (using the Actions context menu) for the new user (or an existing firebase-adminsdk user)

Handling credentials

* Put the json blob in your DevOps tool, if it supports that many characters in a secret
* Put the json blob in KeyVault (max of 25k bytes, key should be less than 5k bytes)
* Pass the setting through as an environment variable when starting the app (convention seems to be to use the 
GOOGLE_APPLICATION_CREDENTIALS environment variable, but in this setup you would use 
FirestoreOrderNotifierOptions__JsonCredentials)
