using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Media;
using Xamarin.Social.Services;
using Xamarin.Auth;
using Android.Content;


namespace MebOsTheme
{
	public class TwitterWrapper : TwitterService
	{
		//private static TwitterService mTwitter;
		private Android.App.Activity cont;
		public TwitterWrapper (Android.App.Activity context, string consumerKey, string consumerSecret, Uri callBack)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
			this.CallbackUrl = callBack;

			cont = context;
		}

		public async Task<string> getPosts(string userName, int nrOfPosts)
		{
			Task<IEnumerable<Account>> accTask = this.GetAccountsAsync (cont);
			IEnumerable<Account> accounts = await accTask;
			Account twitterAccount = accounts.ToList () [0];

			var req = this.CreateRequest ("GET", 
				new Uri ("https://api.twitter.com/1.1/statuses/user_timeline.json"), new Dictionary<string, string> {
					{ "screen_name", userName },
					{ "count", nrOfPosts.ToString() }
				}, twitterAccount);

			Task<Response> responseTask = req.GetResponseAsync ();
			try {
				Response response = await responseTask;

				return response.GetResponseText();
			}
			catch  {
				return null;
			}
		}
	}
}

