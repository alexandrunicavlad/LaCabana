
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
using System.Threading;
using Android.Support.V4.Widget;
using LaCabana.Services;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class LoginActivity : BaseDrawerActivity
	{
		private const int NormalLogin = 0;
		private const int FacebookLogin = 1;
		private const int GoogleLogin = 2;
		private int loginType;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.log_in_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Sign In");
			ClickHandler ();

			DatabaseServices = new DatabaseServices (this);
//			var allUsers = DatabaseServices.GetAllUsers ();

			var signUp = FindViewById<TextView> (Resource.Id.signUpButtonDetails);
			signUp.Click += delegate {
				StartActivityForResult (typeof(SignUpActivity), 0);    
			};

			var loginBtn = FindViewById<TextView> (Resource.Id.signInButton);
			loginBtn.Click += delegate {
				HideKeyboard (loginBtn);
				ThreadPool.QueueUserWorkItem (o => LoginVerify ());
			};

			var forgotPasswordBtn = FindViewById<TextView> (Resource.Id.forgotPassword);

			forgotPasswordBtn.Click += delegate {
				//StartActivity (typeof(RecoverPasswordActivity));  
			};

		}

		private void LoginVerify ()
		{
			var baseService = new BaseService<Dictionary<string,UsersModel>> ();
			var user = new Dictionary<string,UsersModel> ();
			try {
				user = (baseService.Get ("users"));
			} catch (Exception e) {
				var a = 0;
			}
			var email = FindViewById<EditText> (Resource.Id.login_email).Text;
			var password = FindViewById<EditText> (Resource.Id.login_password).Text;


			//RunOnUiThread (() => CreateDialog (Resources.GetString (Resource.String.wait), false, true));

			foreach (var item in user) {
				if (email == "" || email != item.Value.Email) {
					//CreateDialog (Resources.GetString (Resource.String.invalid_email));

				} else if (password == "" || password != item.Value.Password) {
					//CreateDialog (Resources.GetString (Resource.String.invalid_password));

				} else {
					ThreadPool.QueueUserWorkItem (o => {
						UsersModel model = new UsersModel ();
						model.Email = item.Value.Email;
						model.Id = item.Value.Id;
						model.Password = item.Value.Password;
						model.Username = item.Value.Username;
						var abc = DatabaseServices.GetAllUsers ();
						DatabaseServices.InsertUsername (model);
						StartActivity (typeof(BasicMapDemoActivity));

						Finish ();
					});
				}
			}

		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {

				//Finish ();
		
			}
		}
	}
}
