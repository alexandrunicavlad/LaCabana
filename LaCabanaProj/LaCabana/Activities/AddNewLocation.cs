
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
using LaCabana.Helpers;

namespace LaCabana
{
	[Activity (Label = "AddNewLocation")]			
	public class AddNewLocation : BaseActionActivity
	{
		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.add_new_location);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Add new location");
			ClickHandler ();
			var phoneSpinner = FindViewById<Spinner> (Resource.Id.phoneSpinner);
			var emailSpinner = FindViewById<Spinner> (Resource.Id.emailSpinner);

			var stringPhone = new List<String> (){ "Select", "Mobine", "Home", "Work" };
			var adapter = new FontArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, stringPhone);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			phoneSpinner.Adapter = adapter;
			phoneSpinner.SetSelection (0);
		}
	}
}

