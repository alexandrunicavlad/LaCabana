package md5dbce641a4b5bf99921cca81d5bb55bcc;


public class BaseDrawerActivity
	extends md5dbce641a4b5bf99921cca81d5bb55bcc.BaseActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onPrepareOptionsMenu:(Landroid/view/Menu;)Z:GetOnPrepareOptionsMenu_Landroid_view_Menu_Handler\n" +
			"";
		mono.android.Runtime.register ("LaCabana.BaseDrawerActivity, LaCabana, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BaseDrawerActivity.class, __md_methods);
	}


	public BaseDrawerActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BaseDrawerActivity.class)
			mono.android.TypeManager.Activate ("LaCabana.BaseDrawerActivity, LaCabana, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onPrepareOptionsMenu (android.view.Menu p0)
	{
		return n_onPrepareOptionsMenu (p0);
	}

	private native boolean n_onPrepareOptionsMenu (android.view.Menu p0);

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
