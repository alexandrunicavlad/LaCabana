
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
using Android.Provider;
using Java.IO;
using System.IO;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace LaCabana
{
	
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class PicturesActivity :  BaseDrawerActivity
	{
		private Dictionary<string, string> pictures;
		private LinearLayout reviewLayout;
		private BaseService<Dictionary<string, string>> baseService;
		private BaseService< byte[]> baseService1;
		private RelativeLayout loading;
		private ScrollView scrollView;
		private String marker;
		//private Dictionary<string, ReviewModel> reviews;
		private TextView empty;
		private const int REQUEST_IMAGE_ALBUM = 2;
		private GridView gridview;
		private List<Bitmap> picts;
		private TextView numberPhoto;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.pictures_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			ConstructRightIcon ();
			SetTitleActionBar1 ("Pictures");
			marker = Intent.GetStringExtra ("marker");
			picts = new List<Bitmap> ();
			baseService = new BaseService<Dictionary<string, string>> ();
			baseService1 = new BaseService<byte[]> ();
			pictures = new Dictionary<string, string> ();
			reviewLayout = FindViewById<LinearLayout> (Resource.Id.FlyContent);
			empty = FindViewById<TextView> (Resource.Id.emptyReview);
			gridview = FindViewById<GridView> (Resource.Id.gridview);
			numberPhoto = FindViewById<TextView> (Resource.Id.numberPhoto);
			var route = new RouteGenerator ();
			loading = FindViewById<RelativeLayout> (Resource.Id.main_loading);
			loading.Visibility = ViewStates.Visible;
			var addButton = FindViewById<ImageView> (Resource.Id.addReview);
			//reviews = new Dictionary<string, ReviewModel> ();
			if (marker != null) {
				ThreadPool.QueueUserWorkItem (o => GetData (marker));	
			}
			addButton.Click += delegate {
				Intent intent = new Intent (Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
				StartActivityForResult (intent, REQUEST_IMAGE_ALBUM);
//				var intent = new Intent (this, typeof(ReviewAddActivity));	
//				intent.PutExtra ("marker", marker);
//				StartActivity (intent);
//				Finish ();
			};
			gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {
//				AlertDialog.Builder builder = new AlertDialog.Builder (this);
//				AlertDialog dialog = builder.Create ();
				Dialog buulde = new Dialog (this);
				buulde.Window.RequestFeature (WindowFeatures.NoTitle);
				ImageView imageView = new ImageView (this);			
				imageView.SetImageBitmap (picts [args.Position]);
				buulde.AddContentView (imageView, new RelativeLayout.LayoutParams (
					ViewGroup.LayoutParams.MatchParent, 
					ViewGroup.LayoutParams.MatchParent));
				imageView.Click += delegate {
					buulde.Dismiss ();	
				};
				buulde.Show ();

			};

		}

		public void GetData (string marker)
		{	
			try {
				pictures = (baseService.Get (string.Format ("cabins/{0}/Pictures", marker)));	
				foreach (var rev in pictures) {
					var abc = baseService1.Get (string.Format ("pictures/{0}", rev.Value));
					Bitmap bitmap = BitmapFactory.DecodeByteArray (abc, 0, abc.Length);
					picts.Add (bitmap);
				}
			} catch (Exception e) {
				var a = 0;
			}
			RunOnUiThread (() => {
				gridview.Adapter = new PictureAdapter (this, picts);
				loading.Visibility = ViewStates.Gone;
				numberPhoto.Visibility = ViewStates.Visible;
				numberPhoto.Text = string.Format ("({0}) Photos", picts.Count);
			});
		}

		public void ConstructRightIcon ()
		{
			var actionBar = SupportActionBar;

			actionBar.SetDisplayShowCustomEnabled (true);
			LayoutInflater inflate = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);
			View view = inflate.Inflate (Resource.Layout.action_bar_home, null);
			actionBar.SetCustomView (Resource.Layout.action_bar_home);

			view.Click += delegate {
				var a = 0;
			};
			var MenuButton = SupportActionBar.CustomView.FindViewById<ImageButton> (Resource.Id.action_bar_menuBtn);

//			MenuButton.Visibility = ViewStates.Visible;
//			MenuButton.Click += (object sender, EventArgs e) => {
//				PopupMenu menu = new PopupMenu (this, MenuButton);
//				menu.Inflate (Resource.Menu.review_menu);
//				menu.MenuItemClick += (object sender1, PopupMenu.MenuItemClickEventArgs e1) => {
//					var a = e1.Item.TitleFormatted.ToString ();
//					if (a.Equals ("Newest")) {							
//						FillLayout (reviews.Reverse ().ToDictionary (kvp => kvp.Key, kvp => kvp.Value));
//					} else if (a.Equals ("Best")) {
//						var ordbest = reviews.OrderBy (item => item.Value.Rating);
//						FillLayout (ordbest.Reverse ().ToDictionary (kvp => kvp.Key, kvp => kvp.Value));
//					}
//				};
//
//				menu.DismissEvent += delegate {					
//				};
//				menu.Show ();
//			};

		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == REQUEST_IMAGE_ALBUM) && (resultCode == Result.Ok) && (data != null)) {
				var selectedImage = data.Data;
				try {	
					var pathul = selectedImage.ToString ();

					var bitmap = MediaStore.Images.Media.GetBitmap (this.ContentResolver, selectedImage);
					var neda = Bitmap.CreateScaledBitmap (bitmap, 600, 800, false);
					bitmap.Recycle ();
					var bos = new MemoryStream ();
					neda.Compress (Android.Graphics.Bitmap.CompressFormat.Jpeg, 70, bos);
					var asd = bos.ToArray ();
					var baseService = new BaseService<byte[]> ();
					var baseService1 = new BaseService<UsersModel> ();
					try {
						var result = baseService.Push (asd, "pictures");
						var urlUpdate = string.Format ("cabins/{0}/Pictures", marker);
						baseService.Update (result.Result.Name, urlUpdate);

					} catch (Exception e) {
						var a = 0;
					}
				} catch (Exception) {

					//var toast = Toast.MakeText (this, GetString (Resource.String.Failedtoload), ToastLength.Short);
					//toast.Show ();
				}
			}
		}


	}
}

