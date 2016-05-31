using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Content;
using Android.App;
using Android.Widget;
using System.Collections.Generic;

namespace LaCabana
{
	public class InfoWindowAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
	{
		private MarkerOptions _marker;
		private Activity _context;
		private Dictionary<string,CabinModel> _cabins;

		public InfoWindowAdapter (MarkerOptions marker, Activity context, Dictionary<string,CabinModel> cabins)
		{
			_marker = marker;
			_context = context;
			_cabins = cabins;
		}

		public View GetInfoContents (Marker p0)
		{
			LayoutInflater inflater;
			inflater = (LayoutInflater)_context.GetSystemService (Context.LayoutInflaterService);
			var itemLayout = inflater.Inflate (Resource.Layout.infoWindow_layout, null);
			LatLng latLng = _marker.Position;
			var name = itemLayout.FindViewById<TextView> (Resource.Id.infoName);
			var price = itemLayout.FindViewById<TextView> (Resource.Id.infoPrice);
			var rank = itemLayout.FindViewById<RatingBar> (Resource.Id.infoRank);
			foreach (var cabin in _cabins)
				if (p0.Title == cabin.Value.Name) {
					name.Text = cabin.Value.Name;
					price.Text = string.Format ("{0} {1}", cabin.Value.Price, cabin.Value.PriceType);
					rank.Rating = cabin.Value.Rating;
				}
			
			return itemLayout;
		}

		public View GetInfoWindow (Marker p0)
		{
			return null;
		}
	}
}

