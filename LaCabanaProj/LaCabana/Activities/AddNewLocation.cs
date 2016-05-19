﻿
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
using Uri = Android.Net.Uri;
using Java.IO;
using Android.Provider;
using LaCabana.Services;
using Android.Gms.Maps;
using Android.Locations;
using Android.Gms.Maps.Model;
using Android.Util;
using Android.Graphics;
using System.IO;

namespace LaCabana
{
	[Activity (Label = "AddNewLocation")]			
	public class AddNewLocation : BaseActionActivity, IOnMapReadyCallback,ILocationListener
	{
		private Uri _uri;
		private ImageView photoShow;
		private string imagePath;
		private ImageView photoAdd;
		private List<Uri> listOfUri;
		private int count;
		protected static IDatabaseServices DatabaseServices;
		private CabinModel cabin;
		private List<String> stringPhone;
		private List<String> stringMail;
		private List<String> stringPrice;
		private GoogleMap _googleMap;
		private string tag = "MainActivity";
		private double _latitude;
		private double _longitude;
		private string _provider;
		LocationManager locManager;
		Marker homeMarker;
		private LinearLayout mapLayout;
		private TextView locationEdit;
		private List<String> photoList;

		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.add_new_location);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Add new location");
			ClickHandler ();
			SetProfilePicture ();
			cabin = new CabinModel ();
			cabin.Photo = new List<string> ();
			var latitude = Intent.GetDoubleExtra ("latitude", 0);
			var longitude = Intent.GetDoubleExtra ("longitude", 0);
			HideKeyboard (this);
			var phoneSpinner = FindViewById<Spinner> (Resource.Id.phoneSpinner);
			var emailSpinner = FindViewById<Spinner> (Resource.Id.emailSpinner);
			var priceSpinner = FindViewById<Spinner> (Resource.Id.priceSpinner);
			var detailsEdit = FindViewById<EditText> (Resource.Id.DetailsEditText);
			var account = FindViewById<EditText> (Resource.Id.accountText);
			var price = FindViewById<EditText> (Resource.Id.priceEditText);
			RatingBar ratingbar = FindViewById<RatingBar> (Resource.Id.ratingbar);

			ratingbar.RatingBarChange += (o, e) => {
				cabin.Rating = ratingbar.Progress;
			};
			locationEdit = FindViewById<TextView> (Resource.Id.locationEditText);
			mapLayout = FindViewById<LinearLayout> (Resource.Id.MapContent);

			DatabaseServices = new DatabaseServices (this);
			var user = DatabaseServices.GetAllUsers ();
			if (user != null) {
				account.Text = user.Email;
			}
			photoList = new List<string> ();
			stringPhone = new List<String> (){ "Select", "Mobile", "Home", "Work" };
			stringMail = new List<String> (){ "Select", "Custom", "Gmail", "Work" };
			stringPrice = new List<String> (){ "Select", "Lei", "Euro", "USD" };

			var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById (Resource.Id.map);
			mapFragment.GetMapAsync (this);

