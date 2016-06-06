﻿
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
using Xamarin.Auth;
using System.Json;
using System.Threading.Tasks;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Newtonsoft.Json;
using Java.Net;
using System.Net;
using FireSharp.Response;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class LoginActivity : BaseDrawerActivity,IFacebookCallback,GraphRequest.IGraphJSONObjectCallback
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
		private ICallbackManager _facebookCallBackManager;
		private Dictionary<string,UsersModel> user;
		private PushResponse response;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.log_in_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			SetTitleActionBar ("Sign In");
			FacebookSdk.SdkInitialize (ApplicationContext);
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
			PrepareFacebookLogin ();
			var signUp = FindViewById<TextView> (Resource.Id.signUpButtonDetails);
			signUp.Click += delegate {
				StartActivityForResult (typeof(SignUpActivity), 0);    
			};
			var email1 = FindViewById<EditText> (Resource.Id.login_email);
			var password = FindViewById<EditText> (Resource.Id.login_password);
			user = new Dictionary<string,UsersModel> ();
			var loginBtn = FindViewById<TextView> (Resource.Id.signInButton);
			loginBtn.Click += delegate {
				HideKeyboard (loginBtn);
				if (email1.Text == "") {
					CreateDialog (GetString (Resource.String.invalid_email), true);
				} else {
					ThreadPool.QueueUserWorkItem (o => LoginVerify ());
				}
			};

			var forgotPasswordBtn = FindViewById<TextView> (Resource.Id.forgotPassword);

