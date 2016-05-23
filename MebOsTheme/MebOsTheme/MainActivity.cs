using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Social;

namespace MebOsTheme
{
	[Activity (Label = "MebOsTheme", MainLauncher = true, Icon = "@mipmap/icon")]

	public class MainActivity : Activity
	{
		ImageView imageview;
		Button downloadButton;
		Button postButton;
		Button getButton;
		TextView theView;

		//"/data/data/com.companyname.mebostheme/files/25percent.jpg";
		string imageFile = "/data/data/com.companyname.mebostheme/files/25percent.jpg";
		string imageURL = "";

		private void copyAsset()  
		{

			using (var source = Assets.Open(@"25percent.jpg"))
			using (var dest = OpenFileOutput("25percent.jpg", FileCreationMode.WorldReadable | FileCreationMode.WorldWriteable))
			{
				source.CopyTo(dest);
			}
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			TwitterWrapper tw = new TwitterWrapper(this, "myConsumerKey", 
				"myConsumerSecret", 
				new Uri ("http://127.0.0.1"));

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			this.imageview = FindViewById<ImageView> (Resource.Id.imageView1);
			this.downloadButton = FindViewById<Button> (Resource.Id.button1);
			this.postButton = FindViewById<Button> (Resource.Id.button2);
			this.getButton = FindViewById<Button> (Resource.Id.button3);
			this.theView = FindViewById<TextView> (Resource.Id.textView1);

			downloadButton.Click += async delegate 
			{ 
				
				Downloader myDownloader = new Downloader();
				//Task<string> downloadTask = myDownloader.DownloadImage("https://abs.twimg.com/sticky/default_profile_images/default_profile_0_400x400.png");
				Task<string> downloadTask = myDownloader.DownloadImage(imageURL);
				string filePath = await downloadTask;

				imageFile = filePath;

				Bitmap bitmap = BitmapFactory.DecodeFile(filePath);
				imageview.SetImageBitmap(bitmap);

			};

			postButton.Click += async delegate {

				copyAsset();

				Uploader myUploader = new Uploader();
				LinkExtractor le = new LinkExtractor();

				Task<string> uploadTask = myUploader.UploadImage(imageFile);
				string outputString = await uploadTask; 

				string imageLink = le.linkInString(outputString);

				// 2. Create an item to share
				var item = new Item { Text = "Yay test." };
				item.Links.Add (new Uri (imageLink));

				// 3. Present the UI on Android
				var shareIntent = tw.GetShareUI (this, item, result => {
					// result lets you know if the user shared the item or canceled
				});
				StartActivityForResult (shareIntent, 42);

			};

			getButton.Click += async delegate {
				//Task<string> posts = tw.getPosts("Eustreptospondylus", 1); // Non-existing user
				Task<string> posts = tw.getPosts("mrjeppa", 1);
				string rawJSON = await posts;
				LinkExtractor ln = new LinkExtractor();
				string imgLink = ln.extractTwitterLink(rawJSON);
				theView.Text = imgLink;

				imageURL = imgLink;
			};

		}
	}
}