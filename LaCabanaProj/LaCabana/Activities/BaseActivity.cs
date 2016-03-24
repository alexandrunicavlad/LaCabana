
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
using Android.Support.V7.App;
using Android.Views.InputMethods;

namespace LaCabana
{
	[Activity (Label = "BaseActivity")]			
	public class BaseActivity : ActionBarActivity
	{
		protected Dialog Dialog;
		//protected ValidationHelper ValidationHelper;
		public static string Token;
		protected static String UserName;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

//			if (ValidationHelper == null)
//				ValidationHelper = new ValidationHelper(ApplicationContext);
		}

		protected void HandleErrors (Exception e)
		{
			string message = e.Message;
			switch (e.Message) {
			case "Could not resolve host 'google.com'":
				//message = Resources.GetString(Resource.String.ValidationInternetConnection);
				break;
			}
			CreateDialog (message);
		}

		#region Dialog

		protected void CreateDialog (string message)
		{
			CreateDialog ("", message, true, "OK");
		}

		protected  void CreateDialog (string message, bool okButton)
		{
			CreateDialog ("Error", message, okButton, "Ok", false, "", false);
		}

		protected  void CreateDialog (string message, bool okButton, bool loading)
		{
			CreateDialog ("", message, okButton, "", false, "", loading);
		}

		protected  void CreateDialog (string title, string message, bool okButton)
		{
			CreateDialog (title, message, okButton, "Ok", false, "", false);
		}

		protected  void CreateDialog (string title, string message, bool okButton, string okMessage)
		{
			CreateDialog (title, message, okButton, okMessage, false, "", false);
		}

		protected  void CreateDialog (string title, string message, bool okButton, bool loading)
		{
			CreateDialog (title, message, okButton, "Ok", false, "", true);
		}

		protected  void CreateDialog (string title, string message, bool okButton, string okMessage, bool loading)
		{
			CreateDialog (title, message, okButton, okMessage, false, "", true);
		}

		protected  void CreateDialog (string title, string message, bool okButton, string okMessage, bool cancelButton, string cancelMessage, bool loading)
		{
			RunOnUiThread (() => {
				var builder = new Android.App.AlertDialog.Builder (this);

				builder.SetMessage (message);
				if (title != "")
					builder.SetTitle (title);

				if (okButton)
					builder.SetPositiveButton (okMessage, (EventHandler<DialogClickEventArgs>)null);

				if (cancelButton)
					builder.SetNegativeButton (cancelMessage, (EventHandler<DialogClickEventArgs>)null);

				var dialog = builder.Create ();

				if (loading) {
					dialog = ProgressDialog.Show (this, null, message, false, okButton);
				}

				dialog.Show ();

				if (Dialog != null && Dialog.IsShowing) {
					Dialog.Cancel ();
				}

				Dialog = dialog;

				if (!okButton)
					return;

				var okBtn = dialog.GetButton ((int)DialogButtonType.Positive);

				if (okBtn == null)
					return;

				okBtn.Click += delegate {
					if (dialog != null && dialog.IsShowing) {
						dialog.Cancel ();
					}
				};

			});
		}

		#endregion

		protected void HideKeyboard (View view)
		{
			view.RequestFocus ();
			var inputManager = (InputMethodManager)GetSystemService (Context.InputMethodService);
			inputManager.HideSoftInputFromWindow (view.WindowToken, HideSoftInputFlags.None);
		}
	}
}



