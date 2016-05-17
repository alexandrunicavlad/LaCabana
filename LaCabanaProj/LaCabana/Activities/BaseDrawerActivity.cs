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
using Android.Support.V7.Widget;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using LaCabana.Services;
using Android.Graphics.Drawables;


namespace LaCabana
{
	[Activity (Label = "BaseDrawerActivity", Theme = "@style/MyTheme")]			
	public class BaseDrawerActivity : BaseActivity
	{
		protected DrawerLayout drawerLayout;
		protected RelativeLayout loading;
		private bool _shouldGoInvisible = true;
		private BitmapFactory.Options _placeHolderOptions;
		protected static IDatabaseServices DatabaseServices;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SupportActionBar.SetDisplayShowHomeEnabled (false);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			SupportActionBar.SetDisplayShowCustomEnabled (false);
			SupportActionBar.SetBackgroundDrawable (new ColorDrawable (Resources.GetColor (Resource.Color.main_blue_green)));
			SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_keyboard_backspace);

//			MenuButton = FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn);
//			AddButton = FindViewById<ImageButton> (Resource.Id.action_bar_addBtn);
//			DeleteButton = FindViewById<ImageButton> (Resource.Id.action_bar_deleteBtn);
//			SearchButton = FindViewById<ImageButton> (Resource.Id.action_bar_searchBtn);
//			EditButton = FindViewById<ImageButton> (Resource.Id.action_bar_editBtn);

		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home) {	
				OnBackPressed ();
			}
			return base.OnOptionsItemSelected (item);
		}

		public void SetTitleActionBar (string title)
		{
			SupportActionBar.Title = title;
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
			//FindViewById<Button> (Resource.Id.action_bar_back).Click += OpenMenu;
			//FindViewById<ImageButton> (Resource.Id.drawer_back).Click += CloseMenu;
			//drawerLayout.DrawerSlide += DrawerLayoutDrawerSlide;
			//			var userName = ActiveUser;
			//			FindViewById<TextView> (Resource.Id.drawer_profile_name).Text = userName;
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

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			HideMenuItems (menu, _shouldGoInvisible);
			return base.OnPrepareOptionsMenu (menu);
		}

		private void HideMenuItems (IMenu menu, bool visible)
		{
			for (var i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (visible);
		}

		public void SetProfilePicture ()
		{
			var imagecenterLayout = FindViewById<RelativeLayout> (Resource.Id.profile_layout);
			var imagecenter = imagecenterLayout.FindViewById<ImageView> (Resource.Id.account_info_profile_image);
			imagecenter.SetImageDrawable (new RoundedImage (BitmapFactory.DecodeResource (Resources, Resource.Drawable.avatarplaceholder), this, ""));
		}

	}
}

 