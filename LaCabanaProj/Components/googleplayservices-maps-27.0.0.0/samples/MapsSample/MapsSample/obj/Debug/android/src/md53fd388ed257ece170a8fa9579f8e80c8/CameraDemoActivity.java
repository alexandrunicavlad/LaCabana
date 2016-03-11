package md53fd388ed257ece170a8fa9579f8e80c8;


public class CameraDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_OnGoToBondi:(Landroid/view/View;)V:__export__\n" +
			"n_OnGoToSydney:(Landroid/view/View;)V:__export__\n" +
			"n_OnStopAnimation:(Landroid/view/View;)V:__export__\n" +
			"n_OnZoomIn:(Landroid/view/View;)V:__export__\n" +
			"n_OnZoomOut:(Landroid/view/View;)V:__export__\n" +
			"n_OnTiltMore:(Landroid/view/View;)V:__export__\n" +
			"n_OnTiltLess:(Landroid/view/View;)V:__export__\n" +
			"n_OnScrollLeft:(Landroid/view/View;)V:__export__\n" +
			"n_OnScrollRight:(Landroid/view/View;)V:__export__\n" +
			"n_OnScrollUp:(Landroid/view/View;)V:__export__\n" +
			"n_OnScrollDown:(Landroid/view/View;)V:__export__\n" +
			"n_OnToggleAnimate:(Landroid/view/View;)V:__export__\n" +
			"n_OnToggleCustomDuration:(Landroid/view/View;)V:__export__\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.CameraDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraDemoActivity.class, __md_methods);
	}


	public CameraDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CameraDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.CameraDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onGoToBondi (android.view.View p0)
	{
		n_OnGoToBondi (p0);
	}

	private native void n_OnGoToBondi (android.view.View p0);


	public void onGoToSydney (android.view.View p0)
	{
		n_OnGoToSydney (p0);
	}

	private native void n_OnGoToSydney (android.view.View p0);


	public void onStopAnimation (android.view.View p0)
	{
		n_OnStopAnimation (p0);
	}

	private native void n_OnStopAnimation (android.view.View p0);


	public void onZoomIn (android.view.View p0)
	{
		n_OnZoomIn (p0);
	}

	private native void n_OnZoomIn (android.view.View p0);


	public void onZoomOut (android.view.View p0)
	{
		n_OnZoomOut (p0);
	}

	private native void n_OnZoomOut (android.view.View p0);


	public void onTiltMore (android.view.View p0)
	{
		n_OnTiltMore (p0);
	}

	private native void n_OnTiltMore (android.view.View p0);


	public void onTiltLess (android.view.View p0)
	{
		n_OnTiltLess (p0);
	}

	private native void n_OnTiltLess (android.view.View p0);


	public void onScrollLeft (android.view.View p0)
	{
		n_OnScrollLeft (p0);
	}

	private native void n_OnScrollLeft (android.view.View p0);


	public void onScrollRight (android.view.View p0)
	{
		n_OnScrollRight (p0);
	}

	private native void n_OnScrollRight (android.view.View p0);


	public void onScrollUp (android.view.View p0)
	{
		n_OnScrollUp (p0);
	}

	private native void n_OnScrollUp (android.view.View p0);


	public void onScrollDown (android.view.View p0)
	{
		n_OnScrollDown (p0);
	}

	private native void n_OnScrollDown (android.view.View p0);


	public void onToggleAnimate (android.view.View p0)
	{
		n_OnToggleAnimate (p0);
	}

	private native void n_OnToggleAnimate (android.view.View p0);


	public void onToggleCustomDuration (android.view.View p0)
	{
		n_OnToggleCustomDuration (p0);
	}

	private native void n_OnToggleCustomDuration (android.view.View p0);


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
