﻿using System;
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
	[Activity (Label = "FavoritesActivity")]			
	public class FavoritesActivity : BaseActionActivity
	{
		IDatabaseServices DatabaseServices;
		private List<CabinModel> allCabins;
		private double latitude;
		private double longitude;
		private int numberDistance;
		private LinearLayout cabinsLayout;
		private bool notFav = false;
		private IList<String> favList;
		private GridView gridView;
		private RelativeLayout loading;

		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);
			SetContentView (Resource.Layout.favorite_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			DatabaseServices = new DatabaseServices (this);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetProfilePicture ();
			var baseService = new BaseService<Dictionary<string,CabinModel>> ();
			latitude = Intent.GetDoubleExtra ("latitude", 0);
			longitude = Intent.GetDoubleExtra ("longitude", 0);
			numberDistance = Intent.GetIntExtra ("distance", 0);
			favList = Intent.GetStringArrayListExtra ("favoriteList");
			//allCabins = new Dictionary<string,CabinModel> ();
			allCabins = new List<CabinModel> ();
			SetTitleActionBar ("Favorites");
			ClickHandler ();
			gridView = FindViewById<GridView> (Resource.Id.image_recycler);
			loading = FindViewById<RelativeLayout> (Resource.Id.main_loading_recycler);
			gridView.Visibility = ViewStates.Gone;
			loading.Visibility = ViewStates.Visible;
			cabinsLayout = FindViewById<LinearLayout> (Resource.Id.FlyContent);
			ThreadPool.QueueUserWorkItem (o => GetData ());			
			MyPosition (new LatLng (latitude, longitude));


		}

		public void GetData ()
		{
			var route = new RouteGenerator ();
			var baseService1 = new BaseService<CabinModel> ();

			try {	
				foreach (var favorit in favList) {
					var cabama = baseService1.Get ("cabins/" + favorit);
					allCabins.Add (cabama);
				}
				
			} catch (Exception e) {
				//Toast.MakeText (this, "A dat eroare", ToastLength.Short).Show ();
			}

			RunOnUiThread (() => {
				var adapter = new ImageAdapter (this, allCabins, latitude, longitude);
				gridView.Adapter = adapter;
				gridView.Visibility = ViewStates.Visible;
				loading.Visibility = ViewStates.Gone;
//				foreach (var cabin in allCabins) {
//					Org.W3c.Dom.IDocument doc = route.GetDocument (new LatLng (latitude, longitude), new LatLng (cabin.Value.Latitude, cabin.Value.Longitude), RouteGenerator.Mode_driving);
//					float distance = route.GetDistanceValue (doc) / 1000;							
//
//					var view = LayoutInflater.Inflate (Resource.Layout.cabin_view_layout, null);
//					var cabinName = view.FindViewById<TextView> (Resource.Id.cabinName);
//					var cabinRating = view.FindViewById<RatingBar> (Resource.Id.cabinRating);
//					var cabinPrice = view.FindViewById<TextView> (Resource.Id.cabinPrice);
//					var cabinPhoto = view.FindViewById<ImageView> (Resource.Id.cabinImage);
//					var cabinDistance = view.FindViewById<TextView> (Resource.Id.cabinDistance);
//					var cabinFav = view.FindViewById<ImageView> (Resource.Id.favoriteImage);
//					cabinName.Text = cabin.Value.Name;
//					cabinDistance.Text = String.Format ("{0} Km", distance.ToString ("0.0"));
//					cabinRating.Rating = cabin.Value.Rating;
//					cabinPrice.Text = cabin.Value.Price.ToString ();
//					if (cabin.Value.Photo != null) {
//						var picture = Decode (cabin.Value.Photo [0]);
//						cabinPhoto.SetImageBitmap (picture);
//					}
//					cabinFav.Click += delegate {
//						cabinFav.SetImageResource (Resource.Drawable.ic_heart);	
////							var baseService1 = new BaseService<UsersModel> ();
////							var user = DatabaseServices.GetAllUsers ();
////							var urlUpdate = string.Format ("users/{0}/FavoriteList", user.Id);
////							baseService.Update (cabin.Key, urlUpdate);
//					};
//					view.FindViewById<TextView> (Resource.Id.detailsButton).Click += delegate {
//						var intent = new Intent (this, typeof(CabinInfo));				
//						intent.PutExtra ("marker", cabin.Key);
//						intent.PutExtra ("latitude", latitude);
//						intent.PutExtra ("longitude", longitude);
//						StartActivity (intent);
//					};
//					cabinsLayout.AddView (view);
//
//				}
//				cabinsLayout.RequestLayout ();
//				if (cabinsLayout.ChildCount == 0) {
//					Toast.MakeText (this, "Nu exista cabane la distanta de ...", ToastLength.Short).Show ();
//					OnBackPressed ();
//				}
			});
		}
	}
}

