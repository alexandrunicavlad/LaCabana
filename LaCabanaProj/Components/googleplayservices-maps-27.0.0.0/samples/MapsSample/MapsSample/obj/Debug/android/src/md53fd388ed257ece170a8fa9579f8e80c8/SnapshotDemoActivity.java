package md53fd388ed257ece170a8fa9579f8e80c8;


public class SnapshotDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback,
		com.google.android.gms.maps.GoogleMap.OnMapLoadedCallback,
		com.google.android.gms.maps.GoogleMap.SnapshotReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_OnScreenshot:(Landroid/view/View;)V:__export__\n" +
			"n_OnClearScreenshot:(Landroid/view/View;)V:__export__\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"n_onMapLoaded:()V:GetOnMapLoadedHandler:Android.Gms.Maps.GoogleMap/IOnMapLoadedCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"n_onSnapshotReady:(Landroid/graphics/Bitmap;)V:GetOnSnapshotReady_Landroid_graphics_Bitmap_Handler:Android.Gms.Maps.GoogleMap/ISnapshotReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.SnapshotDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SnapshotDemoActivity.class, __md_methods);
	}


	public SnapshotDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SnapshotDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.SnapshotDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onScreenshot (android.view.View p0)
	{
		n_OnScreenshot (p0);
	}

	private native void n_OnScreenshot (android.view.View p0);


	public void onClearScreenshot (android.view.View p0)
	{
		n_OnClearScreenshot (p0);
	}

	private native void n_OnClearScreenshot (android.view.View p0);


	public void onMapReady (com.google.android.gms.maps.GoogleMap p0)
	{
		n_onMapReady (p0);
	}

	private native void n_onMapReady (com.google.android.gms.maps.GoogleMap p0);


	public void onMapLoaded ()
	{
		n_onMapLoaded ();
	}

	private native void n_onMapLoaded ();


	public void onSnapshotReady (android.graphics.Bitmap p0)
	{
		n_onSnapshotReady (p0);
	}

	private native void n_onSnapshotReady (android.graphics.Bitmap p0);

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
