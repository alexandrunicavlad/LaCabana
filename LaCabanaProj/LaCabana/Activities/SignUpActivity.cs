
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

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]			
	public class SignUpActivity : BaseDrawerActivity
	{

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
			var userName = FindViewById<EditText> (Resource.Id.login_username);
			var email = FindViewById<EditText> (Resource.Id.login_email);
			var password = FindViewById<EditText> (Resource.Id.login_password);

			register.Click += delegate {				
				UsersModel user = new UsersModel ();
				user.Id = userName.Text;
				user.Username = userName.Text;
				user.Email = email.Text;
				user.Password = password.Text;
				var requestText = JsonConvert.SerializeObject (user);
				Push (user);
				Finish ();
			};
		}


		private void Push (UsersModel user)
		{
			var baseService = new BaseService<UsersModel> ();
			try {
				baseService.Push (user, "users");
			} catch (Exception e) {
				var a = 0;
			}
		}

		private void Update (UsersModel user)
		{
			var baseService = new BaseService<UsersModel> ();

		}

	}

}

