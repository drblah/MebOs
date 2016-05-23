using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;

//Downloader related:
using System.Net;
using System.IO;

namespace MebOsTheme
{
	public class Downloader
	{
		public async Task<string> DownloadImage (string imageUrl)
		{
			var webClient = new WebClient();
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			string localFilename = "downloaded.png";
			string localPath = Path.Combine (documentsPath, localFilename); 

			webClient.DownloadDataCompleted += (s, e) => 
			{
				var bytes = e.Result; 
				File.WriteAllBytes (localPath, bytes);
			};

			var url = new Uri (imageUrl);

			var data = await webClient.DownloadDataTaskAsync (url);
			return localPath;
		} 

		public Downloader ()
		{

		}
	}
}