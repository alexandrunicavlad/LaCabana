using System;
using Android.Content;
using Android.Database;
using Android.Widget;

namespace LaCabana
{
	public class SearchItemAdapter : Android.Widget.SimpleCursorAdapter
	{

		int v;

		public SearchItemAdapter(Context context, int layout, ICursor c, string[] from, int[] to, int v) : base(context, layout, c, from, to)
		{
			this.v = v;
		}


		public override void BindView(Android.Views.View view, Context context, ICursor cursor)
		{
			var textItem = view.FindViewById<TextView>(Resource.Id.textItem);
			textItem.Text = (cursor.GetString(2));
			//base.BindView(view, context, cursor);
		}
	}
}

