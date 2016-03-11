using Android.App;
using Android.Widget;
using Android.OS;

namespace LaCabana
{
	[Activity]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartActivity (typeof(LoginActivity));
			Finish ();

		}



	}
}