
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using Newtonsoft.Json;
using FireSharp.Response;
using System.Threading;
using Java.Security;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]			
	public class SignUpActivity : BaseDrawerActivity
	{
		PushResponse response;
		private EditText userName;
		private EditText email;
		private EditText password;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.sign_up_layout);

			var signIn = FindViewById<TextView> (Resource.Id.signUpButtonDetails);
			SetTitleActionBar ("Sign up");
			signIn.Click += delegate {
				StartActivityForResult (typeof(LoginActivity), 0);    
			};
			var register = FindViewById<TextView> (Resource.Id.signInButton);
			userName = FindViewById<EditText> (Resource.Id.login_username);
			email = FindViewById<EditText> (Resource.Id.login_email);
			password = FindViewById<EditText> (Resource.Id.login_password);

			register.Click += delegate {	
				ThreadPool.QueueUserWorkItem (o => RegisterLogin ());
			};
		}

		private void RegisterLogin ()
		{
			if (userName.Text == "") {
				CreateDialog ("Please enter username", true);
				return;
			} else if (email.Text == "") {
				CreateDialog ("Please enter email", true);
				return;
			} else if (password.Text == "") {
				CreateDialog ("Please enter password", true);
				return;
			}
			UsersModel user = new UsersModel ();
			CreateDialog (Resources.GetString (Resource.String.wait), false, true);
			//user.Id = userName.Text;
			user.Username = userName.Text;
			user.Email = email.Text;
			MessageDigest md = MessageDigest.GetInstance ("SHA-1");
			md.Update (Org.Apache.Http.Util.EncodingUtils.GetBytes (password.Text, "iso-8859-1"), 0, password.Text.Length);
			byte[] sha1hash = md.Digest ();
			var hash = convertToHex (sha1hash);
			user.Password = hash;
			var requestText = JsonConvert.SerializeObject (user);
			var result = Push (user);
			user.Id = result;
			DatabaseServices.InsertUsername (user);
			var baseserv = new BaseService<UsersModel> ();
			var newUrl = string.Format ("users/{0}", user.Id);
			try {
				baseserv.UpdateUser (user, newUrl);

			} catch (Exception e) {
				var a = 0;
			}
			StartActivity (typeof(BasicMapDemoActivity));
			Finish ();
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

		private string Push (UsersModel user)
		{
			var baseService = new BaseService<UsersModel> ();
			try {
				response = baseService.Push (user, "users");
			} catch (Exception e) {
				var a = 0;
			}
			return response.Result.Name;
		}

		private void Update (UsersModel user)
		{
			var baseService = new BaseService<UsersModel> ();

		}

	}

}

