using System;
using System.Security.Policy;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Android.Graphics;
using Xamarin;
using LaCabana.Helpers;
using System.Reflection;

namespace LaCabana
{
	public class CloudUpload
	{
		public const string ADDR_API = "api.cloudinary.com";
		public const string ADDR_RES = "res.cloudinary.com";
		public const string API_VERSION = "v1_1";
		public const string HTTP_BOUNDARY = "notrandomsequencetouseasboundary";
		public static string USER_AGENT;
		public int Timeout = 0;
		public string api_key = "348639768631669";
		public string api_secret = "HHeKWX7znazzS61cd7tlTxBmV7I";
		public bool UseChunkedEncoding = true;
		private const string url = "https://api.cloudinary.com/v1_1/lacabana/image/upload";
		public int ChunkSize = 65000;

		public void PostImage (Bitmap image)
		{	
		

			Stream stream = new MemoryStream ();
			image.Compress (Bitmap.CompressFormat.Png, 100, stream);

			using (HttpWebResponse response = Call (HttpMethod.Post, url, null, stream)) {
				var abc = response.StatusCode;
			}
		}


		public HttpWebResponse Call (HttpMethod method, string url, SortedDictionary<string, object> parameters, Stream file)
		{
			#if DEBUG
			Console.WriteLine (String.Format ("{0} REQUEST:", method));
			Console.WriteLine (url);
			#endif
			var version = Assembly.GetExecutingAssembly ().GetName ().Version;
			USER_AGENT = String.Format ("cld-dotnet-{0}.{1}.{2}",
				version.Major, version.Minor, version.Build);
			HttpWebRequest request = HttpWebRequest.Create (url) as HttpWebRequest;
			request.Method = "POST";
			request.UserAgent = USER_AGENT;
			if (Timeout > 0) {
				request.Timeout = Timeout;
			}

			if (method == HttpMethod.Post) {
				if (UseChunkedEncoding)
					request.SendChunked = true;

				request.ContentType = "multipart/form-data; boundary=" + HTTP_BOUNDARY;

//				if (!parameters.ContainsKey ("unsigned") || parameters ["unsigned"].ToString () == "false")
//					FinalizeUploadParameters (parameters);

				using (Stream requestStream = request.GetRequestStream ()) {
					using (StreamWriter writer = new StreamWriter (requestStream)) {
						foreach (var param in parameters) {
							if (param.Value != null) {
								if (param.Value is IEnumerable<string>) {
									foreach (var item in (IEnumerable<string>)param.Value) {
										WriteParam (writer, param.Key + "[]", item);
									}
								} else {
									WriteParam (writer, param.Key, param.Value.ToString ());
								}
							}
						}

						if (file != null) {
							WriteFile (writer, file, (int)file.Length, "buburuza");
						}

						writer.Write ("--{0}--", HTTP_BOUNDARY);
					}
				}
			} else {
				byte[] authBytes = Encoding.ASCII.GetBytes (String.Format ("{0}:{1}", api_key, api_secret));
				request.Headers.Add ("Authorization", String.Format ("Basic {0}", Convert.ToBase64String (authBytes)));
			}

			try {
				return (HttpWebResponse)request.GetResponse ();
			} catch (WebException ex) {
				var response = ex.Response as HttpWebResponse;
				if (response == null)
					throw;
				else
					return response;
			}
		}

		public class FileParameter
		{
			public byte[] File { get; set; }

			public string FileName { get; set; }

			public string ContentType { get; set; }

			public FileParameter (byte[] file) : this (file, null)
			{
			}

			public FileParameter (byte[] file, string filename) : this (file, filename, null)
			{
			}

			public FileParameter (byte[] file, string filename, string contenttype)
			{
				File = file;
				FileName = filename;
				ContentType = contenttype;
			}
		}

		internal void FinalizeUploadParameters (IDictionary<string, object> parameters)
		{
			parameters.Add ("timestamp", GetTime ());
			parameters.Add ("signature", SignParameters (parameters));
			parameters.Add ("api_key", api_key);
		}

		private string GetTime ()
		{
			return Convert.ToInt64 (((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalSeconds)).ToString ();
		}

		public string SignParameters (IDictionary<string, object> parameters)
		{
			StringBuilder signBase = new StringBuilder (String.Join ("&", parameters.Where (pair => pair.Value != null).Select (pair => String.Format ("{0}={1}", pair.Key,
				                         pair.Value is IEnumerable<string>
					? String.Join (",", ((IEnumerable<string>)pair.Value).ToArray ())
					: pair.Value.ToString ()))
				.ToArray ()));

			//signBase.Append (Account.ApiSecret);

			var hash = ComputeHash (signBase.ToString ());
			StringBuilder sign = new StringBuilder ();
			foreach (byte b in hash)
				sign.Append (b.ToString ("x2"));

			return sign.ToString ();
		}

		private byte[] ComputeHash (string s)
		{
			using (var sha1 = SHA1.Create ()) {
				return sha1.ComputeHash (Encoding.UTF8.GetBytes (s));
			}
		}

		private bool WriteFile (StreamWriter writer, Stream stream, int length, string fileName)
		{
			WriteLine (writer, "--{0}", HTTP_BOUNDARY);
			WriteLine (writer, "Content-Disposition: form-data;  name=\"file\"; filename=\"{0}\"", fileName);
			WriteLine (writer, "Content-Type: application/octet-stream");
			WriteLine (writer);

			writer.Flush ();

			int bytesSent = 0;
			int toSend = 0;
			byte[] buf = new byte[ChunkSize];
			int cnt = 0;

			while ((toSend = length - bytesSent) > 0
			       && (cnt = stream.Read (buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0) {
				writer.BaseStream.Write (buf, 0, cnt);
				bytesSent += cnt;
			}

			return cnt == 0;
		}

		private void WriteParam (StreamWriter writer, string key, string value)
		{
			#if DEBUG
			Console.WriteLine (String.Format ("{0}: {1}", key, value));
			#endif
			WriteLine (writer, "--{0}", HTTP_BOUNDARY);
			WriteLine (writer, "Content-Disposition: form-data; name=\"{0}\"", key);
			WriteLine (writer);
			WriteLine (writer, value);
		}

		private void WriteLine (StreamWriter writer)
		{
			writer.Write ("\r\n");
		}

		private void WriteLine (StreamWriter writer, string format)
		{
			writer.Write (format);
			writer.Write ("\r\n");
		}

		private void WriteLine (StreamWriter writer, string format, Object val)
		{
			writer.Write (format, val);
			writer.Write ("\r\n");
		}

	}


}

