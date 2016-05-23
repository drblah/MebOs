using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MebOsTheme
{
	public class LinkExtractor
	{

		public LinkExtractor ()
		{
		}

		public string linkInString(string text) 
		{
			List<string> links = new List<string> ();
			Regex url = new Regex(@"(http[s]*:\/\/[www]*[^\s""]+)", RegexOptions.IgnoreCase);
			Regex jpg = new Regex (@"(.+\.jpg)");

			string imageLink;

			MatchCollection matches = url.Matches (text);
			foreach (Match link in matches) {
				links.Add (link.Value);
			}
			if (links.Count > 0) {

				if (jpg.Match (links [0]).Success) {
					imageLink = links [0];
				} else {

					//  Unshorten url
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(links[0]);
					req.AllowAutoRedirect = false;
					var resp = req.GetResponse();
					string realUrl = resp.Headers["Location"];

					if (jpg.Match (realUrl).Success) {
						imageLink = realUrl;
					} else {
						imageLink = "Found no jpgs";
					}
				}
			} else {
				imageLink = "No links";
			}

			return imageLink;	
		}

		public string extractTwitterLink (string twitterPosts)
		{
			string post;
			string link;

			JArray data = JArray.Parse(twitterPosts);


			post = data [0]["text"].ToString();

			link = linkInString (post);

			return link;
		}

	}
}

