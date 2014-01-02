using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour 
{
	private static Global instance;
	public static Global Instance {get{return instance;}}

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
