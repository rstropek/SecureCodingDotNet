# Protect Settings with ASP.NET Core

## Secret Manager

To run the sample in [09.1-SecretManager](09.1-SecretManager), perform the following steps:

* Change `UserSecretsId` in the *.csproj* file to a unique value (i.e. new Guid)
* Run the *.NET Secret Manager Tool* to add a setting: `dotnet user-secrets set "ping" "pong"`
* Take a look at the settings in *%APPDATA%\Microsoft\UserSecrets\...*
* Run the sample program
