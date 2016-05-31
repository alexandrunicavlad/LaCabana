using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Threading;
using LaCabana.Services;

namespace LaCabana
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class ReviewAddActivity :  BaseDrawerActivity
	{
		private IDatabaseServices DatabaseServices;
		private string marker;
		private EditText account;
		private EditText reviewText;
		private EditText title;
		private RatingBar rating;
		private UsersModel user;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.review_add_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			ConstructRightIcon ();
			DatabaseServices = new DatabaseServices (this);
			user = DatabaseServices.GetAllUsers ();
			SetTitleActionBar1 ("Add Review");
			marker = Intent.GetStringExtra ("marker");
			var saveButton = FindViewById<Button> (Resource.Id.add_button_location);
			account = FindViewById<EditText> (Resource.Id.accountText);
			account.Text = user.Username;
			title = FindViewById<EditText> (Resource.Id.TitleText);
			rating = FindViewById<RatingBar> (Resource.Id.cabinRating);
			reviewText = FindViewById<EditText> (Resource.Id.reviewText);
			saveButton.Click += delegate {
				saveButton.Click += delegate {
					ThreadPool.QueueUserWorkItem (o => SaveReview ());
				};
			};

		}

		private void SaveReview ()
		{			
			if (account.Text == "") {
				CreateDialog ("Please enter username", true);
				return;
			} else if (title.Text == "") {
				CreateDialog ("Please enter title", true);
				return;
			} else if (rating.Progress == 0) {
				CreateDialog ("Please enter rating", true);
				return;
			} else if (reviewText.Text == "") {
				CreateDialog ("Please enter review", true);
				return;
			}
			var review = new ReviewModel ();
			CreateDialog ("", GetString (Resource.String.wait), false, "", false, "", true);
			review.UserNameAdded = account.Text;
			review.Title = title.Text;
			review.DateAdd = DateTime.Now.ToString ("dd/MM/yyyy");
			review.Rating = rating.Progress;
			review.ReviewText = reviewText.Text;
			if (user.ProfilePhoto != null)
				review.UserPhoto = user.ProfilePhoto;
			var baseService = new BaseService<ReviewModel> ();
			var baseService1 = new BaseService<UsersModel> ();
			try {
				var result = baseService.Push (review, "reviews");
				var urlUpdate = string.Format ("cabins/{0}/Reviews", marker);
				baseService.Update (result.Result.Name, urlUpdate);
				var intent = new Intent (this, typeof(ReviewActivity));				
				intent.PutExtra ("marker", marker);
				StartActivity (intent);
				Finish ();

			} catch (Exception e) {
				var a = 0;
			}
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

			MenuButton.Visibility = ViewStates.Visible;
			MenuButton.Click += (object sender, EventArgs e) => {
				PopupMenu menu = new PopupMenu (this, MenuButton);
				menu.Inflate (Resource.Menu.cabin_menu);
				menu.MenuItemClick += (object sender1, PopupMenu.MenuItemClickEventArgs e1) => {
					var a = e1.Item.TitleFormatted.ToString ();
					if (a.Equals ("Pictures")) {
					} else if (e.Equals ("Reviews")) {

					}
				};
				menu.DismissEvent += delegate {					
				};
				menu.Show ();
			};

		}
	}
}

