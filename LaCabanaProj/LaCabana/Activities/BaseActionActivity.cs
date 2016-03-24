
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
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace LaCabana
{
	[Activity (Label = "BaseActionActivity")]			
	public class BaseActionActivity : FragmentActivity
	{
		protected ImageButton MenuButton;
		protected ImageButton AddButton;
		protected ImageButton DeleteButton;
		protected ImageButton SearchButton;
		protected ImageButton EditButton;
		protected DrawerLayout drawerLayout;
		protected NavigationView navigationView;
		protected RelativeLayout loading;
		private bool _shouldGoInvisible = true;
		private BitmapFactory.Options _placeHolderOptions;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

//			var view = LayoutInflater.Inflate (Resource.Layout.action_bar_home, null);
//			Android.App.ActionBar.LayoutParams layoutParams = new Android.App.ActionBar.LayoutParams (Android.App.ActionBar.LayoutParams.MatchParent,
//				                                                  Android.App.ActionBar.LayoutParams.MatchParent);
//			ActionBar.SetCustomView (view, layoutParams);
			ActionBar.SetDisplayShowHomeEnabled (false);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayShowCustomEnabled (false);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (Resources.GetColor (Resource.Color.yellow)));
			ActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_sandwich);
		}

		public void SetProfilePicture ()
		{
			var imagecenterLayout = FindViewById<RelativeLayout> (Resource.Id.profile_layout);
			var imagecenter = imagecenterLayout.FindViewById<ImageView> (Resource.Id.account_info_profile_image);
			imagecenter.SetImageDrawable (new RoundedImage (BitmapFactory.DecodeResource (Resources, Resource.Drawable.avatarplaceholder), this, ""));
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home) {	
				if (drawerLayout.IsDrawerOpen ((int)GravityFlags.Start)) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {
					drawerLayout.OpenDrawer ((int)GravityFlags.Start);
				}
			}
			return base.OnOptionsItemSelected (item);
		}

		protected void ClickHandler ()
		{		

			var mapButton = FindViewById<LinearLayout> (Resource.Id.map_item);
			mapButton.Click += delegate {
				mapButton.Clickable = false;

				if (this is BasicMapDemoActivity) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {					
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
					StartActivityForResult (typeof(BasicMapDemoActivity), 2);
				}

				mapButton.Clickable = true;
			};

			var myAccountButton = FindViewById<LinearLayout> (Resource.Id.myaccountItem);
			myAccountButton.Click += delegate {
				myAccountButton.Clickable = false;
				if (this is LoginActivity) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {					
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
					StartActivityForResult (typeof(LoginActivity), 2);
				}
				myAccountButton.Clickable = true;
			};
		}

		public void SetTitleActionBar (string title)
		{
			ActionBar.Title = title;
		}

		protected void OpenMenu (object sender, EventArgs e)
		{
			drawerLayout.OpenDrawer ((int)GravityFlags.Start);
		}

		protected void CloseMenu (object sender, EventArgs e)
		{
			drawerLayout.CloseDrawer ((int)GravityFlags.Start);
		}

		protected void SetupDrawer (DrawerLayout drawer)
		{
			drawerLayout = drawer;

		}

		public void PictureProfile ()
		{
			_placeHolderOptions = new BitmapFactory.Options ();
			_placeHolderOptions.InSampleSize = 3;
			_placeHolderOptions.InDither = false;
			_placeHolderOptions.InPurgeable = true;

			try {
				var pic = BitmapFactory.DecodeResource (Resources, Resource.Drawable.avatarplaceholder, _placeHolderOptions);
				FindViewById<ImageView> (Resource.Id.account_info_profile_image).SetImageDrawable (new RoundedImage (pic, this));
				_placeHolderOptions.InSampleSize = 3;

			} catch (Exception e) {
				//HandleErrors (e);
			}
		}

	}
}


		
