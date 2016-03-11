package md53fd388ed257ece170a8fa9579f8e80c8;


public class LiteListDemoActivity_NamedLocation
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MapsSample.LiteListDemoActivity+NamedLocation, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LiteListDemoActivity_NamedLocation.class, __md_methods);
	}


	public LiteListDemoActivity_NamedLocation () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LiteListDemoActivity_NamedLocation.class)
			mono.android.TypeManager.Activate ("MapsSample.LiteListDemoActivity+NamedLocation, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LiteListDemoActivity_NamedLocation (java.lang.String p0, com.google.android.gms.maps.model.LatLng p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LiteListDemoActivity_NamedLocation.class)
			mono.android.TypeManager.Activate ("MapsSample.LiteListDemoActivity+NamedLocation, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Gms.Maps.Model.LatLng, Xamarin.GooglePlayServices.Maps, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1 });
	}

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
