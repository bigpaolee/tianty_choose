using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateNewGameObject : ScriptableObject 
{
	[UnityEditor.MenuItem("Tools/Tianty Software/CreateNewGameObject")]
	public static void Create()
	{
		GameObject go = new GameObject("NewGameObject");
		go.transform.localPosition = Vector3.zero;
	}
}
