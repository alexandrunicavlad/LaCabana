
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
			user.Password = password.Text;
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

