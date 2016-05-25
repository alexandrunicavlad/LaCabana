
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
using LaCabana.Services;
using Android.Support.V4.Widget;
using Android.Graphics;
using Android.Gms.Maps.Model;
using System.Threading;

namespace LaCabana
{
	[Activity (Label = "CabinsNear")]			
	public class CabinsNear : BaseActionActivity
	{
		IDatabaseServices DatabaseServices;
		private Dictionary<string,CabinModel> allCabins;
		private double latitude;
		private double longitude;
		private int numberDistance;
		private LinearLayout cabinsLayout;
		private RelativeLayout loading;
		private IList<string> favList;
		private ScrollView scrollView;

		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);
			SetContentView (Resource.Layout.cabins_near_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			DatabaseServices = new DatabaseServices (this);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetProfilePicture ();
			var baseService = new BaseService<Dictionary<string,CabinModel>> ();
			latitude = Intent.GetDoubleExtra ("latitude", 0);
			longitude = Intent.GetDoubleExtra ("longitude", 0);
			numberDistance = Intent.GetIntExtra ("distance", 0);
			favList = Intent.GetStringArrayListExtra ("favoriteList");
			allCabins = new Dictionary<string,CabinModel> ();
			SetTitleActionBar ("Cabins near");
			ClickHandler ();
			loading = FindViewById<RelativeLayout> (Resource.Id.main_loading);
			scrollView = FindViewById<ScrollView> (Resource.Id.ScrollList);
			loading.Visibility = ViewStates.Visible;
			scrollView.Visibility = ViewStates.Gone;
			cabinsLayout = FindViewById<LinearLayout> (Resource.Id.FlyContent);
			ThreadPool.QueueUserWorkItem (o => GetData ());			
			MyPosition (new LatLng (latitude, longitude));


		}

		public void GetData ()
		{		
			var route = new RouteGenerator ();
			var baseService = new BaseService<Dictionary<string,CabinModel>> ();
			try {				
				allCabins = (baseService.Get ("cabins"));
			} catch (Exception e) {
				//Toast.MakeText (this, "A dat eroare", ToastLength.Short).Show ();
			}
			RunOnUiThread (() => {
				foreach (var cabin in allCabins) {
					
					Org.W3c.Dom.IDocument doc = route.GetDocument (new LatLng (latitude, longitude), new LatLng (cabin.Value.Latitude, cabin.Value.Longitude), RouteGenerator.Mode_driving);
					float distance = route.GetDistanceValue (doc) / 1000;
					if (distance > numberDistance) {
					} else {				
					
						var view = LayoutInflater.Inflate (Resource.Layout.cabin_view_layout, null);
						var cabinName = view.FindViewById<TextView> (Resource.Id.cabinName);
						var cabinRating = view.FindViewById<RatingBar> (Resource.Id.cabinRating);
						var cabinPrice = view.FindViewById<TextView> (Resource.Id.cabinPrice);
						var cabinPhoto = view.FindViewById<ImageView> (Resource.Id.cabinImage);
						var cabinDistance = view.FindViewById<TextView> (Resource.Id.cabinDistance);
						var cabinFav = view.FindViewById<ImageView> (Resource.Id.favoriteImage);
						var cabinDetails = view.FindViewById<TextView> (Resource.Id.cabinDetails);
						var direction = view.FindViewById<TextView> (Resource.Id.destinationButton);
						cabinName.Text = cabin.Value.Name;
						cabinDistance.Text = String.Format ("{0} Km", distance.ToString ("0.0"));
						cabinRating.Rating = cabin.Value.Rating;
						cabinPrice.Text = string.Format ("{0} {1}", cabin.Value.Price.ToString (), cabin.Value.PriceType);
						cabinDetails.Text = cabin.Value.Details;
						if (cabin.Value.Photo != null) {
							var picture = Decode (cabin.Value.Photo [0]);
							cabinPhoto.SetImageBitmap (picture);
						} else {
							cabinPhoto.SetImageResource (Resource.Drawable.cabana_photo);
							cabinPhoto.SetScaleType (ImageView.ScaleType.CenterCrop);
						}
						direction.Click += delegate {
							var newuri = string.Format ("http://maps.google.com/maps?saddr={0},{1}&daddr={2},{3}", latitude.ToString ("00.0000000", System.Globalization.CultureInfo.InvariantCulture), 
								             longitude.ToString ("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								             cabin.Value.Latitude.ToString ("00.0000000", System.Globalization.CultureInfo.InvariantCulture), 
								             cabin.Value.Longitude.ToString ("00.0000000", System.Globalization.CultureInfo.InvariantCulture));

							Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse (newuri);
							Intent mapIntent = new Intent (Intent.ActionView, gmmIntentUri);
							mapIntent.SetPackage ("com.google.android.apps.maps");
							StartActivity (mapIntent);
						};

						if (favList != null) {
							if (favList.Any (s => cabin.Key.Contains (s))) {
								cabinFav.SetImageResource (Resource.Drawable.ic_heart);
							}
						} else {
							favList = new List<string> ();
						}
						cabinFav.Click += delegate {		
							if (favList.Contains (cabin.Key))
								return;
							if (userMod.Id != null) {
								cabinFav.SetImageResource (Resource.Drawable.ic_heart);	
								var baseService1 = new BaseService<UsersModel> ();
								var baseService2 = new BaseService<UsersModel> ();
								var user = DatabaseServices.GetAllUsers ();
								var urlUpdate = string.Format ("users/{0}/FavoriteList", user.Id);
								baseService.Update (cabin.Key, urlUpdate);
								var newUrl = string.Format ("users/{0}", user.Id);
								try {
									var ada = baseService2.Get (newUrl);
									DatabaseServices.DeleteUser ();
									DatabaseServices.InsertUsername (ada);
									SetProfilePicture ();
								} catch (Exception e) {
									var a = 0;
								}
							} else {
								Toast.MakeText (this, "Please login for select this page", ToastLength.Short).Show ();
							}
							 
						};
						view.FindViewById<TextView> (Resource.Id.detailsButton).Click += delegate {
							var intent = new Intent (this, typeof(CabinInfo));				
							intent.PutExtra ("marker", cabin.Key);
							intent.PutExtra ("latitude", latitude);
							intent.PutExtra ("longitude", longitude);
							StartActivity (intent);
						};
						cabinsLayout.AddView (view);
					}
				}
				cabinsLayout.RequestLayout ();
				if (cabinsLayout.ChildCount == 0) {
					Toast.MakeText (this, string.Format ("Nu exista cabane la distanta de {0} km", numberDistance), ToastLength.Short).Show ();
					OnBackPressed ();
				}
				loading.Visibility = ViewStates.Gone;
				scrollView.Visibility = ViewStates.Visible;
			});
		}
	}
}

