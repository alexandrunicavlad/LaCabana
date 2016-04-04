
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

namespace LaCabana
{
	[Activity (Label = "CabinsNear")]			
	public class CabinsNear : BaseActionActivity
	{
		IDatabaseServices DatabaseServices;
		private Dictionary<string,CabinModel> allCabins;

		protected override void OnCreate (Bundle bundle)
		{			

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.cabins_near_layout);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			DatabaseServices = new DatabaseServices (this);
			SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			var baseService = new BaseService<Dictionary<string,CabinModel>> ();
			allCabins = new Dictionary<string,CabinModel> ();
			try {
				allCabins = (baseService.Get ("cabins"));	
			} catch (Exception e) {
				var a = 0;
			}

			SetTitleActionBar ("Cabins near");
			ClickHandler ();
			var cabinsLayout = FindViewById<LinearLayout> (Resource.Id.FlyContent);
			foreach (var cabin in allCabins) {
				var view = LayoutInflater.Inflate (Resource.Layout.cabin_view_layout, null);
				var cabinName = view.FindViewById<TextView> (Resource.Id.cabinName);
				var cabinRating = view.FindViewById<RatingBar> (Resource.Id.cabinRating);
				var cabinPrice = view.FindViewById<TextView> (Resource.Id.cabinPrice);
				var cabinPhoto = view.FindViewById<ImageView> (Resource.Id.cabinImage);
				cabinName.Text = cabin.Value.Name;
				cabinRating.Progress = cabin.Value.Rating;
				cabinPrice.Text = cabin.Value.Price.ToString ();
				if (cabin.Value.Photo != null) {
					var picture = Decode (cabin.Value.Photo [0]);
					cabinPhoto.SetImageBitmap (picture);
				}
				cabinsLayout.AddView (view);
			}
			cabinsLayout.RequestLayout ();
		}

		public  Bitmap Decode (string imageData)
		{
			try {
				byte[] encodeByte = Android.Util.Base64.Decode (imageData, Android.Util.Base64Flags.Default);
				Bitmap bitmap = BitmapFactory.DecodeByteArray (encodeByte, 0, encodeByte.Length);
				return bitmap;
			} catch (Exception) {
				return null;
			}
		}
	}
}

