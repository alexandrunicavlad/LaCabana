package md53fd388ed257ece170a8fa9579f8e80c8;


public class LiteListDemoActivity_ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.LiteListDemoActivity+ViewHolder, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LiteListDemoActivity_ViewHolder.class, __md_methods);
	}


	public LiteListDemoActivity_ViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LiteListDemoActivity_ViewHolder.class)
			mono.android.TypeManager.Activate ("MapsSample.LiteListDemoActivity+ViewHolder, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


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
