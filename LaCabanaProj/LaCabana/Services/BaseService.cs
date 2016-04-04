using System;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

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

		public void Push (T baseModel, string url)
		{			
			var URL = string.Format ("{0} {1}", BasePath1, url);
			TestFixtureSetUpCabana (URL);
			var response = _client.Push ("", baseModel);
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


	}
}

