using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Gms.Maps.Model;

namespace LaCabana
{
	public class ImageAdapter : BaseAdapter
	{
		Context context;
		List<CabinModel> allCabins;
		double _latitude;
		double _longitude;

		public ImageAdapter (Context c, List<CabinModel> cabins, double latitude, double longitude)
		{
			context = c;
			allCabins = cabins;
			_latitude = latitude;
			_longitude = longitude;
		}

		public override int Count {
			get { return allCabins.Count; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var route = new RouteGenerator ();
			var item = allCabins [position];
			View itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.favorite_card, parent, false);
			var cabinName = itemView.FindViewById<TextView> (Resource.Id.cabinName);
			var cabinPrice = itemView.FindViewById<TextView> (Resource.Id.cabinPrice);
			var cabinDistance = itemView.FindViewById<TextView> (Resource.Id.cabinDistance);
			var cabinPhoto = itemView.FindViewById<ImageView> (Resource.Id.cabinImage);
			cabinName.Text = item.Name;
			cabinPrice.Text = item.Price.ToString ();
			Org.W3c.Dom.IDocument doc = route.GetDocument (new LatLng (_latitude, _longitude), new LatLng (item.Latitude, item.Longitude), RouteGenerator.Mode_driving);
			float distance = route.GetDistanceValue (doc) / 1000;
			cabinDistance.Text = String.Format ("{0} Km", distance.ToString ("0.0"));
			if (item.Photo == null) {
				cabinPhoto.SetImageResource (Resource.Drawable.ic_no_photo);
				cabinPhoto.SetScaleType (ImageView.ScaleType.Center);
			} else {				

			}
			return itemView;
		}

	}
}

