using System;
using System.IO;

namespace DevelopersAzteca.Storage
{
	public class KnownFolders
	{

		public static StorageFolder Documents {
			get{
				return GetFolder (Folder.Documents);
			}
		}

		public static StorageFolder Downloads {
			get{
				return GetFolder (Folder.Downloads);
			}
		}

		public static StorageFolder Music {
			get{
				return GetFolder (Folder.Music);
			}
		}

		public static StorageFolder Pictures {
			get{
				return GetFolder (Folder.Pictures);
			}
		}

		public static StorageFolder CameraRoll{
			get{
				return GetFolder (Folder.CameraRoll);
			}
		}

		public static StorageFolder Movies {
			get{
				return GetFolder (Folder.Movie);
			}
		}

		public static StorageFolder Home{
			get{
				string path = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
				StorageFolder storagefolder = new StorageFolder();
				string[] element = path.Split ('/');
				string foldername = element [element.Length - 1];
				if (System.IO.Directory.Exists (path)) {
					storagefolder.FullPath = path;
					storagefolder.Name = foldername;
				} else {
					throw new DirectoryNotFoundException ("No existe el directorio " + foldername);
				}
				return storagefolder;
			}
		}

		public static StorageFolder SDCard{
			get{
				string path = Path.Combine ("/sdcard/");
				StorageFolder storagefolder = new StorageFolder();
				string[] element = path.Split ('/');
				string foldername = element [element.Length - 1];
				if (System.IO.Directory.Exists (path)) {
					storagefolder.FullPath = path;
					storagefolder.Name = foldername;
				} else {
					throw new DirectoryNotFoundException ("No existe el directorio " + foldername);
				}
				return storagefolder;
			}
		}

		public static StorageFolder Root{
			get{
				string path = Path.Combine (Android.OS.Environment.RootDirectory.AbsolutePath);
				StorageFolder storagefolder = new StorageFolder();
				string[] element = path.Split ('/');
				string foldername = element [element.Length - 1];
				if (System.IO.Directory.Exists (path)) {
					storagefolder.FullPath = path;
					storagefolder.Name = foldername;
				} else {
					throw new DirectoryNotFoundException ("No existe el directorio " + foldername);
				}
				return storagefolder;
			}
		}

		private static StorageFolder GetFolder(Folder folder = Folder.Downloads){
			switch (folder) {
			case Folder.Downloads:
				return KnownFolders.Home.GetFolder ("Download");

			case Folder.Movie:
				return KnownFolders.Home.GetFolder ("Movies");

			case Folder.Music:
				return KnownFolders.Home.GetFolder ("Music");

			case Folder.Pictures:
				return KnownFolders.Home.GetFolder ("Pictures");

			case Folder.CameraRoll:
				return KnownFolders.Home.GetFolder ("DCIM");

			default:
				return KnownFolders.Home;
			}
		}
	}

	public enum Folder{
		Downloads, Music, Pictures, Documents, Movie, CameraRoll
	}
}

