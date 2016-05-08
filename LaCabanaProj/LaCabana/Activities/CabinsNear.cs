
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
		private IList<string> favList;

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
				Toast.MakeText (this, "A dat eroare", ToastLength.Short).Show ();
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
						cabinName.Text = cabin.Value.Name;
						cabinDistance.Text = String.Format ("{0} Km", distance.ToString ("0.0"));
						cabinRating.Rating = cabin.Value.Rating;
						cabinPrice.Text = cabin.Value.Price.ToString ();
						if (cabin.Value.Photo != null) {
							var picture = Decode (cabin.Value.Photo [0]);
							cabinPhoto.SetImageBitmap (picture);
						}
						if (favList != null) {
							if (favList.Any (s => cabin.Key.Contains (s))) {
								cabinFav.SetImageResource (Resource.Drawable.ic_heart);
							}
						}
						cabinFav.Click += delegate {
							
							if (userMod.Id != null) {
								cabinFav.SetImageResource (Resource.Drawable.ic_heart);	
								var baseService1 = new BaseService<UsersModel> ();
								var user = DatabaseServices.GetAllUsers ();
								var urlUpdate = string.Format ("users/{0}/FavoriteList", user.Id);
								baseService.Update (cabin.Key, urlUpdate);
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
					Toast.MakeText (this, "Nu exista cabane la distanta de ...", ToastLength.Short).Show ();
					OnBackPressed ();
				}
			});
		}
	}
}

