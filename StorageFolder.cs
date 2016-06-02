using System;
using System.Collections.Generic;
using System.IO;

namespace DevelopersAzteca.Storage
{
	public class StorageFolder
	{
		public StorageFolder ()
		{

		}

		public string FullPath {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public List<StorageFile> GetFiles(){
			List<StorageFile> files = new List<StorageFile> ();
			if (System.IO.Directory.Exists (FullPath)) {
				var enumeratefiles = Directory.EnumerateFiles (FullPath);
				foreach (var file in enumeratefiles) {
					files.Add(StorageFile.GetFileFromPath (file));
				}
			} else {
				throw new DirectoryNotFoundException ("No es posible obtener los archivos, al parecer, el directorio ya no existe");
			}
			return files;
		}

		public StorageFile GetFile(string filenamewithextension){
			if (System.IO.Directory.Exists (FullPath)) {
				var filepath = Path.Combine (FullPath, filenamewithextension);
				return StorageFile.GetFileFromPath (filepath);
			}else {
				throw new DirectoryNotFoundException ("No es posible obtener los archivos, al parecer, el directorio ya no existe");
			}
		}

		public StorageFile CreateFileAsync(string filename){
			return CreateFile (filename, CreationCollisionOption.FailIfExists);
		}

		public StorageFile CreateFile(string filename, CreationCollisionOption option){
			var filepath = Path.Combine (FullPath, filename);
			if (option == CreationCollisionOption.OpenIfExists) {
				if (File.Exists (filepath)) {
					return StorageFile.GetFileFromPath (filepath);
				}
			} else {
				if (option == CreationCollisionOption.FailIfExists) {
					if (File.Exists (filepath)) {
						throw new Exception ("El archivo ya existe");
					}
				} else {
					if (option == CreationCollisionOption.ReplaceExisting) {
						File.Delete(filepath);
					} else {
						filepath = Path.Combine (FullPath, UniqueString() + filename);
					}
				}
			}

			if (System.IO.Directory.Exists (FullPath)) {
				using (var fileStream = System.IO.File.Create (filepath)) {
					return StorageFile.GetFileFromPath (filepath);
				}
			}else {
				throw new DirectoryNotFoundException ("El directorio actual no existe, posiblemente haya sido borrado");
			}
		}

		public StorageFolder CreateFolder(string foldername){
			return CreateFolder (foldername, CreationCollisionOption.FailIfExists);
		}

		public StorageFolder CreateFolder(string foldername, CreationCollisionOption option){
			var dirpath = Path.Combine (FullPath, foldername);
			if (option == CreationCollisionOption.FailIfExists) {
				if (Directory.Exists (dirpath)) {
					throw new Exception ("El directorio que intentas crear ya existe");
				}
			} else {
				if (option == CreationCollisionOption.OpenIfExists) {
					StorageFolder.GetFolderFromPath (dirpath);
				} else {
					if (option == CreationCollisionOption.ReplaceExisting) {
						Directory.Delete (dirpath);	
					} else {
						dirpath = Path.Combine (FullPath, UniqueString() + foldername);
					}
				}
			}

			if (Directory.Exists (FullPath)) {
				var directoryInfo = Directory.CreateDirectory (dirpath);
				if (directoryInfo.Exists) {
					return StorageFolder.GetFolderFromPath (dirpath);
				} else {
					throw new DirectoryNotFoundException ("Imposible crear el directorio");
				}
			}else {
				throw new DirectoryNotFoundException ("El directorio actual no existe, posiblemente haya sido borrado");
			}
		}

		public void Delete(){
			if (Directory.Exists (FullPath)) {
				Directory.Delete (FullPath);
			}else {
				throw new DirectoryNotFoundException ("El directorio actual no existe, posiblemente haya sido borrado");
			}
		}

		public StorageFolder GetFolder(string name){
			if (Directory.Exists (FullPath)) {
				var dir = Path.Combine (FullPath, name);
				if (Directory.Exists (dir)) {
					return StorageFolder.GetFolderFromPath (dir);
				} else {
					throw new DirectoryNotFoundException ("El directorio a buscar no existe");
				}
			}else {
				throw new DirectoryNotFoundException ("El directorio actual no existe, posiblemente haya sido borrado");
			}
		}

		public List<StorageFolder> GetFolders(){
			List<StorageFolder> folders = new List<StorageFolder> ();
			if (Directory.Exists (FullPath)) {
				var directorios = Directory.EnumerateDirectories (FullPath);
				foreach (var directorio in directorios) {
					folders.Add(StorageFolder.GetFolderFromPath (directorio));
				}
			}else {
				throw new DirectoryNotFoundException ("El directorio actual no existe, posiblemente haya sido borrado");
			}
			return folders;
		}

		private string UniqueString(){
			Guid g = Guid.NewGuid ();
			string GuidString = Convert.ToBase64String (g.ToByteArray ());
			GuidString = GuidString.Replace ("=", "");
			GuidString = GuidString.Replace ("+", "");
			return GuidString;
		}

		public static StorageFolder GetFolderFromPath(string dirpath){
			StorageFolder storagefolder = new StorageFolder();
			string[] element = dirpath.Split ('/');
			string foldername = element [element.Length - 1];
			if (System.IO.Directory.Exists (dirpath)) {
				storagefolder.FullPath = dirpath;
				storagefolder.Name = foldername;
			} else {
				throw new DirectoryNotFoundException ("No existe el directorio " + foldername);
			}
			return storagefolder;
		}
	}
}

