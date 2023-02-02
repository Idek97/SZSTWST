package crc6441400785c83554ee;


public class AssetListActivity
	extends androidx.appcompat.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onBackPressed:()V:GetOnBackPressedHandler\n" +
			"";
		mono.android.Runtime.register ("SZSTWST_MobileApp.AssetListActivity, SZSTWST_MobileApp", AssetListActivity.class, __md_methods);
	}


	public AssetListActivity ()
	{
		super ();
		if (getClass () == AssetListActivity.class) {
			mono.android.TypeManager.Activate ("SZSTWST_MobileApp.AssetListActivity, SZSTWST_MobileApp", "", this, new java.lang.Object[] {  });
		}
	}


	public AssetListActivity (int p0)
	{
		super (p0);
		if (getClass () == AssetListActivity.class) {
			mono.android.TypeManager.Activate ("SZSTWST_MobileApp.AssetListActivity, SZSTWST_MobileApp", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
		}
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();


	public void onBackPressed ()
	{
		n_onBackPressed ();
	}

	private native void n_onBackPressed ();

	private java.util.ArrayList refList;
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
