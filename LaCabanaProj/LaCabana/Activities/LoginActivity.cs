
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
using System.Threading;
using Android.Support.V4.Widget;
using LaCabana.Services;
using Android.Provider;
using System.IO;
using Android.Graphics;
using Android.Util;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class LoginActivity : BaseDrawerActivity
	{
		private const int NormalLogin = 0;
		private const int FacebookLogin = 1;
		private const int GoogleLogin = 2;
		private int loginType;
		private bool myaccountBool = false;
		private RelativeLayout myAccountlayout;
		private RelativeLayout loginLayout;
		private ImageView addPhoto;
		private Button uploadButton;
		private string imageFile;
		private bool emailFail = false;
		private bool passwordFail = false;
		private EditText email;
		private EditText account;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.log_in_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Sign In");

			myAccountlayout = FindViewById<RelativeLayout> (Resource.Id.my_account_layout);
			email = myAccountlayout.FindViewById<EditText> (Resource.Id.emailEditText);
			account = myAccountlayout.FindViewById<EditText> (Resource.Id.accountText);
			loginLayout = FindViewById<RelativeLayout> (Resource.Id.FlyContent);
			addPhoto = myAccountlayout.FindViewById<ImageView> (Resource.Id.addPhoto);
			uploadButton = myAccountlayout.FindViewById<Button> (Resource.Id.uploadButton);
			DatabaseServices = new DatabaseServices (this);
			var allUsers = DatabaseServices.GetAllUsers ();
			if (allUsers.Email != null) {
				loginLayout.Visibility = ViewStates.Gone;
				myAccountlayout.Visibility = ViewStates.Visible;
				myaccountBool = true;
			}
			var signUp = FindViewById<TextView> (Resource.Id.signUpButtonDetails);
			signUp.Click += delegate {
				StartActivityForResult (typeof(SignUpActivity), 0);    
			};

			var loginBtn = FindViewById<TextView> (Resource.Id.signInButton);
			loginBtn.Click += delegate {
				HideKeyboard (loginBtn);
				ThreadPool.QueueUserWorkItem (o => LoginVerify ());
			};

			var forgotPasswordBtn = FindViewById<TextView> (Resource.Id.forgotPassword);

			forgotPasswordBtn.Click += delegate {
				//StartActivity (typeof(RecoverPasswordActivity));  
			};


			if (myaccountBool) {
				uploadButton.Click += PictureChangeClick;

				email.Text = allUsers.Email;
				account.Text = allUsers.Username;
				addPhoto.SetImageBitmap (Decode (allUsers.ProfilePhoto));
				myAccountlayout.FindViewById<Button> (Resource.Id.add_button_location).Click += delegate {
					ThreadPool.QueueUserWorkItem (o => SaveChanges (allUsers));
				};
			}

		}

		public void SaveChanges (UsersModel user)
		{
			CreateDialog (Resources.GetString (Resource.String.wait), false, true);
			UsersModel model = new UsersModel ();
			model.Email = email.Text;
			model.Id = user.Id;
			model.Password = user.Password;
			model.Username = account.Text;
			model.FavoriteList = user.FavoriteList;
			if (imageFile != null) {
				model.ProfilePhoto = imageFile;
			} else {
				model.ProfilePhoto = user.ProfilePhoto;
			}
			DatabaseServices.DeleteUser ();
			DatabaseServices.InsertUsername (model);
			var baseserv = new BaseService<UsersModel> ();
			var newUrl = string.Format ("users/{0}", user.Id);
			try {
				baseserv.UpdateUser (model, newUrl);
			} catch (Exception e) {
				var a = 0;
			}
			Finish ();

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

		private void PictureChangeClick (object sender, EventArgs e)
		{			
			var intent = new Intent ();
			intent.SetType ("image/*");
			intent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (intent, "Select Picture"), 0);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null)) {
				Android.Net.Uri uri = data.Data;
				addPhoto.SetImageURI (uri);
				addPhoto.SetScaleType (ImageView.ScaleType.CenterCrop);
				var bitmap = MediaStore.Images.Media.GetBitmap (this.ContentResolver, uri);
				MemoryStream stream = new MemoryStream ();
				var resizebit = Bitmap.CreateScaledBitmap (bitmap, 100, 100, false);
				resizebit.Compress (Bitmap.CompressFormat.Png, 100, stream);
				resizebit.Recycle ();
				byte[] byteArray = stream.ToArray ();
				imageFile = Base64.EncodeToString (byteArray, Base64.Default);
			}
		}

		private void LoginVerify ()
		{
			var baseService = new BaseService<Dictionary<string,UsersModel>> ();
			CreateDialog (Resources.GetString (Resource.String.wait), false, true);

			var user = new Dictionary<string,UsersModel> ();
			try {
				user = (baseService.Get ("users"));
			} catch (Exception e) {
				var a = 0;
			}
			var email = FindViewById<EditText> (Resource.Id.login_email).Text;
			var password = FindViewById<EditText> (Resource.Id.login_password).Text;


			RunOnUiThread (() => {
				

				foreach (var item in user) {
					if (email == "" || email != item.Value.Email) {
						emailFail = true;
					} else if (password == "" || password != item.Value.Password) {
						passwordFail = true;
					} else {
						passwordFail = false;
						emailFail = false;
						ThreadPool.QueueUserWorkItem (o => {
							UsersModel model = new UsersModel ();
							model.Email = item.Value.Email;
							model.Id = item.Key;
							model.Password = item.Value.Password;
							model.Username = item.Value.Username;
							model.FavoriteList = item.Value.FavoriteList;
							model.ProfilePhoto = item.Value.ProfilePhoto;
							//var abc = DatabaseServices.GetAllUsers ();
							DatabaseServices.InsertUsername (model);
							StartActivity (typeof(BasicMapDemoActivity));

							Finish ();
						});
					}
				}
				if (emailFail) {
					CreateDialog (GetString (Resource.String.invalid_email), true);
				} else if (passwordFail) {
					CreateDialog (GetString (Resource.String.invalid_password), true);
				}

			});
		}

		public void ShowError (string text)
		{
			Toast.MakeText (this, text, ToastLength.Short).Show ();

		}


	}
}
