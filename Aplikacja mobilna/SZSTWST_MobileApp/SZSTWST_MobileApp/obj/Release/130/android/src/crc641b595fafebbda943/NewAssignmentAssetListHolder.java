package crc641b595fafebbda943;


public class NewAssignmentAssetListHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SZSTWST_MobileApp.Adapters.NewAssignmentAssetListHolder, SZSTWST_MobileApp", NewAssignmentAssetListHolder.class, __md_methods);
	}


	public NewAssignmentAssetListHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == NewAssignmentAssetListHolder.class) {
			mono.android.TypeManager.Activate ("SZSTWST_MobileApp.Adapters.NewAssignmentAssetListHolder, SZSTWST_MobileApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}

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
