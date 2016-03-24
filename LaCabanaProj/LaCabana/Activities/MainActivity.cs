using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using System.Reflection;

namespace LaCabana
{
	[Activity]
	public class MainActivity : Activity
	{
		private static readonly string DatabaseDirectory =
			Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "../databases");
		public const string DatabaseFileName = "LaCabanaDb";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			CreateSqLiteDatabase ();
			StartActivity (typeof(LoginActivity));
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
				strSqLitePathOnDevice = Path.Combine (DatabaseDirectory, DatabaseFileName);
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
			} catch (Exception) {
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