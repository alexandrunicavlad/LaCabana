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
	[Activity(Label = "FavoritesActivity")]
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

		protected override void OnCreate(Bundle bundle)
		{

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.favorite_layout);
			SetupDrawer(FindViewById<DrawerLayout>(Resource.Id.drawerLayout));
			DatabaseServices = new DatabaseServices(this);
			SetupDrawer(FindViewById<DrawerLayout>(Resource.Id.drawerLayout));
			SetProfilePicture();
			var baseService = new BaseService<Dictionary<string, CabinModel>>();
			latitude = Intent.GetDoubleExtra("latitude", 0);
			longitude = Intent.GetDoubleExtra("longitude", 0);
			numberDistance = Intent.GetIntExtra("distance", 0);
			favList = Intent.GetStringArrayListExtra("favoriteList");
			var extras = Intent.Extras;
			if (extras != null)
			{
				var odsa = extras.GetString("cabin200");
				var allCabins1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, CabinModel>>(odsa);
				CabinsOn200(allCabins1);
			}
			//allCabins = new Dictionary<string,CabinModel> ();
			allCabins = new List<CabinModel>();
			SetTitleActionBar(GetString(Resource.String.favorites));
			ClickHandler();
			gridView = FindViewById<GridView>(Resource.Id.image_recycler);
			loading = FindViewById<RelativeLayout>(Resource.Id.main_loading_recycler);
			gridView.Visibility = ViewStates.Gone;
			loading.Visibility = ViewStates.Visible;
			cabinsLayout = FindViewById<LinearLayout>(Resource.Id.FlyContent);
			ThreadPool.QueueUserWorkItem(o => GetData());
			MyPosition(new LatLng(latitude, longitude));

		}

		public void GetData()
		{
			var route = new RouteGenerator();
			var baseService1 = new BaseService<CabinModel>();
			var ada = favList[0];
			if (favList[0] == null)
			{
				Toast.MakeText(this, GetString(Resource.String.cabinnotselect), ToastLength.Short).Show();
				OnBackPressed();
			}
			try
			{
				foreach (var favorit in favList)
				{
					var cabama = baseService1.Get("cabins/" + favorit);
					allCabins.Add(cabama);
				}
			}
			catch (Exception e)
			{
				CreateDialog(GetString(Resource.String.Error), GetString(Resource.String.networkconnection), false, "", true, GetString(Resource.String.Cancel), false);
			}

			RunOnUiThread(() =>
			{
				var adapter = new ImageAdapter(this, allCabins, latitude, longitude, favList);
				gridView.Adapter = adapter;
				gridView.Visibility = ViewStates.Visible;
				loading.Visibility = ViewStates.Gone;

			});
		}
	}
}

