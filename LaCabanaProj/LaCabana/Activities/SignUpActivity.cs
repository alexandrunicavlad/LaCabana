
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

		}
	}

}