//				DismissViewController (true, null);
//				if (eventArgs.IsAuthenticated) {
//					// Use eventArgs.Account to do wonderful things
//				}
//			};
//			forgotPasswordBtn.Click += delegate {
//				LoginToFacebook (true);
//			};
//			PresentViewController (auth.GetUI (), true, null);

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

		private void PrepareFacebookLogin ()
		{
			_facebookCallBackManager = CallbackManagerFactory.Create ();
			var facebookBtn = FindViewById<ImageView> (Resource.Id.facebookBtn);
			LoginManager.Instance.RegisterCallback (_facebookCallBackManager, this);

			facebookBtn.Click += delegate {
				loginType = FacebookLogin;
				LoginManager.Instance.LogInWithReadPermissions (this, new List<string> {
					"public_profile",
					"user_friends",
					"user_about_me",
					"email"
				});
			};
		}

		public void OnCancel ()
		{
			//throw new NotImplementedException();
		}

		public void OnError (FacebookException p0)
		{
			//throw new NotImplementedException();
		}

		public void OnSuccess (Java.Lang.Object p0)
		{
			var loginResult = p0 as LoginResult;
			var dada = GraphRequest.NewMeRequest (loginResult.AccessToken, this);
			Bundle parameters = new Bundle ();
			parameters.PutString ("fields", "id,name,email,picture");
			dada.Parameters = parameters;
			dada.ExecuteAsync ();
		}

		public void OnCompleted (Org.Json.JSONObject json, GraphResponse respone)
		{			
			var item = respone.JSONObject;
			UsersModel user = new UsersModel ();
			if (item != null) {
				user.Username = item.Get ("name").ToString ();
				user.Email = item.Get ("email").ToString ();
				if (item.Has ("picture")) {		
					var imageUrl = respone.JSONObject.GetJSONObject ("picture").GetJSONObject ("data").GetString ("url");
					using (var webClient = new WebClient ()) {
						var imageBytes = webClient.DownloadData (imageUrl);
						if (imageBytes != null && imageBytes.Length > 0) {
							user.ProfilePhoto = Base64.EncodeToString (imageBytes, Base64.Default);
						}
					}
				}

				//var requestText = JsonConvert.SerializeObject (user);
				//var result = Push (user);
			}
			FacebookVerify (user);
//			user.Id = result;
//			DatabaseServices.InsertUsername (user);
//			var baseserv = new BaseService<UsersModel> ();
//			var newUrl = string.Format ("users/{0}", user.Id);
//			try {
//				baseserv.UpdateUser (user, newUrl);
//
//			} catch (Exception e) {
//				var a = 0;
//			}
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
			if (resultCode == Result.Ok && loginType == FacebookLogin) {
				switch (loginType) {
				case FacebookLogin:
					CreateDialog (Resources.GetString (Resource.String.wait), false, true);
					_facebookCallBackManager.OnActivityResult (requestCode, (int)resultCode, data);
					break;		

				}
			}
			loginType = NormalLogin;
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

		private void FacebookVerify (UsersModel fbUser)
		{
			var baseService = new BaseService<Dictionary<string,UsersModel>> ();
			CreateDialog (Resources.GetString (Resource.String.wait), false, true);
			if (user.Count == 0) {	
				try {
					user = (baseService.Get ("users"));
				} catch (Exception e) {
					var a = 0;
				}
			}

			RunOnUiThread (() => {				
				var listOfUser = user.Select (kvp => kvp.Value).ToList ();
				var userulll = listOfUser.Find (p => p.Email == fbUser.Email);
				if (userulll == null) {			
						
					ThreadPool.QueueUserWorkItem (o => {
						CreateDialog (Resources.GetString (Resource.String.wait), false, true);
						//user.Id = userName.Text;
						var requestText = JsonConvert.SerializeObject (fbUser);
						var result = Push (fbUser);
						fbUser.Id = result;
						DatabaseServices.InsertUsername (fbUser);
						var baseserv = new BaseService<UsersModel> ();
						var newUrl = string.Format ("users/{0}", fbUser.Id);
						try {
							baseserv.UpdateUser (fbUser, newUrl);

						} catch (Exception e) {
							var a = 0;
						}
						StartActivity (typeof(BasicMapDemoActivity));
						Finish ();
					});					

				} else {
					ThreadPool.QueueUserWorkItem (o => {
						CreateDialog (Resources.GetString (Resource.String.wait), false, true);
						DatabaseServices.InsertUsername (userulll);
						StartActivity (typeof(BasicMapDemoActivity));
						Finish ();
					});
				}
			});
		}

		private string Push (UsersModel user)
		{
			var baseService = new BaseService<UsersModel> ();
			try {
				response = baseService.Push (user, "users");
			} catch (Exception e) {
				var a = 0;
			}
			return  response.Result.Name;
		}

		private void LoginVerify ()
		{			
			var baseService = new BaseService<Dictionary<string,UsersModel>> ();
			CreateDialog (Resources.GetString (Resource.String.wait), false, true);
			if (user.Count == 0) {	
				try {
					user = (baseService.Get ("users"));
				} catch (Exception e) {
					var a = 0;
				}
			}


			var email = FindViewById<EditText> (Resource.Id.login_email).Text;
			var password = FindViewById<EditText> (Resource.Id.login_password).Text;

			RunOnUiThread (() => {				
				var listOfUser = user.Select (kvp => kvp.Value).ToList ();

				var userulll = listOfUser.Find (p => p.Email == email);
				if (userulll != null) {
					if (password == "") {
						CreateDialog (GetString (Resource.String.invalid_password), true);
					} else {
						if (password == userulll.Password) {
							ThreadPool.QueueUserWorkItem (o => {
								UsersModel model = new UsersModel ();
								model.Email = userulll.Email;
								model.Id = userulll.Id;
								model.Password = userulll.Password;
								model.Username = userulll.Username;
								model.FavoriteList = userulll.FavoriteList;
								model.ProfilePhoto = userulll.ProfilePhoto;
								//var abc = DatabaseServices.GetAllUsers ();
								DatabaseServices.InsertUsername (model);
								StartActivity (typeof(BasicMapDemoActivity));

								Finish ();
							});
						} else {
							CreateDialog (GetString (Resource.String.invalid_password), true);
						}
					}
				}

			});
		}

		public void ShowError (string text)
		{
			Toast.MakeText (this, text, ToastLength.Short).Show ();

		}

		void LoginToFacebook (bool allowCancel)
		{		
			
			var auth = new OAuth2Authenticator (
				           clientId: "196006237460777",
				           scope: "",
				           authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				           redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, ee) => {
				if (!ee.IsAuthenticated) {
					var builder = new AlertDialog.Builder (this);
					builder.SetMessage ("Not Authenticated");
					builder.SetPositiveButton ("Ok", (o, e) => {
					});
					builder.Create ().Show ();
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me?fields=name"), null, ee.Account);
				var abc = ee.Account.Username;
				request.GetResponseAsync ().ContinueWith (t => {
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten ().InnerException.ToString ());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						var obj = JsonValue.Parse (t.Result.GetResponseText ());
						var acas = t.Result.GetResponseText ();
						builder.SetTitle ("Logged in");
						builder.SetMessage ("Name: " + obj ["name"]);
					}

					builder.SetPositiveButton ("Ok", (o, e) => {
					});
					builder.Create ().Show ();
				}, UIScheduler);
			};

			var intent = auth.GetUI (this);
			StartActivity (intent);
		}



		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext ();

	}
}
