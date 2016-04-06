
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
using Android.Gms.Maps.Model;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]		
	public class CabinInfo : BaseDrawerActivity
	{
		private CabinModel cabin;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.cabin_info_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Cabin");
			var marker = Intent.GetStringExtra ("marker");
			var baseService = new BaseService<CabinModel> ();
			cabin = new CabinModel ();
			var cabinPhoto = FindViewById<ImageView> (Resource.Id.cabinImage);
			var cabinName = FindViewById<TextView> (Resource.Id.cabinName);
			var phoneText1 = FindViewById<TextView> (Resource.Id.phoneText1);
			var phoneType1 = FindViewById<TextView> (Resource.Id.phoneType1);
			var phoneText2 = FindViewById<TextView> (Resource.Id.phoneText2);
			var phoneType2 = FindViewById<TextView> (Resource.Id.phoneType2);
			var emailText = FindViewById<TextView> (Resource.Id.emailText);
			var emailType = FindViewById<TextView> (Resource.Id.emailType);
			var streetText = FindViewById<TextView> (Resource.Id.streetText);
			var priceText = FindViewById<TextView> (Resource.Id.priceText);
			var route = new RouteGenerator ();

			if (marker != null) {
				try {
					cabin = (baseService.Get (string.Format ("cabins/{0}", marker)));	
				} catch (Exception e) {
					var a = 0;
				}
				//foreach (var cabin in allCabins) {
				//if (cabin.Name.Equals (marker)) {
				cabinName.Text = cabin.Name;
				phoneText1.Text = cabin.Phone.ToString ();
				phoneType1.Text = cabin.PhoneType;
				emailText.Text = cabin.Email;
				emailType.Text = cabin.EmailType;
				priceText.Text = cabin.Price.ToString ();
				if (cabin.Photo != null) {
					var picture = Decode (cabin.Photo [0]);
					cabinPhoto.SetImageBitmap (picture);
				}
				var latitude = Intent.GetDoubleExtra ("latitude", 0);
				var longitude = Intent.GetDoubleExtra ("longitude", 0);
				Org.W3c.Dom.IDocument doc = route.GetDocument (new LatLng (latitude, longitude), new LatLng (cabin.Latitude, cabin.Longitude), RouteGenerator.Mode_driving);
				streetText.Text = route.GetEndAddress (doc);
				//}
			}
		}
	}
}


