using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Gms.Maps.Model;
using Android.Graphics;
using System.Linq;

namespace LaCabana
{
	public class ImageAdapter : BaseAdapter
	{
		Context context;
		List<CabinModel> allCabins;
		double _latitude;
		double _longitude;
		IList<String> favLists;

		public ImageAdapter(Context c, List<CabinModel> cabins, double latitude, double longitude, IList<String> favList)
		{
			context = c;
			allCabins = cabins;
			_latitude = latitude;
			_longitude = longitude;
			favLists = favList;
		}

		public override int Count
		{
			get { return allCabins.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var route = new RouteGenerator();
			var item = allCabins[position];
			var item1 = favLists[position];
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.favorite_card, parent, false);
			var cabinName = itemView.FindViewById<TextView>(Resource.Id.cabinName);
			var cabinPrice = itemView.FindViewById<TextView>(Resource.Id.cabinPrice);
			var cabinDistance = itemView.FindViewById<TextView>(Resource.Id.cabinDistance);
			var cabinPhoto = itemView.FindViewById<ImageView>(Resource.Id.cabinImage);
			var cabinInfo = itemView.FindViewById<ImageView>(Resource.Id.cabinInfo);
			var cabinDirection = itemView.FindViewById<ImageView>(Resource.Id.cabinDirection);
			var cabinFav = itemView.FindViewById<ImageView>(Resource.Id.cabinFavorite);
			cabinName.Text = item.Name;
			cabinPrice.Text = item.Price.ToString() + " " + item.PriceType;
			Org.W3c.Dom.IDocument doc = route.GetDocument(new LatLng(_latitude, _longitude), new LatLng(item.Latitude, item.Longitude), RouteGenerator.Mode_driving);
			float distance = route.GetDistanceValue(doc) / 1000;
			cabinDistance.Text = String.Format("{0} Km", distance.ToString("0.0"));
			if (item.Pictures != null)
			{
				if (item.Pictures.ContainsKey("main"))
				{
					var baseService1 = new BaseService<byte[]>();
					var abc = baseService1.Get(string.Format("pictures/{0}", item.Pictures.Last().Value));
					cabinPhoto.SetImageBitmap(BitmapFactory.DecodeByteArray(abc, 0, abc.Length));
					cabinPhoto.SetScaleType(ImageView.ScaleType.FitXy);
				}
				else {
					cabinPhoto.SetImageResource(Resource.Drawable.cabana_photo);
					cabinPhoto.SetScaleType(ImageView.ScaleType.CenterCrop);
				}
			}
			else {
				cabinPhoto.SetImageResource(Resource.Drawable.cabana_photo);
				cabinPhoto.SetScaleType(ImageView.ScaleType.CenterCrop);
			}

			cabinInfo.Click += delegate
			{
				var intent = new Intent(context, typeof(CabinInfo));
				intent.PutExtra("marker", item1);
				intent.PutExtra("latitude", _latitude);
				intent.PutExtra("longitude", _longitude);
				context.StartActivity(intent);
			};
			itemView.Click += delegate
			 {
				 var intent = new Intent(context, typeof(CabinInfo));
				 intent.PutExtra("marker", item1);
				 intent.PutExtra("latitude", _latitude);
				 intent.PutExtra("longitude", _longitude);
				 context.StartActivity(intent);
			 };
			cabinDirection.Click += delegate
			{
				var newuri = string.Format("http://maps.google.com/maps?saddr={0},{1}&daddr={2},{3}", _latitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 _longitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 item.Latitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 item.Longitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture));

				Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse(newuri);
				Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
				mapIntent.SetPackage("com.google.android.apps.maps");
				context.StartActivity(mapIntent);
			};

			return itemView;
		}

	}
}

