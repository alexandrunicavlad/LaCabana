
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
using Android.Support.V4.Widget;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]		
	public class CabinInfo : BaseDrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.cabin_info_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Cabin");
		}
	}
}

