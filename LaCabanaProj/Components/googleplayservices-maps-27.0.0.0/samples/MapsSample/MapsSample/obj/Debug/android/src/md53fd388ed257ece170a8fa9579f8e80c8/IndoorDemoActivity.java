package md53fd388ed257ece170a8fa9579f8e80c8;


public class IndoorDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_OnToggleLevelPicker:(Landroid/view/View;)V:__export__\n" +
			"n_OnFocusedBuildingInfo:(Landroid/view/View;)V:__export__\n" +
			"n_OnVisibleLevelInfo:(Landroid/view/View;)V:__export__\n" +
			"n_OnHigherLevel:(Landroid/view/View;)V:__export__\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.IndoorDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", IndoorDemoActivity.class, __md_methods);
	}


	public IndoorDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == IndoorDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.IndoorDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onToggleLevelPicker (android.view.View p0)
	{
		n_OnToggleLevelPicker (p0);
	}

	private native void n_OnToggleLevelPicker (android.view.View p0);


	public void onFocusedBuildingInfo (android.view.View p0)
	{
		n_OnFocusedBuildingInfo (p0);
	}

	private native void n_OnFocusedBuildingInfo (android.view.View p0);


	public void onVisibleLevelInfo (android.view.View p0)
	{
		n_OnVisibleLevelInfo (p0);
	}

	private native void n_OnVisibleLevelInfo (android.view.View p0);


	public void onHigherLevel (android.view.View p0)
	{
		n_OnHigherLevel (p0);
	}

	private native void n_OnHigherLevel (android.view.View p0);


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
