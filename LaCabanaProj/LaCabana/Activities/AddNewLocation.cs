
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
using Android.Graphics.Drawables;
using System.Threading;

namespace LaCabana
{
	[Activity(Label = "AddNewLocation")]
	public class AddNewLocation : BaseActionActivity, IOnMapReadyCallback, ILocationListener
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
		private Bitmap thePic;
		private double latitude;
		private double longitude;

		protected override void OnCreate(Bundle bundle)
		{

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.add_new_location);
			SetupDrawer(FindViewById<DrawerLayout>(Resource.Id.drawerLayout));
			SetTitleActionBar(GetString(Resource.String.addNewLocation));
			SetProfilePicture();
			ClickHandler();
			cabin = new CabinModel();
			cabin.Photo = new List<string>();
			latitude = Intent.GetDoubleExtra("latitude", 0);
			longitude = Intent.GetDoubleExtra("longitude", 0);
			var extras = Intent.Extras;
			if (extras != null)
			{
				var odsa = extras.GetString("cabin200");
				var allCabins1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, CabinModel>>(odsa);
				CabinsOn200(allCabins1);
			}
			HideKeyboard(this);
			var phoneSpinner = FindViewById<Spinner>(Resource.Id.phoneSpinner);
			var emailSpinner = FindViewById<Spinner>(Resource.Id.emailSpinner);
			var priceSpinner = FindViewById<Spinner>(Resource.Id.priceSpinner);
			var detailsEdit = FindViewById<EditText>(Resource.Id.DetailsEditText);
			var account = FindViewById<EditText>(Resource.Id.accountText);
			var price = FindViewById<EditText>(Resource.Id.priceEditText);
			RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);
			ratingbar.Rating = 3;
			ratingbar.RatingBarChange += (o, e) =>
			{
				cabin.Rating = ratingbar.Progress;
			};
			LayerDrawable stars = (LayerDrawable)ratingbar.ProgressDrawable;
			stars.GetDrawable(2).SetColorFilter(Resources.GetColor(Resource.Color.green), PorterDuff.Mode.SrcAtop);
			locationEdit = FindViewById<TextView>(Resource.Id.locationEditText);
			mapLayout = FindViewById<LinearLayout>(Resource.Id.MapContent);

			DatabaseServices = new DatabaseServices(this);
			var user = DatabaseServices.GetAllUsers();
			if (user != null)
			{
				account.Text = user.Email;
			}
			photoList = new List<string>();
			stringPhone = new List<String>() { "Select", "Mobile", "Home", "Work" };
			stringMail = new List<String>() { "Select", "Custom", "Gmail", "Work" };
			stringPrice = new List<String>() { "Select", "Lei", "Euro", "USD" };

			var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
			mapFragment.GetMapAsync(this);

			var adapter = new FontArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, stringPhone);
			var adapter1 = new FontArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, stringMail);
			var adapter2 = new FontArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, stringPrice);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			phoneSpinner.Adapter = adapter;
			phoneSpinner.SetSelection(0);
			emailSpinner.Adapter = adapter1;
			emailSpinner.SetSelection(0);
			priceSpinner.Adapter = adapter2;
			priceSpinner.SetSelection(0);

			photoShow = FindViewById<ImageView>(Resource.Id.addPhoto);
			photoAdd = FindViewById<ImageView>(Resource.Id.photoShow);
			photoShow.Click += PictureChangeClick;
			var saveButton = FindViewById<Button>(Resource.Id.add_button_location);
			saveButton.Click += delegate
			{
				ThreadPool.QueueUserWorkItem(o => SaveLocation());
			};
			if (account.Text != "")
			{
				cabin.IdAdded = account.Text;
			}
			account.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				cabin.IdAdded = account.Text;
			};

			price.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				if (price.Text != "")
				{
					var floa = float.Parse(price.Text);
					cabin.Price = floa;
				}

			};
			cabin.Pictures = new Dictionary<string, string>();
			detailsEdit.TextChanged += delegate
			{
				cabin.Details = detailsEdit.Text;
			};

			emailSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
			{
				cabin.EmailType = stringMail[e.Position];
			};

			phoneSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
			{
				cabin.PhoneType = stringPhone[e.Position];
			};

			priceSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
			{
				cabin.PriceType = stringPrice[e.Position];
			};

			FindViewById<Button>(Resource.Id.uploadButton).Click += delegate
			{
				cabin.Photo = photoList;
			};

			FindViewById<EditText>(Resource.Id.CabinEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				cabin.Name = e.Text.ToString();
			};

			FindViewById<EditText>(Resource.Id.phoneEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				var phone = e.Text.ToString();
				if (!phone.Equals(""))
					cabin.Phone = Convert.ToInt32(phone);
			};

			FindViewById<EditText>(Resource.Id.emailEditText).TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				cabin.Email = e.Text.ToString();
			};

			locationEdit.Click += delegate
			{
				HideKeyboard(this);
				mapLayout.Visibility = ViewStates.Visible;
			};
			MyPosition(new LatLng(latitude, longitude));
		}

		private void PictureChangeClick(object sender, EventArgs e)
		{
			Intent intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
			StartActivityForResult(intent, 1);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
			{
				_uri = data.Data;
				PerformCrop(_uri);
				//				MemoryStream stream = new MemoryStream ();
				//				var resizebit = Bitmap.CreateScaledBitmap (bitmap, 100, 100, false);
				//				resizebit.Compress (Bitmap.CompressFormat.Png, 100, stream);
				//				resizebit.Recycle ();
				//				byte[] byteArray = stream.ToArray ();
				//				String imageFile = Base64.EncodeToString (byteArray, Base64.Default);
				//				photoList.Add (imageFile);
			}
			else if ((requestCode == 2) && (resultCode == Result.Ok) && (data != null))
			{
				Bundle extras = data.Extras;
				thePic = (Bitmap)extras.GetParcelable("data");
				photoAdd.Visibility = ViewStates.Visible;
				photoAdd.SetScaleType(ImageView.ScaleType.FitXy);
				photoAdd.SetImageBitmap(thePic);

			}
		}

		private void SaveLocation()
		{

			if (cabin.IdAdded == null || cabin.IdAdded.Equals(""))
			{
				CreateDialog(GetString(Resource.String.Enteryourname), "", true, "Ok", false, "", false);
				return;
			}
			if (cabin.Name == null || cabin.Name.Equals(""))
			{
				CreateDialog(GetString(Resource.String.Entercabinname), "", true, "Ok", false, "", false);
				return;
			}
			else if (cabin.Phone == 0 && cabin.Email == null)
			{
				CreateDialog(GetString(Resource.String.Enterphone), "", true, "Ok", false, "", false);
				return;
			}
			else if (cabin.Latitude == 0 || cabin.Longitude == 0)
			{
				CreateDialog(GetString(Resource.String.Enterlocation), "", true, "Ok", false, "", false);
				return;
			}
			else if (photoList.Count != cabin.Photo.Count)
			{
				CreateDialog(GetString(Resource.String.Uploadphoto), "", true, "Ok", false, "", false);
				return;
			}
			else if (cabin.Email == null || cabin.Email.Equals(""))
			{
				CreateDialog(GetString(Resource.String.enteremail), "", true, "Ok", false, "", false);
				return;
			}
			else if (cabin.Price == null || cabin.Price == 0)
			{
				CreateDialog(GetString(Resource.String.invalidte_price), "", true, "Ok", false, "", false);
				return;
			}
			CreateDialog("", GetString(Resource.String.wait), false, "", false, "", true);
			if (cabin.Rating == 0)
			{
				cabin.Rating = 3;
			}

			if (cabin.PhoneType.Equals("Select"))
			{
				cabin.PhoneType = stringPhone[1];
			}
			if (cabin.EmailType.Equals("Select"))
			{
				cabin.EmailType = stringMail[1];
			}
			if (cabin.PriceType.Equals("Select"))
			{
				cabin.PriceType = stringPrice[1];
			}

			if (thePic != null)
			{
				var bos = new MemoryStream();
				thePic.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 70, bos);
				var asd = bos.ToArray();
				var baseService2 = new BaseService<byte[]>();
				try
				{
					var result = baseService2.Push(asd, "pictures");
					if (result != null)
					{
						cabin.Pictures.Add("main", result.Result.Name);
					}
				}
				catch (Exception e)
				{
					CreateDialog(GetString(Resource.String.Error), GetString(Resource.String.Unabletosave), false, "", true, GetString(Resource.String.Cancel), false);
				}
			}
			var baseService = new BaseService<CabinModel>();
			try
			{
				var result1 = baseService.Push(cabin, "cabins");
				StartActivityForResult(typeof(BasicMapDemoActivity), 2);
				Finish();
			}
			catch (Exception e)
			{
				CreateDialog(GetString(Resource.String.Error), GetString(Resource.String.Unabletosave), false, "", true, GetString(Resource.String.Cancel), false);
			}

		}

		public void OnMapReady(GoogleMap googleMap)
		{
			var myHome = new LatLng(_latitude, _longitude);
			googleMap.MyLocationEnabled = true;
			_googleMap = googleMap;
			var isCurent = false;
			googleMap.MyLocationChange += (object sender, GoogleMap.MyLocationChangeEventArgs e) =>
			{
				if (isCurent)
					return;
				isCurent = true;
				homeMarker = googleMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(e.Location.Latitude, e.Location.Longitude)));
				homeMarker.Title = "myLocation";
				googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(e.Location.Latitude, e.Location.Longitude), 10));
			};

			googleMap.MapLongClick += (object sender, GoogleMap.MapLongClickEventArgs e) =>
			{
				cabin.Latitude = e.Point.Latitude;
				cabin.Longitude = e.Point.Longitude;
				locationEdit.Text = locationEdit.Text = string.Format("{0} , {1}", e.Point.Latitude, e.Point.Longitude);
				mapLayout.Visibility = ViewStates.Gone;
			};

			locManager = GetSystemService(Context.LocationService) as LocationManager;
			locManager.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);

		}

		public void PerformCrop(Android.Net.Uri selectedImage)
		{
			try
			{
				Intent cropIntent = new Intent("com.android.camera.action.CROP");
				//indicate image type and Uri
				cropIntent.SetDataAndType(selectedImage, "image/*");
				//set crop properties
				cropIntent.PutExtra("crop", "true");
				//indicate aspect of desired crop
				cropIntent.PutExtra("aspectX", 0.4);
				cropIntent.PutExtra("aspectY", 1);
				//indicate output X and Y
				cropIntent.PutExtra("outputX", 256);
				cropIntent.PutExtra("outputY", 150);
				//retrieve data on return
				cropIntent.PutExtra("return-data", true);
				//start the activity - we handle returning in onActivityResult
				StartActivityForResult(cropIntent, 2);
			}
			catch (ActivityNotFoundException anfe)
			{

				String errorMessage = "Whoops - your device doesn't support the crop action!";
				Toast toast = Toast.MakeText(this, errorMessage, ToastLength.Short);
				toast.Show();
			}


		}

		public void OnLocationChanged(Android.Locations.Location location)
		{
			Log.Debug(tag, "Location changed");
			_latitude = location.Latitude;
			_longitude = location.Longitude;
			_provider = location.Provider.ToString();
		}

		public void OnProviderDisabled(string provider)
		{
			Log.Debug(tag, provider + " disabled by user");
		}

		public void OnProviderEnabled(string provider)
		{
			Log.Debug(tag, provider + " enabled by user");
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			Log.Debug(tag, provider + " availability has changed to " + status.ToString());
		}



	}


}


