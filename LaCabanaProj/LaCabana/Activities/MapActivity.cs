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
using Android.Support.V4.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Util;
using Android.Support.V4.Widget;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class BasicMapDemoActivity : BaseActionActivity, IOnMapReadyCallback,ILocationListener
	{
		private GoogleMap googleMap;
		private string tag = "MainActivity";
		private double _latitude;
		private double _longitude;
		private string _provider;
		LocationManager locManager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			SetTitleActionBar ("Map");
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			ClickHandler ();
			SetProfilePicture ();
			var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById (Resource.Id.map);
			mapFragment.GetMapAsync (this);
			//SearchButton.Visibility = ViewStates.Visible;
			//MenuButton.Visibility = ViewStates.Gone;


		}

		public void OnMapReady (GoogleMap googleMap)
		{
			var myHome = new LatLng (_latitude, _longitude);
			var currentPosition = googleMap;

			googleMap.MyLocationEnabled = true;
			googleMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (myHome, 16));
			//	googleMap.AddMarker (new MarkerOptions ().SetPosition (myHome).SetTitle ("My Home").SetSnippet ("My sweet home"));
			locManager = GetSystemService (Context.LocationService) as LocationManager;
			locManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);

		}

		public void OnLocationChanged (Android.Locations.Location location)
		{
			Log.Debug (tag, "Location changed");
			_latitude = location.Latitude;
			_longitude = location.Longitude;
			_provider = location.Provider.ToString ();
		}

		public void OnProviderDisabled (string provider)
		{
			Log.Debug (tag, provider + " disabled by user");
		}

		public void OnProviderEnabled (string provider)
		{
			Log.Debug (tag, provider + " enabled by user");
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			Log.Debug (tag, provider + " availability has changed to " + status.ToString ());
		}

	}
}

