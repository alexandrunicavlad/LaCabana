
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
using System.Threading;
using Android.Graphics;
using System.Globalization;

namespace LaCabana
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
	public class ReviewActivity : BaseDrawerActivity
	{
		private Dictionary<string, string> review;
		private LinearLayout reviewLayout;
		private BaseService<Dictionary<string, string>> baseService;
		private BaseService<ReviewModel> baseService1;
		private RelativeLayout loading;
		private ScrollView scrollView;
		private String marker;
		private Dictionary<string, ReviewModel> reviews;
		private TextView empty;
		private DateTime dt;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.review_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			ConstructRightIcon();
			SetTitleActionBar1(GetString(Resource.String.Reviews));
			marker = Intent.GetStringExtra("marker");
			baseService = new BaseService<Dictionary<string, string>>();
			baseService1 = new BaseService<ReviewModel>();
			review = new Dictionary<string, string>();
			reviewLayout = FindViewById<LinearLayout>(Resource.Id.FlyContent);
			empty = FindViewById<TextView>(Resource.Id.emptyReview);
			var route = new RouteGenerator();
			loading = FindViewById<RelativeLayout>(Resource.Id.main_loading);
			scrollView = FindViewById<ScrollView>(Resource.Id.ScrollList);
			loading.Visibility = ViewStates.Visible;
			scrollView.Visibility = ViewStates.Gone;
			var addButton = FindViewById<ImageView>(Resource.Id.addReview);
			reviews = new Dictionary<string, ReviewModel>();
			if (marker != null)
			{
				ThreadPool.QueueUserWorkItem(o => GetData(marker));
			}
			addButton.Click += delegate
			{
				var intent = new Intent(this, typeof(ReviewAddActivity));
				intent.PutExtra("marker", marker);
				StartActivity(intent);
				Finish();
			};

		}

		public void GetData(string marker)
		{
			try
			{
				review = (baseService.Get(string.Format("cabins/{0}/Reviews", marker)));
				foreach (var rev in review)
				{
					var abc = baseService1.Get(string.Format("reviews/{0}", rev.Value));
					reviews.Add(rev.Value, abc);
				}
			}
			catch (Exception e)
			{
				var a = 0;
			}
			RunOnUiThread(() =>
			{
				FillLayout(reviews);
			});
		}

		public void FillLayout(Dictionary<string, ReviewModel> revs)
		{
			if (reviewLayout.ChildCount != 0)
			{
				reviewLayout.RemoveAllViews();
			}
			foreach (var rev in revs)
			{

				var view = LayoutInflater.Inflate(Resource.Layout.review_view_layout, null);
				var userImage = view.FindViewById<ImageView>(Resource.Id.userImage);
				var userName = view.FindViewById<TextView>(Resource.Id.userName);
				var ratingStar = view.FindViewById<RatingBar>(Resource.Id.userRating);
				var titleRev = view.FindViewById<TextView>(Resource.Id.titlereview);
				var duration = view.FindViewById<TextView>(Resource.Id.duration);
				var revText = view.FindViewById<TextView>(Resource.Id.reviewText);
				var report = view.FindViewById<TextView>(Resource.Id.reportButton);
				var usefull = view.FindViewById<TextView>(Resource.Id.usefullButton);
				usefull.Click += delegate
				{
					rev.Value.Usefull = rev.Value.Usefull + 1;
					baseService1.UpdateUser(rev.Value, string.Format("reviews/{0}", rev.Key));
					usefull.Clickable = false;
				};
				report.Click += delegate
				{
					rev.Value.Usefull = rev.Value.Usefull - 1;
					report.Clickable = false;
					baseService1.UpdateUser(rev.Value, string.Format("reviews/{0}", rev.Key));

				};
				if (rev.Value.UserPhoto != null)
				{
					var picture = Decode(rev.Value.UserPhoto);
					userImage.SetImageDrawable(new RoundedImage(picture, this, ""));
					userImage.SetScaleType(ImageView.ScaleType.CenterInside);
				}
				else {
					userImage.SetImageDrawable(new RoundedImage(BitmapFactory.DecodeResource(Resources, Resource.Drawable.avatarplaceholder), this, ""));
					userImage.SetScaleType(ImageView.ScaleType.CenterInside);
				}
				var now = DateTime.Now;
				if (now.ToString().Contains("/"))
				{
					dt = DateTime.ParseExact(rev.Value.DateAdd, "dd/MM/yyyy", null);
				}
				else {
					var newdate = rev.Value.DateAdd.Replace("/", ".");
					dt = DateTime.ParseExact(newdate, "dd.MM.yyyy", null);
				}

				var diff = (now - dt).TotalDays;

				if (diff <= 1)
				{
					duration.Text = "Today";
				}
				else if (diff > 1 && diff < 7)
				{
					duration.Text = "1 wk";
				}
				else if (diff >= 7 && diff < 30)
				{
					duration.Text = Convert.ToInt32(diff / 7).ToString() + " wk";
				}
				else if (diff >= 30 && diff < 365)
				{
					duration.Text = Convert.ToInt32(diff / 30).ToString() + " mth";
				}
				else if (diff >= 365)
				{
					duration.Text = Convert.ToInt32(diff / 365).ToString() + " yr";
				}
				titleRev.Text = rev.Value.Title;
				userName.Text = rev.Value.UserNameAdded;
				ratingStar.Progress = rev.Value.Rating;
				revText.Text = rev.Value.ReviewText;
				reviewLayout.AddView(view);

			}
			reviewLayout.RequestLayout();
			if (reviewLayout.ChildCount == 0)
			{
				Toast.MakeText(this, GetString(Resource.String.Reviewnotfound), ToastLength.Short).Show();
				empty.Visibility = ViewStates.Visible;
				loading.Visibility = ViewStates.Gone;
				scrollView.Visibility = ViewStates.Visible;
			}
			else {
				loading.Visibility = ViewStates.Gone;
				empty.Visibility = ViewStates.Gone;
				scrollView.Visibility = ViewStates.Visible;
			}
		}

		public void ConstructRightIcon()
		{
			var actionBar = SupportActionBar;

			actionBar.SetDisplayShowCustomEnabled(true);
			LayoutInflater inflate = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
			View view = inflate.Inflate(Resource.Layout.action_bar_home, null);
			actionBar.SetCustomView(Resource.Layout.action_bar_home);

			view.Click += delegate
			{
				var a = 0;
			};
			var MenuButton = SupportActionBar.CustomView.FindViewById<ImageButton>(Resource.Id.action_bar_menuBtn);

			MenuButton.Visibility = ViewStates.Visible;
			MenuButton.Click += (object sender, EventArgs e) =>
			{
				PopupMenu menu = new PopupMenu(this, MenuButton);
				menu.Inflate(Resource.Menu.review_menu);
				menu.MenuItemClick += (object sender1, PopupMenu.MenuItemClickEventArgs e1) =>
				{
					var a = e1.Item.TitleFormatted.ToString();
					if (a.Equals("Newest"))
					{
						FillLayout(reviews.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
					}
					else if (a.Equals("Best"))
					{
						var ordbest = reviews.OrderBy(item => item.Value.Rating);
						FillLayout(ordbest.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
					}
				};

				menu.DismissEvent += delegate
				{
				};
				menu.Show();
			};

		}
	}
}

