# Android-Storage
Wrapper for StorageFolder and StorageFile

# Hot to use
```csharp
StorageFolder downloads = DevelopersAzteca.Storage.KnownFolders.Downloads;
List<StorageFile> filesondownloads = downloads.GetFiles();
foreach(var file in filesondownloads){
    System.Diagnostics.Debug.WriteLine(file.Name);
}
```
