using UnityEngine;
using System.Collections;

public class MainTest : MonoBehaviour 
{
	public GameObject Button;

	private UIButton ButtonA;
	private UIButton ButtonB;

	void Start()
	{
		ButtonA = UIButton.Create(Button);
		ButtonA.gameObject.name = "ButtonA";
		ButtonA.transform.parent = Global.Instance.transform;
		ButtonA.transform.localPosition = new Vector3(-100, 0, 0);
		ButtonA.SetValueChangedDelegate(DoButton);

		ButtonB = UIButton.Create(Button);
		ButtonB.gameObject.name = "ButtonB";
		ButtonB.transform.parent = Global.Instance.transform;
		ButtonB.transform.localPosition = new Vector3(100, 0, 0);
		ButtonB.SetValueChangedDelegate(DoButton);
	}
	
	// Update is called once per frame
	void DoButton (IUIObject obj)
	{
		if(obj.name == "ButtonA")
		{
			int a = 10; 
			int b = 10;
			Debug.Log(2 >> 1);
			Debug.Log("is Button A");
		}
		else if(obj.name == "ButtonB")
		{
			Debug.Log("is Button B");
		}
	}
}
