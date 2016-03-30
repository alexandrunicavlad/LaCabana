using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace LaCabana.Helpers
{
	public class FontArrayAdapter<T> : ArrayAdapter<String>
	{
		private Context _context;

		public FontArrayAdapter (IntPtr handle, JniHandleOwnership transfer) : base (handle, transfer)
		{
		}

		public FontArrayAdapter (Context context, int textViewResourceId) : base (context, textViewResourceId)
		{			
			_context = context;
		}

		public FontArrayAdapter (Context context, int resource, int textViewResourceId)
			: base (context, resource, textViewResourceId)
		{			
			_context = context;
		}

		public FontArrayAdapter (Context context, int textViewResourceId, string[] objects)
			: base (context, textViewResourceId, objects)
		{			
			_context = context;
		}

		public FontArrayAdapter (Context context, int resource, int textViewResourceId, string[] objects)
			: base (context, resource, textViewResourceId, objects)
		{			
			_context = context;
		}

		public FontArrayAdapter (Context context, int textViewResourceId, IList<string> objects)
			: base (context, textViewResourceId, objects)
		{			
			_context = context;
		}

		public FontArrayAdapter (Context context, int resource, int textViewResourceId, IList<string> objects)
			: base (context, resource, textViewResourceId, objects)
		{			
			_context = context;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = base.GetView (position, convertView, parent);
			view.SetBackgroundColor (_context.Resources.GetColor (Resource.Color.white));
			((TextView)view).SetTextSize (ComplexUnitType.Sp, 16);
			((TextView)view).SetTextColor (_context.Resources.GetColor (Resource.Color.gray));
			return view;
		}

		public override View GetDropDownView (int position, View convertView, ViewGroup parent)
		{
			var view = base.GetDropDownView (position, convertView, parent);
		
//			((TextView)view).SetTextSize (ComplexUnitType.Sp, 16);
//			((TextView)view).SetTextColor (_context.Resources.GetColor (Resource.Color.gray));
			/*if(position==base.Count)
            {
                ((TextView) view).Text = "";
                ((TextView) view).Hint = GetItem(base.Count);
                ((TextView) view).SetHintTextColor(
                    _context.Resources.GetColorStateList(Android.Resource.Color.SecondaryTextDark));
            }*/
			return view;
		}

		/*public override int Count
        {
            get
            {
                return base.Count-1;
            }
        }*/
	}
}