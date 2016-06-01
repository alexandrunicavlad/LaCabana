using System;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using Android.App;

namespace LaCabana
{
	public class PictureAdapter : BaseAdapter
	{
		Context context;
		List<Bitmap> allCabins;


		public PictureAdapter (Context c, List<Bitmap> cabins)
		{
			context = c;
			allCabins = cabins;
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
			ImageView imageView;

			if (convertView == null) {  // if it's not recycled, initialize some attributes
				imageView = new ImageView (context);

				imageView.LayoutParameters = new GridView.LayoutParams (context.Resources.DisplayMetrics.WidthPixels / 3, context.Resources.DisplayMetrics.HeightPixels / 3);

				imageView.SetScaleType (ImageView.ScaleType.FitXy);
			} else {
				imageView = (ImageView)convertView;
			}				

			imageView.SetImageBitmap (allCabins [position]);
			return imageView;
		}

	}
}
