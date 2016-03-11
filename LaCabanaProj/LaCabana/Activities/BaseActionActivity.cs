
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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			ActionBar.SetCustomView (Resource.Layout.action_bar_home);
			ActionBar.SetDisplayShowHomeEnabled (false);
			ActionBar.SetDisplayHomeAsUpEnabled (false);
			ActionBar.SetDisplayShowTitleEnabled (false);
			ActionBar.SetDisplayShowCustomEnabled (true);
			FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn).Visibility = ViewStates.Visible;

//			var backButton = FindViewById<Button> (Resource.Id.action_bar_menu);
//
//			backButton.Click += delegate {
//				OnBackPressed ();
//			};



			// DatabaseServices = new DatabaseServices(this);

			MenuButton = FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn);
			AddButton = FindViewById<ImageButton> (Resource.Id.action_bar_addBtn);
			DeleteButton = FindViewById<ImageButton> (Resource.Id.action_bar_deleteBtn);
			SearchButton = FindViewById<ImageButton> (Resource.Id.action_bar_searchBtn);
			EditButton = FindViewById<ImageButton> (Resource.Id.action_bar_editBtn);

		}

		public void SetTitleActionBar (string title)
		{
			FindViewById<TextView> (Resource.Id.action_bar_menu).Text = title;
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
			FindViewById<Button> (Resource.Id.action_bar_menu).Click += OpenMenu;
			//FindViewById<ImageButton> (Resource.Id.drawer_back).Click += CloseMenu;
			//drawerLayout.DrawerSlide += DrawerLayoutDrawerSlide;
//			var userName = ActiveUser;
//			FindViewById<TextView> (Resource.Id.drawer_profile_name).Text = userName;
		}

		//		private void DrawerLayoutDrawerSlide (object sender, DrawerLayout.DrawerSlideEventArgs e)
		//		{
		//			if (e.SlideOffset > 0.1) {
		//				if (_shouldGoInvisible) {
		//					_shouldGoInvisible = false;
		//					FindViewById<Button> (Resource.Id.action_bar_menu).Visibility = ViewStates.Gone;
		////					FindViewById<LinearLayout> (Resource.Id.drawer_profile_button).Visibility = ViewStates.Visible;
		////					FindViewById<ImageView> (Resource.Id.drawer_back).Visibility = ViewStates.Visible;
		//					SupportInvalidateOptionsMenu ();
		//				}
		//			} else {
		//				if (!_shouldGoInvisible) {
		//					_shouldGoInvisible = true;
		//					FindViewById<Button> (Resource.Id.action_bar_menu).Visibility = ViewStates.Visible;
		////					FindViewById<LinearLayout> (Resource.Id.drawer_profile_button).Visibility = ViewStates.Gone;
		////					FindViewById<ImageView> (Resource.Id.drawer_back).Visibility = ViewStates.Gone;
		////					FindViewById<LinearLayout> (Resource.Id.accountsLayout).Visibility = ViewStates.Gone;
		//					SupportInvalidateOptionsMenu ();
		//				}
		//			}
		//		}




	}
}


		
