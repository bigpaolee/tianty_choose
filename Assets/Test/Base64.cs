using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Base64 : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
        string str = "hello Unity3d";
        Debug.Log(str);
        Debug.Log(str = Base64Encode(str));
        Debug.Log(Base64Decode(str));
	}

    public static string Base64Decode(string str)
    {
        byte[] bytes = Convert.FromBase64String(str);
        bytes = Convert.FromBase64String(Encoding.Default.GetString(bytes));
        return Encoding.Default.GetString(bytes);
    }


    public static string Base64Encode(string str)
    {
        string go = Convert.ToBase64String(Encoding.Default.GetBytes(str));
        return Convert.ToBase64String(Encoding.Default.GetBytes(go));
    }
}
