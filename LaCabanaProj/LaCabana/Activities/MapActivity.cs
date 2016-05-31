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
using FireSharp.Interfaces;
using System.Threading;
using Android.Database;
using Android.Provider;

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
		private Dictionary<string,CabinModel> allCabins;
		public double _clickLatitude;
		public double _clickLongitude;
		public LatLng myHome;
		private ListView searchList;
		private List<String> SUGGESTIONS;
		private Android.Widget.SimpleCursorAdapter adaptere;
		private MatrixCursor c;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			ConstructRightIcon ();
			SetTitleActionBar1 ("Map");
			SetProfilePicture ();
			ClickHandler ();
			var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById (Resource.Id.map);
			mapFragment.GetMapAsync (this);
			DatabaseServices = new DatabaseServices (this);
			//SearchButton.Visibility = ViewStates.Visible;
			//MenuButton.Visibility = ViewStates.Gone;
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			//allCabins = DatabaseServices.GetAllCabins ();
			searchList = FindViewById<ListView> (Resource.Id.searchList);
			allCabins = new Dictionary<string,CabinModel> ();
			SUGGESTIONS = new List<string> ();
			ThreadPool.QueueUserWorkItem (o => GetData ());
		}

		public void ConstructRightIcon ()
		{
			ActionBar actionBar = ActionBar;

			actionBar.SetDisplayShowCustomEnabled (true);
			actionBar.SetDisplayOptions (ActionBarDisplayOptions.ShowCustom, ActionBarDisplayOptions.ShowTitle);
			LayoutInflater inflate = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);
			View view = inflate.Inflate (Resource.Layout.action_bar_home, null);
			actionBar.SetCustomView (Resource.Layout.action_bar_home);

			view.Click += delegate {
				var a = 0;
			};
			String[] fromul = new String[] { "cityName" };
			int[] toul = new int[] { Android.Resource.Id.Text1 };
			adaptere = new Android.Widget.SimpleCursorAdapter (this, Android.Resource.Layout.SimpleListItem1, null, fromul, toul, CursorAdapterFlags.RegisterContentObserver);
			Search.Visibility = ViewStates.Visible;
			Search = ActionBar.CustomView.FindViewById<Android.Widget.SearchView> (Resource.Id.searchView);
			SearchButton = ActionBar.CustomView.FindViewById<ImageButton> (Resource.Id.action_bar_searchBtn);
			SearchButton.Visibility = ViewStates.Gone;
			Search.SearchClick += delegate {	
				Search.SuggestionsAdapter = adaptere;
			};
			Search.QueryTextChange += (object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e) => {
				PopulateAdapter (e.NewText);

			};
			Search.QueryTextSubmit += (object sender, Android.Widget.SearchView.QueryTextSubmitEventArgs e) => {
				var b = 0;
			};
			Search.SuggestionClick += (object sender, Android.Widget.SearchView.SuggestionClickEventArgs e) => {
				var a = e.Position;
				var abc = c.MoveToPosition (a);
				var intent = new Intent (this, typeof(CabinInfo));	
				intent.PutExtra ("marker", c.GetString (1));
				intent.PutExtra ("latitude", myHome.Latitude);
				intent.PutExtra ("longitude", myHome.Longitude);
				StartActivity (intent);										

			};


			//Search.SetOnSuggestionListener (new );
		}

		private void PopulateAdapter (String query)
		{
			c = new MatrixCursor (new String[]{ BaseColumns.Id, "key", "cityName" });

			foreach (var icab in allCabins) {
				//if (icab.Value.Name.ToLower ().StartsWith (query.ToLower ())) {
				if (icab.Value.Name.ToLower ().Contains (query.ToLower ())) {					
					var adac = new Java.Lang.Object[] { 1, icab.Key, icab.Value.Name };						
					c.AddRow (adac);
				}
			}
			adaptere.ChangeCursor (c);
		}


		public void GetData ()
		{
			var baseService = new BaseService<Dictionary<string,CabinModel>> ();
			try {				
				allCabins = (baseService.Get ("cabins"));
			} catch (Exception e) {
				//Toast.MakeText (this, "A dat eroare", ToastLength.Short).Show ();
			}
			RunOnUiThread (() => {
				PutAllMarker ();
//				foreach (var cabin in allCabins) {
//					DatabaseServices.InsertCabin (cabin.Value);
//				}
//				var abc = DatabaseServices.GetAllCabins ();
			});
		}



		public void OnMapReady (GoogleMap googleMap)
		{
			myHome = new LatLng (_latitude, _longitude);
			googleMap.MyLocationEnabled = true;
			_googleMap = googleMap;
			var isCurent = false;
			googleMap.MyLocationChange += (object sender, GoogleMap.MyLocationChangeEventArgs e) => {
				if (isCurent)
					return;
				isCurent = true;
				//homeMarker = googleMap.AddMarker (new MarkerOptions ().SetPosition (new LatLng (e.Location.Latitude, e.Location.Longitude)));
				//homeMarker.Title = "myLocation";
				myHome.Latitude = e.Location.Latitude;
				myHome.Longitude = e.Location.Longitude;
				MyPosition (myHome);
				googleMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (new LatLng (e.Location.Latitude, e.Location.Longitude), 10));

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
			googleMap.InfoWindowClick += (object sender, GoogleMap.InfoWindowClickEventArgs e) => {							
				var intent = new Intent (this, typeof(CabinInfo));	
				foreach (var item in allCabins) {
					if (item.Value.Name.Equals (e.Marker.Title)) {
						intent.PutExtra ("marker", item.Key);
						intent.PutExtra ("latitude", myHome.Latitude);
						intent.PutExtra ("longitude", myHome.Longitude);
						StartActivity (intent);
					}						
				}
			};

			locManager = GetSystemService (Context.LocationService) as LocationManager;
			locManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);

		}

		public void PutAllMarker ()
		{
			if (allCabins == null) {
				return;
			}
			foreach (var cab in allCabins) {				
				var marker = (new MarkerOptions ().SetPosition (new LatLng (cab.Value.Latitude, cab.Value.Longitude)));
				marker.SetTitle (cab.Value.Name);
				_googleMap.AddMarker (marker);
				var adapter = new InfoWindowAdapter (marker, this, allCabins);
				_googleMap.SetInfoWindowAdapter (adapter);
			}	

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

