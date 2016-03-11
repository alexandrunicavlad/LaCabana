package md53fd388ed257ece170a8fa9579f8e80c8;


public class SplitStreetViewPanoramaAndMapDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnStreetViewPanoramaReadyCallback,
		com.google.android.gms.maps.OnMapReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onSaveInstanceState:(Landroid/os/Bundle;)V:GetOnSaveInstanceState_Landroid_os_Bundle_Handler\n" +
			"n_onStreetViewPanoramaReady:(Lcom/google/android/gms/maps/StreetViewPanorama;)V:GetOnStreetViewPanoramaReady_Lcom_google_android_gms_maps_StreetViewPanorama_Handler:Android.Gms.Maps.IOnStreetViewPanoramaReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.SplitStreetViewPanoramaAndMapDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SplitStreetViewPanoramaAndMapDemoActivity.class, __md_methods);
	}


	public SplitStreetViewPanoramaAndMapDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SplitStreetViewPanoramaAndMapDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.SplitStreetViewPanoramaAndMapDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onSaveInstanceState (android.os.Bundle p0)
	{
		n_onSaveInstanceState (p0);
	}

	private native void n_onSaveInstanceState (android.os.Bundle p0);


	public void onStreetViewPanoramaReady (com.google.android.gms.maps.StreetViewPanorama p0)
	{
		n_onStreetViewPanoramaReady (p0);
	}

	private native void n_onStreetViewPanoramaReady (com.google.android.gms.maps.StreetViewPanorama p0);


	public void onMapReady (com.google.android.gms.maps.GoogleMap p0)
	{
		n_onMapReady (p0);
	}

	private native void n_onMapReady (com.google.android.gms.maps.GoogleMap p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
