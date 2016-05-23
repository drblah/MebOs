using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using Android.Graphics;

//Downloader related:
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MebOsTheme
{
	public class Uploader
	{
		private const string uploadURL = "http://192.168.1.150:4567/save_image";
		public async Task<string> UploadImage (string filePath)
		{
			// http://stackoverflow.com/a/10192092
			
		    /*
			 * METODE MED COMPRESSION
			byte[] bitmapData;
			var stream = new MemoryStream();
			bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
			bitmapData = stream.ToArray();
			var fileContent = new ByteArrayContent(bitmapData);
			*/

			var fstream = File.OpenRead (filePath);

			var stream = new MemoryStream ();
			fstream.CopyTo (stream);
			var fileContent = new ByteArrayContent (stream.ToArray());

			fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse ("application/octet-stream");
			fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue ("form-action")
			{ 
				Name = "file", 
				FileName = "uploaded_image.jpg"
			};

			string boundary = "---8d0f01e6b3b5dafaaadaad";
			MultipartFormDataContent multipartContent = new MultipartFormDataContent (boundary);
			multipartContent.Add (fileContent);

			HttpClient httpClient = new HttpClient ();
			HttpResponseMessage response = await httpClient.PostAsync (uploadURL, multipartContent);
			if (response.IsSuccessStatusCode) 
			{
				string content = await response.Content.ReadAsStringAsync ();
				return content;
			}
			return null; 
		}

		public Uploader ()
		{
		}
	}
}

