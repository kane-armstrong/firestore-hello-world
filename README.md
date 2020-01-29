# Firestore Sandbox

This repository contains a series of projects aimed at playing with and getting a feel for working with Google Cloud 
Firestore.

## Saving Data to Firestore

Here I play around with patterns for saving data to Firestore. Inspired by serialization exceptions encountered while
attempting to save a class annotated with `FirestoreData` containing a `List` property.

## Push Notifications via Firestore
 
Use case: I have an appointment with an adviser, and there are several of them. To streamline the check-in process, and 
since the company doesn't have any reception staff, there is a kiosk where I can check in. My visit is recorded on the
server side for reporting purposes, and each adviser has a device on their desk that notifies them that they have a 
visitor. Only the adviser who has the visitor should be notified, and the notification should be received without delay.

This is obviously contrived -- just an excuse to toy with Firestore as a mechanism for push notifications.

### Setup

I created a Firebase project, a Firestore database on said project, exported the Firebase Admin SDK credentials
(Project Settings > Service accounts), base64 encoded the exported json, and copied that into appsettings.json.

### Usage

1. Run the server project using `dotnet run` from the csproj directory
2. Run the client using `dotnet run base64creds` from the csproj directory
3. Run the kiosk project using `dotnet run` from the csproj directory
4. Checkin on the kiosk project
5. Observe the notification on the client project

### Realistically handling credentials on the server side

1. Save the credentials (json) to disk and load the file in process - obviously don't check that into version control. You 
could pass it in in a CI/CD pipeline (e.g. mount it in k8s)
2. Pass the credentials through as a secret, may want to base64 encode to make this easier
