package md53fd388ed257ece170a8fa9579f8e80c8;


public class LayersDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_OnTrafficToggled:(Landroid/view/View;)V:__export__\n" +
			"n_OnMyLocationToggled:(Landroid/view/View;)V:__export__\n" +
			"n_OnBuildingsToggled:(Landroid/view/View;)V:__export__\n" +
			"n_OnIndoorToggled:(Landroid/view/View;)V:__export__\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.LayersDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LayersDemoActivity.class, __md_methods);
	}


	public LayersDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LayersDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.LayersDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onTrafficToggled (android.view.View p0)
	{
		n_OnTrafficToggled (p0);
	}

	private native void n_OnTrafficToggled (android.view.View p0);


	public void onMyLocationToggled (android.view.View p0)
	{
		n_OnMyLocationToggled (p0);
	}

	private native void n_OnMyLocationToggled (android.view.View p0);


	public void onBuildingsToggled (android.view.View p0)
	{
		n_OnBuildingsToggled (p0);
	}

	private native void n_OnBuildingsToggled (android.view.View p0);


	public void onIndoorToggled (android.view.View p0)
	{
		n_OnIndoorToggled (p0);
	}

	private native void n_OnIndoorToggled (android.view.View p0);


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
