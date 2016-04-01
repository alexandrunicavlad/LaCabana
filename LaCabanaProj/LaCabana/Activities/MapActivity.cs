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
using Android.Support.V7.Widget;
using LaCabana.Services;
using Android.Support.V4.Widget;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class BasicMapDemoActivity : BaseActionActivity, IOnMapReadyCallback,ILocationListener
	{
		private GoogleMap _googleMap;
		private string tag = "MainActivity";
		private double _latitude;
		private double _longitude;
		private string _provider;
		LocationManager locManager;
		Marker homeMarker;
		IDatabaseServices DatabaseServices;
		private List<CabinModel> allCabins;
		public double _clickLatitude;
		public double _clickLongitude;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			SetTitleActionBar ("Map");
			ClickHandler ();
			SetProfilePicture ();
			var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById (Resource.Id.map);
			mapFragment.GetMapAsync (this);
			DatabaseServices = new DatabaseServices (this);

			//SearchButton.Visibility = ViewStates.Visible;
			//MenuButton.Visibility = ViewStates.Gone;
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			allCabins = DatabaseServices.GetAllCabins ();

		}

		public void OnMapReady (GoogleMap googleMap)
		{
			var myHome = new LatLng (_latitude, _longitude);
			googleMap.MyLocationEnabled = true;
			_googleMap = googleMap;
			PutAllMarker ();
			var isCurent = false;
			googleMap.MyLocationChange += (object sender, GoogleMap.MyLocationChangeEventArgs e) => {
				if (isCurent)
					return;
				isCurent = true;
				homeMarker = googleMap.AddMarker (new MarkerOptions ().SetPosition (new LatLng (e.Location.Latitude, e.Location.Longitude)));
				homeMarker.Title = "myLocation";
				googleMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (new LatLng (e.Location.Latitude, e.Location.Longitude), 15));
			};

			googleMap.MapLongClick += (object sender, GoogleMap.MapLongClickEventArgs e) => {
				var intent = new Intent (this, typeof(AddNewLocation));
				intent.PutExtra ("latitude", e.Point.Latitude);
				intent.PutExtra ("longitude", e.Point.Longitude);
				StartActivity (intent);
			};
//			googleMap.MarkerClick += (object sender, GoogleMap.MarkerClickEventArgs e) => {
//				WindowAdapter (e.Marker, allCabins);	//				
//			};


			locManager = GetSystemService (Context.LocationService) as LocationManager;
			locManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);

		}

		public void PutAllMarker ()
		{
			foreach (var cab in allCabins) {
				var marker = (new MarkerOptions ().SetPosition (new LatLng (cab.Latitude, cab.Longitude)));
				marker.SetTitle (cab.Name);
				_googleMap.AddMarker (marker);
				var adapter = new InfoWindowAdapter (marker, this, allCabins);
				_googleMap.SetInfoWindowAdapter (adapter);
			}	

		}

		public void MapLongClick ()
		{
			_googleMap.MapLongClick += (object sender, GoogleMap.MapLongClickEventArgs e) => {
				
			};
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

