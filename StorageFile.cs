using System;
using System.IO;

namespace DevelopersAzteca.Storage
{
	public class StorageFile
	{

		public string FullPath { 
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public string DisplayName {
			get;
			set;
		}

		public string DisplayType {
			get;
			set;
		}

		public DateTime DateCreated {
			get;
			set;
		}

		public FileAttributes Attributes {
			get;
			set;
		}

		public StorageFile ()
		{
			
		}

		public StorageFile CopyTo(StorageFolder folderdestination){
			return CopyTo (folderdestination, Name);
		}

		public StorageFile CopyTo(StorageFolder folderdestination, string filenamewithextension){
			return CopyTo (folderdestination, filenamewithextension, NameCollisionOption.FailIfExists);
		}

		public StorageFile CopyTo(StorageFolder folderdestination, string filenamewithextension, NameCollisionOption option){
			if (Exists()) {
				string outfilepath = Path.Combine (folderdestination.FullPath, filenamewithextension);
				if (option == NameCollisionOption.FailIfExists) {
					if (File.Exists (outfilepath)) {
						throw new Exception ("El archivo que tratas de copiar ya existe");
					}
				} else {
					if (option == NameCollisionOption.GenerateUniqueName) {
						outfilepath = Path.Combine (folderdestination.FullPath, UniqueString() + filenamewithextension);
					} else {
						File.Delete (outfilepath);
					}
				}
				File.Copy (FullPath, outfilepath);
				if (File.Exists (outfilepath)) {
					return StorageFile.GetFileFromPath (outfilepath);
				} else {
					throw new FileNotFoundException ("No fue posible copiar el archivo");
				}
			}
			return null;
		}

		public void Delete(){
			if (Exists()) {
				File.Delete (FullPath);
			}
		}

		public static StorageFile GetFileFromPath(string path){
			string[] element = path.Split ('/');
			string filenameext = element [element.Length - 1];
			string[] elementfile = filenameext.Split ('.');
			string filename = elementfile [0];
			string ext = elementfile [1];
			var filedatetime = File.GetCreationTime (path);
			var fileAttr = File.GetAttributes (path);
			return new StorageFile {
				Name = filenameext,
				DisplayName = filename,
				DisplayType = ext,
				DateCreated = filedatetime,
				Attributes = fileAttr,
				FullPath = path
			};
		}

		public Stream Open(FileAccessMode mode){
			if (Exists ()) {
				if (mode == FileAccessMode.Read) {
					return File.OpenRead (FullPath);    
				} else {
					return File.OpenWrite (FullPath);
				}	
			}else {
				throw new FileNotFoundException ("El archivo que tratas de copiar no existe, path: " + FullPath + ", name: " + Name);
			}
		}

		private bool Exists(){
			if (File.Exists (FullPath)) {
				return true;
			} else {
				throw new FileNotFoundException ("El archivo que tratas de copiar no existe, path: " + FullPath + ", name: " + Name);
			}
		}

		private string UniqueString(){
			Guid g = Guid.NewGuid ();
			string GuidString = Convert.ToBase64String (g.ToByteArray ());
			GuidString = GuidString.Replace ("=", "");
			GuidString = GuidString.Replace ("+", "");
			return GuidString;
		}
	}

}

