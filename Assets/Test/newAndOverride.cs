using UnityEngine;
using System.Collections;

public class newAndOverride : MonoBehaviour
{
	void Start()
	{
		SDK sdk = new SDK ();
		sdk.print ();	 // SDK
		SDK91 sdk91 = new SDK91 ();
		sdk91.print ();  // SDK91
		SDKFL sdkfl = new SDKFL ();
		sdkfl.print ();  // SDKFL
		((SDK)sdk91).print (); //SDK
		((SDK)sdkfl).print (); //SDKFL
	}
}

public class SDK
{
	public virtual void print()
	{
		Debug.Log (" SDK  ");
	}
}

public class SDK91 :SDK
{
	public new void print()
	{
		Debug.Log (" SDK91 ..");
	}
}

public class SDKFL :SDK
{
	public override void print()
	{
		Debug.Log (" SDKFL ..");
	}
}