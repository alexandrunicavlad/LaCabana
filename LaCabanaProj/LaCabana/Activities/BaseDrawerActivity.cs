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
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Support.V7.App;


namespace LaCabana
{
	[Activity (Label = "BaseDrawerActivity", Theme = "@style/MyTheme")]			
	public class BaseDrawerActivity : BaseActivity
	{
		protected DrawerLayout drawerLayout;
		protected NavigationView navigationView;
		protected RelativeLayout loading;
		private bool _shouldGoInvisible = true;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SupportActionBar.SetCustomView (Resource.Layout.action_bar_back);
			SupportActionBar.SetDisplayShowHomeEnabled (false);
			SupportActionBar.SetDisplayHomeAsUpEnabled (false);
			SupportActionBar.SetDisplayShowTitleEnabled (false);
			SupportActionBar.SetDisplayShowCustomEnabled (true);

			FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn).Visibility = ViewStates.Visible;

			var backButton = FindViewById<Button> (Resource.Id.action_bar_back);

			backButton.Click += delegate {
				//OnBackPressed ();
			};

			// DatabaseServices = new DatabaseServices(this);

			MenuButton = FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn);
			AddButton = FindViewById<ImageButton> (Resource.Id.action_bar_addBtn);
			DeleteButton = FindViewById<ImageButton> (Resource.Id.action_bar_deleteBtn);
			SearchButton = FindViewById<ImageButton> (Resource.Id.action_bar_searchBtn);
			EditButton = FindViewById<ImageButton> (Resource.Id.action_bar_editBtn);

		}

		public void SetTitleActionBar (string title)
		{
			FindViewById<TextView> (Resource.Id.action_bar_back).Text = title;
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
			FindViewById<Button> (Resource.Id.action_bar_back).Click += OpenMenu;
			//FindViewById<ImageButton> (Resource.Id.drawer_back).Click += CloseMenu;
			//drawerLayout.DrawerSlide += DrawerLayoutDrawerSlide;
			//			var userName = ActiveUser;
			//			FindViewById<TextView> (Resource.Id.drawer_profile_name).Text = userName;
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


	}
}

