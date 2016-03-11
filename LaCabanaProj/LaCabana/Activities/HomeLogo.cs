
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
using Android.Animation;
using Android.Views.Animations;
using Android.Graphics;
using Java.Lang;
using System.Threading;

namespace LaCabana
{
	[Activity (MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class HomeLogo : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.home_logo);

			var progressBar = FindViewById<ProgressBar> (Resource.Id.splash_progressBar);

			var apiVersion = Android.OS.Build.VERSION.SdkInt;
			if (apiVersion <= Android.OS.BuildVersionCodes.Kitkat) {
				progressBar.IndeterminateDrawable.SetColorFilter (Resources.GetColor (Resource.Color.green), PorterDuff.Mode.SrcAtop);
			}
			ThreadPool.QueueUserWorkItem (o => StartMainActivity ());

		}

		private void StartMainActivity ()
		{           
			Java.Lang.Thread.Sleep (3000);
			StartActivity (typeof(LoginActivity));
			Finish ();
		}
	}
}

