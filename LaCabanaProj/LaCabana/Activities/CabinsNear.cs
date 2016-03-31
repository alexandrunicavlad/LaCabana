
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

namespace LaCabana
{
	[Activity (Label = "CabinsNear")]			
	public class CabinsNear : BaseActionActivity
	{
		IDatabaseServices DatabaseServices;
		private List<CabinModel> allCabins;

		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.cabins_near_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			DatabaseServices = new DatabaseServices (this);

			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			allCabins = DatabaseServices.GetAllCabins ();

			SetTitleActionBar ("Cabins near");
			ClickHandler ();
			var cabinsLayout = FindViewById<LinearLayout> (Resource.Id.FlyContent);
			foreach (var cabin in allCabins) {
				var view = LayoutInflater.Inflate (Resource.Layout.cabin_view_layout, null);
				var cabinName = view.FindViewById<TextView> (Resource.Id.cabinName);
				var cabinRating = view.FindViewById<RatingBar> (Resource.Id.cabinRating);
				var cabinPrice = view.FindViewById<TextView> (Resource.Id.cabinPrice);
				cabinName.Text = cabin.Name;
				cabinRating.Progress = cabin.Rating;
				cabinPrice.Text = cabin.Price.ToString ();
				cabinsLayout.AddView (view);
			}
			cabinsLayout.RequestLayout ();
		}
	}
}

