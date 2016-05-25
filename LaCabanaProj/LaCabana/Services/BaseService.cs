using System;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using Java.Text;
using Java.Util;
using System.Security.Cryptography;
using System.Text;
using Java.Security;
using System.Net.Http;
using FireSharp.Response;

namespace LaCabana
{
	public class BaseService<T>
	{
		private IFirebaseClient _client;
		protected const string BasePath1 = "https://lacabana1.firebaseio.com/";
		protected const string BasePathWithoutSlash1 = "https://mapcabana.firebaseio.com";



		public async void TestFixtureSetUpCabana (string URL)
		{
			IFirebaseConfig config1 = new FirebaseConfig {
				//AuthSecret = FirebaseSecret1,
				BasePath = URL
			};
			_client = new FirebaseClient (config1);

		}

		public PushResponse Push (T baseModel, string url)
		{			
			var URL = string.Format ("{0} {1}", BasePath1, url);
			TestFixtureSetUpCabana (URL);
			var response = _client.Push ("", baseModel);
			return response;
		}

		public T Get (string url)
		{
			var URL = string.Format ("{0} {1}", BasePath1, url);
			TestFixtureSetUpCabana (URL);
			var response = _client.Get ("");
			var abc = response.Body;
			var deserializedStreamText = JsonConvert.DeserializeObject<T> (abc);
			return deserializedStreamText;
		}

		public void Update (string fav, string url)
		{
			var URL = string.Format ("{0} {1}", BasePath1, url);
			TestFixtureSetUpCabana (URL);
			var response = _client.Push ("", fav);
		}

		public void UpdateUser (T basemodel, string url)
		{
			var URL = string.Format ("{0} {1}", BasePath1, url);
			TestFixtureSetUpCabana (URL);
			var response = _client.Update ("", basemodel);
		}


		public void Face ()
		{
			var cloudinary = "lacabana";
			var api_key = "348639768631669";
			var api_secret = "HHeKWX7znazzS61cd7tlTxBmV7I";
			SimpleDateFormat simpledate = new SimpleDateFormat ("dd-MM-yyyy-hh-mm-ss");
			//var timestamp = simpledate.Format (new Date ());
			var timestamp = "1463577477";
			var file = "http://www.safir-travel-egypt.com/images/images/ASDAD.jpg";
			var fasf = "timestamp=" + timestamp + api_secret;
			MessageDigest md = MessageDigest.GetInstance ("SHA-1");
			md.Update (Org.Apache.Http.Util.EncodingUtils.GetBytes (fasf, "iso-8859-1"), 0, fasf.Length);
			byte[] sha1hash = md.Digest ();
			var signature = convertToHex (sha1hash);
			//var nnewurl = string.Format ("https://api.cloudinary.com/v1_1/lacabana/image/upload?file={0}&api_key={1}&timestamp={2}&signature={3}", file, api_key, timestamp, signature);
			var newurl = string.Format ("https://api.cloudinary.com/v1_1/lacabana/image/upload");
			HttpClient httpClient = new HttpClient ();
			MultipartFormDataContent form = new MultipartFormDataContent ();
			form.Add (new StringContent (api_key), "api_key");
			form.Add (new StringContent (timestamp), "timestamp");
			form.Add (new StringContent (signature), "signature");
			//form.Add (new ByteArrayContent (imagebytearraystring, 0, imagebytearraystring.Count ()), "profile_pic", "hello1.jpg");
			try {
				//HttpResponseMessage response = httpClient.PostAsync (newurl, form).Result;
				HttpResponseMessage response = httpClient.PostAsync (newurl, null).Result;
				response.EnsureSuccessStatusCode ();
				httpClient.Dispose ();
				string sd = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception e) {
				var abc = 0;
			}


		}

		private static String convertToHex (byte[] data)
		{
			StringBuilder buf = new StringBuilder ();
			foreach (byte b in data) {
				int halfbyte = (b >> 4) & 0x0F;
				int two_halfs = 0;
				do {
					buf.Append ((0 <= halfbyte) && (halfbyte <= 9) ? (char)('0' + halfbyte) : (char)('a' + (halfbyte - 10)));
					halfbyte = b & 0x0F;
				} while (two_halfs++ < 1);
			}
			return buf.ToString ();
		}

		//		private void onFacebookAccessTokenChange(AccessToken token) {
		//			ifif (token != null) {
		//				ref.authWithOAuthToken("facebook", token.getToken(), new Firebase.AuthResultHandler() {
		//					@Override
		//					public void onAuthenticated(AuthData authData) {
		//						// The Facebook user is now authenticated with your Firebase app
		//					}
		//					@Override
		//					public void onAuthenticationError(FirebaseError firebaseError) {
		//						// there was an error
		//					}
		//				});
		//			} else {
		//				/* Logged out of Facebook so do a logout from the Firebase app */
		//				ref.unauth();
		//			}
		//		}




	}
}

