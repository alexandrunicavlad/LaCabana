package md53fd388ed257ece170a8fa9579f8e80c8;


public class OptionsDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("MapsSample.OptionsDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OptionsDemoActivity.class, __md_methods);
	}


	public OptionsDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OptionsDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.OptionsDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
