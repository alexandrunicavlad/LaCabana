using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Content;
using Android.App;

namespace LaCabana
{
	public class InfoWindowAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
	{
		private Marker _marker;
		private Activity _context;

		public InfoWindowAdapter (Marker marker, Activity context)
		{
			_marker = marker;
			_context = context;
		}

		public View GetInfoContents (Marker p0)
		{
			LayoutInflater inflater;
			inflater = (LayoutInflater)_context.GetSystemService (Context.LayoutInflaterService);
			var itemLayout = inflater.Inflate (Resource.Layout.infoWindow_layout, null);
			LatLng latLng = _marker.Position;
			return itemLayout;
		}

		public View GetInfoWindow (Marker p0)
		{
			return null;
		}
	}
}

