
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
using Android.Gms.Maps.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Android.Graphics;
using System.Threading;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Graphics.Drawables;

namespace LaCabana
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
	public class CabinInfo : ActionBarActivity
	{
		private CabinModel cabin;
		private string requestURL = "https://api.cloudinary.com/v1_1/lacabana/resources/image/upload/?prefix=";
		private const string ApiKey = "348639768631669";
		private const string ApiSecret = "HHeKWX7znazzS61cd7tlTxBmV7I";
		private string marker;
		private RelativeLayout loading;
		private ScrollView scrollView;
		private BaseService<CabinModel> baseService;
		private Android.Support.V7.Widget.Toolbar toolbar;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.cabin_info_layout);
			//SetupDrawer (FindViewById<DrawerLayout> (Resource.Id.drawerLayout));
			ConstructActionBar();
			//SetTitleActionBar1("Cabin");
			marker = Intent.GetStringExtra("marker");
			baseService = new BaseService<CabinModel>();
			cabin = new CabinModel();

			loading = FindViewById<RelativeLayout>(Resource.Id.main_loading);
			scrollView = FindViewById<ScrollView>(Resource.Id.scrollViewMain);

			if (marker != null)
			{
				ThreadPool.QueueUserWorkItem(o =>
				{
					GetData();
				});
			}
		}

		private void GetData()
		{
			try
			{
				cabin = (baseService.Get(string.Format("cabins/{0}", marker)));
				FillLayout();
			}
			catch (Exception e)
			{
				var a = 0;
			}
			RunOnUiThread(() =>
			{
				loading.Visibility = ViewStates.Gone;
				scrollView.Visibility = ViewStates.Visible;
			});
		}

		private void FillLayout()
		{
			var cabinPhoto = FindViewById<ImageView>(Resource.Id.cabinImage);
			var cabinName = FindViewById<TextView>(Resource.Id.cabinName);
			var phoneText1 = FindViewById<TextView>(Resource.Id.phoneText1);
			var phoneType1 = FindViewById<TextView>(Resource.Id.phoneType1);
			var phoneText2 = FindViewById<TextView>(Resource.Id.phoneText2);
			var phoneType2 = FindViewById<TextView>(Resource.Id.phoneType2);
			var emailText = FindViewById<TextView>(Resource.Id.emailText);
			var emailType = FindViewById<TextView>(Resource.Id.emailType);
			var streetText = FindViewById<TextView>(Resource.Id.streetText);
			var priceText = FindViewById<TextView>(Resource.Id.priceText);
			var destailsText = FindViewById<TextView>(Resource.Id.detailsText);
			var directionText = FindViewById<ImageView>(Resource.Id.cabinDirection);
			var phoneLayout = FindViewById<LinearLayout>(Resource.Id.phoneLayout1);
			var emailLayout = FindViewById<RelativeLayout>(Resource.Id.emailLayout);
			var route = new RouteGenerator();
			cabinName.Text = cabin.Name;
			phoneText1.Text = string.Format("0{0}", cabin.Phone.ToString());
			phoneType1.Text = cabin.PhoneType;
			emailText.Text = cabin.Email;
			emailType.Text = cabin.EmailType;
			priceText.Text = string.Format("{0} {1}", cabin.Price.ToString(), cabin.PriceType);
			destailsText.Text = cabin.Details;
			if (cabin.Pictures != null)
			{
				if (cabin.Pictures.ContainsKey("main"))
				{
					var baseService1 = new BaseService<byte[]>();
					var abc = baseService1.Get(string.Format("pictures/{0}", cabin.Pictures.Last().Value));
					cabinPhoto.SetImageBitmap(BitmapFactory.DecodeByteArray(abc, 0, abc.Length));
					cabinPhoto.SetScaleType(ImageView.ScaleType.FitXy);
				}
				else {
					cabinPhoto.SetImageResource(Resource.Drawable.cabana_photo);
					cabinPhoto.SetScaleType(ImageView.ScaleType.CenterCrop);
				}
			}
			else {
				cabinPhoto.SetImageResource(Resource.Drawable.cabana_photo);
				cabinPhoto.SetScaleType(ImageView.ScaleType.CenterCrop);
			}

			var latitude = Intent.GetDoubleExtra("latitude", 0);
			var longitude = Intent.GetDoubleExtra("longitude", 0);

			phoneLayout.Click += delegate
						{
							var uri = Android.Net.Uri.Parse(string.Format("tel:0{0}", cabin.Phone.ToString()));
							var intent = new Intent(Intent.ActionDial, uri);
							StartActivity(intent);
						};
			emailLayout.Click += delegate
			{
				var email = new Intent(Android.Content.Intent.ActionSend);
				email.PutExtra(Android.Content.Intent.ExtraEmail,
									new string[] { "", cabin.Email });
				email.PutExtra(Android.Content.Intent.ExtraSubject, string.Format("Hello Cabin {0}", cabin.Name));
				email.PutExtra(Android.Content.Intent.ExtraText, string.Format("Hello Cabin {0}", cabin.Name));
				email.SetType("message/rfc822");
				StartActivity(email);
			};
			Org.W3c.Dom.IDocument doc = route.GetDocument(new LatLng(latitude, longitude), new LatLng(cabin.Latitude, cabin.Longitude), RouteGenerator.Mode_driving);
			streetText.Text = route.GetEndAddress(doc);
			directionText.Click += delegate
			{

				var newuri = string.Format("http://maps.google.com/maps?saddr={0},{1}&daddr={2},{3}", latitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 longitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 cabin.Latitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture),
								 cabin.Longitude.ToString("00.0000000", System.Globalization.CultureInfo.InvariantCulture));

				Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse(newuri);
				Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
				mapIntent.SetPackage("com.google.android.apps.maps");
				StartActivity(mapIntent);
			};

		}

		private void ConstructActionBar()
		{
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.tool_bar);

			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayShowTitleEnabled(false);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			toolbar.NavigationClick += delegate
			{
				Finish();
			};

			toolbar.NavigationIcon = Resources.GetDrawable(Resource.Drawable.ic_keyboard_backspace);
			var MenuButton = toolbar.FindViewById<ImageView>(Resource.Id.iconRight);
			MenuButton.Visibility = ViewStates.Visible;
			MenuButton.Click += (object sender, EventArgs e) =>
			{
				Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, MenuButton);
				menu.Inflate(Resource.Menu.cabin_menu);
				menu.MenuItemClick += (object sender1, Android.Support.V7.Widget.PopupMenu.MenuItemClickEventArgs e1) =>
				{
					var a = e1.Item.TitleFormatted.ToString();
					if (a.Equals("Pictures"))
					{
						var intent = new Intent(this, typeof(PicturesActivity));
						intent.PutExtra("marker", marker);
						StartActivity(intent);
					}
					else if (a.Equals("Reviews"))
					{
						var intent = new Intent(this, typeof(ReviewActivity));
						intent.PutExtra("marker", marker);
						StartActivity(intent);
					}
				};
				menu.DismissEvent += delegate
				{
				};
				menu.Show();
			};

		}

		public void ConstructRightIcon()
		{
			var actionBar = SupportActionBar;

			actionBar.SetBackgroundDrawable(new ColorDrawable(Resources.GetColor(Resource.Color.transparent)));
			actionBar.SetDisplayShowCustomEnabled(true);
			LayoutInflater inflate = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
			View view = inflate.Inflate(Resource.Layout.action_bar_home, null);
			actionBar.SetCustomView(Resource.Layout.action_bar_home);

			view.Click += delegate
			{
				var a = 0;
			};
			var MenuButton = SupportActionBar.CustomView.FindViewById<ImageView>(Resource.Id.action_bar_menuBtn);

			MenuButton.Visibility = ViewStates.Visible;
			MenuButton.Click += (object sender, EventArgs e) =>
			{
				Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, MenuButton);
				menu.Inflate(Resource.Menu.cabin_menu);
				menu.MenuItemClick += (object sender1, Android.Support.V7.Widget.PopupMenu.MenuItemClickEventArgs e1) =>
				{
					var a = e1.Item.TitleFormatted.ToString();
					if (a.Equals("Pictures"))
					{
						var intent = new Intent(this, typeof(PicturesActivity));
						intent.PutExtra("marker", marker);
						StartActivity(intent);
					}
					else if (a.Equals("Reviews"))
					{
						var intent = new Intent(this, typeof(ReviewActivity));
						intent.PutExtra("marker", marker);
						StartActivity(intent);
					}
				};
				menu.DismissEvent += delegate
				{
				};
				menu.Show();
			};

		}

		//private void GetData(string type)
		//{
		//	var reqUrl = string.Format("{0}{1}/&max_results=500", requestURL, type);
		//	var request = (HttpWebRequest)WebRequest.Create(reqUrl);
		//	request.Timeout = 10000;
		//	request.Method = "GET";
		//	request.ContentType = "application/json";
		//	request.Credentials = CredentialCache.DefaultCredentials;
		//	var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ApiKey + ":" + ApiSecret));

		//	request.Headers.Add("Authorization", "Basic " + encoded);
		//	try
		//	{
		//		var response = (HttpWebResponse)request.GetResponse();
		//		var reader = new StreamReader(response.GetResponseStream());
		//		var streamText = reader.ReadToEnd();
		//		var deserializedStreamText = JsonConvert.DeserializeObject<Images>(streamText);
		//		//				images.AddRange (deserializedStreamText.resources);

		//	}
		//	catch (Exception ex)
		//	{
		//		HandleErrors(ex);
		//		//retrying = true;
		//	}
		//}
	}
}


