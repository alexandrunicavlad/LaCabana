using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using System.Reflection;
using System.Threading;
using Android.Graphics;

namespace LaCabana
{
	[Activity (MainLauncher = true, Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class HomeLogo : Activity
	{

		private static readonly string DatabaseDirectory =
			System.IO.Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "../databases");
		public const string DatabaseFileName = "LaCabanaDb";

		protected override void OnCreate (Bundle bundle)
		{
			CreateSqLiteDatabase ();
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.home_logo);

			var progressBar = FindViewById<ProgressBar> (Resource.Id.splash_progressBar);

			var apiVersion = Android.OS.Build.VERSION.SdkInt;
			if (apiVersion <= Android.OS.BuildVersionCodes.Kitkat) {
				progressBar.IndeterminateDrawable.SetColorFilter (Resources.GetColor (Resource.Color.green), PorterDuff.Mode.SrcAtop);
			}
			ThreadPool.QueueUserWorkItem (o => StartMainActivity ());

		}

		private void StartMainActivity ()
		{           
			Java.Lang.Thread.Sleep (3000);
			StartActivity (typeof(BasicMapDemoActivity));
			Finish ();
		}

		private bool CreateSqLiteDatabase ()
		{
			var strSqLitePathOnDevice = GetSQLitePathOnDevice ();
			var isSqLiteInitialized = false;
			try {
				if (File.Exists (strSqLitePathOnDevice)) {
					isSqLiteInitialized = true;
				} else {
					var streamSqLite = Assets.Open (DatabaseFileName);
					Directory.CreateDirectory (DatabaseDirectory);
					var streamWrite = new FileStream (strSqLitePathOnDevice, FileMode.OpenOrCreate,
						                  FileAccess.Write);
					if (streamSqLite != null) {
						if (CopySQLiteOnDevice (streamSqLite, streamWrite)) {
							isSqLiteInitialized = true;
						}
					}
				}
			} catch (Exception) {
				var currentMethod = MethodBase.GetCurrentMethod ();
				/*if (currentMethod.DeclaringType != null)
                Console.WriteLine(String.Format("CLASS : {0}; METHOD : {1}; EXCEPTION : {2}"
                    , currentMethod.DeclaringType.FullName
                    , currentMethod.Name
                    , exception.Message));*/
			}
			return isSqLiteInitialized;
		}

		private string GetSQLitePathOnDevice ()
		{
			var strSqLitePathOnDevice = string.Empty;
			try {
				strSqLitePathOnDevice = System.IO.Path.Combine (DatabaseDirectory, DatabaseFileName);
			} catch (Exception) {
				var currentMethod = MethodBase.GetCurrentMethod ();
				/* Console.WriteLine(String.Format("CLASS : {0}; METHOD : {1}; EXCEPTION : {2}"
                 , currentMethod.DeclaringType.FullName
                 , currentMethod.Name
                 , exception.Message));*/
			}
			return strSqLitePathOnDevice;
		}

		private bool CopySQLiteOnDevice (Stream streamSqLite, Stream streamWrite)
		{
			bool isSuccess = false;
			const int length = 256;
			var buffer = new Byte[length];
			try {
				int bytesRead = streamSqLite.Read (buffer, 0, length);
				while (bytesRead > 0) {
					streamWrite.Write (buffer, 0, bytesRead);
					bytesRead = streamSqLite.Read (buffer, 0, length);
				}
				isSuccess = true;
			} catch (System.Exception) {
				var currentMethod = MethodBase.GetCurrentMethod ();
				/* Console.WriteLine(String.Format("CLASS : {0}; METHOD : {1}; EXCEPTION : {2}"
                    , currentMethod.DeclaringType.FullName
                    , currentMethod.Name
                    , exception.Message));*/
			} finally {
				streamSqLite.Close ();
				streamWrite.Close ();
			}
			return isSuccess;

		}
	}
}

