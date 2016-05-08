
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
using Android.Views.InputMethods;
using LaCabana.Services;
using Android.Gms.Maps.Model;

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
		protected static IDatabaseServices DatabaseServices;
		public UsersModel user;

		public LatLng myLocation{ get; set; }

		public UsersModel userMod{ get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			DatabaseServices = new DatabaseServices (this);
//			var view = LayoutInflater.Inflate (Resource.Layout.action_bar_home, null);
//			Android.App.ActionBar.LayoutParams layoutParams = new Android.App.ActionBar.LayoutParams (Android.App.ActionBar.LayoutParams.MatchParent,
//				                                                  Android.App.ActionBar.LayoutParams.MatchParent);
//			ActionBar.SetCustomView (view, layoutParams);
			ActionBar.SetDisplayShowHomeEnabled (false);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetDisplayShowTitleEnabled (true);
			ActionBar.SetDisplayShowCustomEnabled (false);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (Resources.GetColor (Resource.Color.main_blue_green)));
			ActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_menu);

		}

		public void MyPosition (LatLng location)
		{
			myLocation = location;
		}

		public void UserData (UsersModel userul)
		{
			userMod = userul;
		}

		public void SetProfilePicture ()
		{
			user = DatabaseServices.GetAllUsers ();
			UserData (user);
			var imagecenterLayout = FindViewById<RelativeLayout> (Resource.Id.profile_layout);
			var imagecenter = imagecenterLayout.FindViewById<ImageView> (Resource.Id.account_info_profile_image);
			if (user.ProfilePhoto == null) {
				imagecenter.SetImageDrawable (new RoundedImage (BitmapFactory.DecodeResource (Resources, Resource.Drawable.avatarplaceholder), this, ""));
			} else {
				var picture = Decode (user.ProfilePhoto);
				imagecenter.SetImageDrawable (new RoundedImage (picture, this, ""));
			}
			var accountMail = imagecenterLayout.FindViewById<TextView> (Resource.Id.emailText);
			if (user.Email == null) {
				accountMail.Text = "enter mail";
			} else {
				accountMail.Text = user.Email;
			}
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
			var addNewLocationButton = FindViewById<LinearLayout> (Resource.Id.addnewlocationItem);
			addNewLocationButton.Click += delegate {
				addNewLocationButton.Clickable = false;
				if (this is AddNewLocation) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {					
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
					StartActivityForResult (typeof(AddNewLocation), 2);
				}
				addNewLocationButton.Clickable = true;
			};
			var cabinsNear = FindViewById<LinearLayout> (Resource.Id.cabinesItem);
			cabinsNear.Click += delegate {
				cabinsNear.Clickable = false;
				if (this is CabinsNear) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {							
					
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
					AlertDialog.Builder alert = new AlertDialog.Builder (this);
					alert.SetTitle ("Select range");
					var infate = LayoutInflater.Inflate (Resource.Layout.slider_range, null);
					var seekBar = infate.FindViewById<SeekBar> (Resource.Id.edit_seekBar);
					var text = infate.FindViewById<TextView> (Resource.Id.seekbar_text);
					alert.SetView (infate);
					seekBar.Progress = 10;
					text.Text = seekBar.Progress.ToString () + "km";
					seekBar.ProgressChanged += delegate (object sender, SeekBar.ProgressChangedEventArgs args) {
						if (!args.FromUser)
							return;
						text.Text = (seekBar.Progress).ToString () + "km";
					};
					alert.SetPositiveButton ("Done", delegate {		
						var intent = new Intent (this, typeof(CabinsNear));
						intent.PutExtra ("latitude", myLocation.Latitude);
						intent.PutExtra ("longitude", myLocation.Longitude);
						intent.PutExtra ("distance", seekBar.Progress);
						if (user.FavoriteList != null) {
							var favlist = user.FavoriteList.Values.ToList ();
							intent.PutStringArrayListExtra ("favoriteList", favlist);
						
						}
						StartActivity (intent);
					});
					alert.SetNegativeButton ("Cancel", delegate {

					});
					alert.Show ();
					
				}
					

				cabinsNear.Clickable = true;
				
			};
			var favorites = FindViewById<LinearLayout> (Resource.Id.favoritesItem);
			favorites.Click += delegate {
				favorites.Clickable = false;
				if (this is FavoritesActivity) {
					drawerLayout.CloseDrawer ((int)GravityFlags.Start);
				} else {	
					if (user.Id != null) {
//						var urlUpdate = string.Format ("users/{0}", user.Id);
//						var baseService = new BaseService<UsersModel> ();
//						var favId = (baseService.Get (urlUpdate));
						if (user.FavoriteList == null) {
							Toast.MakeText (this, "Nu aveti nicio cabana favorita", ToastLength.Short).Show ();
							drawerLayout.CloseDrawer ((int)GravityFlags.Start);
						} else {	
							drawerLayout.CloseDrawer ((int)GravityFlags.Start);
							var intent = new Intent (this, typeof(FavoritesActivity));
							var favlist = user.FavoriteList.Values.ToList ();
							intent.PutExtra ("latitude", myLocation.Latitude);
							intent.PutExtra ("longitude", myLocation.Longitude);
							intent.PutStringArrayListExtra ("favoriteList", favlist);
							StartActivity (intent);
						}
					} else {
						Toast.MakeText (this, "Please login for select this page", ToastLength.Short).Show ();
						drawerLayout.CloseDrawer ((int)GravityFlags.Start);
					}
				}
				favorites.Clickable = true;
			};
			var logOut = FindViewById<LinearLayout> (Resource.Id.logoutItem);
			logOut.Click += LogOut;

		}

		public void SetTitleActionBar (string title)
		{
			ActionBar.Title = title;
		}

		protected void OpenMenu (object sender, EventArgs e)
		{
			drawerLayout.OpenDrawer ((int)GravityFlags.Start);
			ActionBar.Hide ();
		}

		protected void CloseMenu (object sender, EventArgs e)
		{
			drawerLayout.CloseDrawer ((int)GravityFlags.Start);
			ActionBar.Show ();
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

		protected void HideKeyboard (Activity activity)
		{
			var inputMethodManager = (InputMethodManager)GetSystemService ((Context.InputMethodService));
			if (activity != null && activity.CurrentFocus != null) {
				inputMethodManager.HideSoftInputFromWindow (activity.CurrentFocus.WindowToken, 0);
			}
		}

		private void LogOut (object sender, EventArgs e)
		{
			var builder = new AlertDialog.Builder (this).SetTitle ("Log out").SetMessage ("Are you sure you want to log out?").SetPositiveButton ("Ok", (EventHandler<DialogClickEventArgs>)null).SetNegativeButton ("Cancel", (EventHandler<DialogClickEventArgs>)null);
			var dialog = builder.Create ();
			dialog.Show ();
			dialog.CancelEvent += delegate {
				dialog.Cancel ();
			};
			var okBtn = dialog.GetButton ((int)DialogButtonType.Positive);
			okBtn.Click += delegate {
				DatabaseServices.DeleteUser ();
				StartActivityForResult (typeof(BasicMapDemoActivity), 2);
				Finish ();
				dialog.Cancel ();
			};
			var cancelBtn = dialog.GetButton ((int)DialogButtonType.Negative);
			cancelBtn.Click += delegate {
				dialog.Cancel ();	
			};
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == 1) && (resultCode == Result.Ok)) {	
				var a = 2;
			}
		}

	}
}


		
