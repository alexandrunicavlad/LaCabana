package md53fd388ed257ece170a8fa9579f8e80c8;


public class CameraDemoActivity_MyCancelableCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.GoogleMap.CancelableCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCancel:()V:GetOnCancelHandler:Android.Gms.Maps.GoogleMap/ICancelableCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"n_onFinish:()V:GetOnFinishHandler:Android.Gms.Maps.GoogleMap/ICancelableCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.CameraDemoActivity+MyCancelableCallback, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraDemoActivity_MyCancelableCallback.class, __md_methods);
	}


	public CameraDemoActivity_MyCancelableCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CameraDemoActivity_MyCancelableCallback.class)
			mono.android.TypeManager.Activate ("MapsSample.CameraDemoActivity+MyCancelableCallback, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCancel ()
	{
		n_onCancel ();
	}

	private native void n_onCancel ();


	public void onFinish ()
	{
		n_onFinish ();
	}

	private native void n_onFinish ();

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