			var adapter = new FontArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, stringPhone);
			var adapter1 = new FontArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, stringMail);
			var adapter2 = new FontArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, stringPrice);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter1.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter2.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			phoneSpinner.Adapter = adapter;
			phoneSpinner.SetSelection (0);
			emailSpinner.Adapter = adapter1;
			emailSpinner.SetSelection (0);
			priceSpinner.Adapter = adapter2;
			priceSpinner.SetSelection (0);

			photoShow = FindViewById<ImageView> (Resource.Id.addPhoto);
			photoAdd = FindViewById<ImageView> (Resource.Id.photoShow);
			photoShow.Click += PictureChangeClick;
			var saveButton = FindViewById<Button> (Resource.Id.add_button_location);
			saveButton.Click += delegate {
				SaveLocation ();
			};
			if (account.Text != "") {
				cabin.IdAdded = account.Text;
			}
			account.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				cabin.IdAdded = account.Text;
			};

			price.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				var floa = float.Parse (price.Text);
				cabin.Price = floa;
			};

			detailsEdit.TextChanged += delegate {
				cabin.Details = detailsEdit.Text;
			};

			emailSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				cabin.EmailType = stringMail [e.Position];
			};

			phoneSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				cabin.PhoneType = stringPhone [e.Position];
			};

			priceSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				cabin.PriceType = stringPrice [e.Position];
			};

			FindViewById<Button> (Resource.Id.uploadButton).Click += delegate {
				cabin.Photo = photoList;
			};

			FindViewById<EditText> (Resource.Id.CabinEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				cabin.Name = e.Text.ToString ();
			};

			FindViewById<EditText> (Resource.Id.phoneEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				var phone = e.Text.ToString ();
				cabin.Phone = Convert.ToInt32 (phone);
			};

			FindViewById<EditText> (Resource.Id.emailEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				cabin.Email = e.Text.ToString ();
			};

			locationEdit.Click += delegate {			
				HideKeyboard (this);	
				mapLayout.Visibility = ViewStates.Visible;
			};
		}

		private void PictureChangeClick (object sender, EventArgs e)
		{			
			var intent = new Intent ();
			intent.SetType ("image/*");
			intent.SetAction (Intent.ActionGetContent);
			photoAdd.Visibility = ViewStates.Visible;
			StartActivityForResult (Intent.CreateChooser (intent, "Select Picture"), 0);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null)) {
				_uri = data.Data;
				photoAdd.SetImageURI (_uri);
				var bitmap = MediaStore.Images.Media.GetBitmap (this.ContentResolver, _uri);
//				MemoryStream stream = new MemoryStream ();
//				var resizebit = Bitmap.CreateScaledBitmap (bitmap, 100, 100, false);
//				resizebit.Compress (Bitmap.CompressFormat.Png, 100, stream);
//				resizebit.Recycle ();
//				byte[] byteArray = stream.ToArray ();
//				String imageFile = Base64.EncodeToString (byteArray, Base64.Default);
//				photoList.Add (imageFile);
			}
		}

		private void SaveLocation ()
		{		
			
			if (cabin.Name == null || cabin.Name.Equals ("")) {
				Toast.MakeText (this, "Enter a Cabin's name", ToastLength.Short).Show ();
				return;
			} else if (cabin.Phone == 0 && cabin.Email == null) {
				Toast.MakeText (this, "Enter a phone number or an email", ToastLength.Short).Show ();
				return;
			} else if (cabin.Latitude == 0 || cabin.Longitude == 0) {
				Toast.MakeText (this, "Enter location", ToastLength.Short).Show ();
				return;
			} else if (photoList.Count != cabin.Photo.Count) {
				Toast.MakeText (this, "Please upload photo", ToastLength.Short).Show ();
				return;
			}
			if (cabin.Rating == 0) {
				cabin.Rating = 3;
			}
			if (cabin.PhoneType.Equals ("Select")) {
				cabin.PhoneType = stringPhone [1];
			}
			if (cabin.EmailType.Equals ("Select")) {
				cabin.EmailType = stringMail [1];
			}
			if (cabin.PriceType.Equals ("Select")) {
				cabin.PriceType = stringPrice [1];
			}
			var baseService = new BaseService<CabinModel> ();
			try {
				baseService.Push (cabin, "cabins");
				StartActivityForResult (typeof(BasicMapDemoActivity), 2);
				Finish ();
			} catch (Exception e) {
				var a = 0;
			}
			if (cabin.PhoneType.Equals ("Select")) {
				cabin.PhoneType = stringPhone [1];
			}
			if (cabin.EmailType.Equals ("Select")) {
				cabin.EmailType = stringMail [1];
			}
			if (cabin.EmailType.Equals ("Select")) {
				cabin.PriceType = stringPrice [1];
			}
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			var myHome = new LatLng (_latitude, _longitude);
			googleMap.MyLocationEnabled = true;
			_googleMap = googleMap;
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
				cabin.Latitude = e.Point.Latitude;
				cabin.Longitude = e.Point.Longitude;
				locationEdit.Text = locationEdit.Text = string.Format ("{0} , {1}", e.Point.Latitude, e.Point.Longitude);
				mapLayout.Visibility = ViewStates.Gone;
			};

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


