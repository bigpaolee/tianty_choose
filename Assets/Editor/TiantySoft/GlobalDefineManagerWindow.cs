using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GlobalDefine;

public class GlobalDefineManagerWindow :  EditorWindow
{ 
	public static string GlobalDefineDataBasePath = Application.dataPath + "/Profiles"; 
	public static string GlobalDefineDataPath;
	public static string SMCS_FilePath  = Application.dataPath + "/" + "smcs.rsp";
	public static string ProfileName = "GlobalDefineData.json";
	public static string JsonDemo = "{\"ALL_DEFINES\":[\"DefineDemo1\",\"DefineDemo2\"],\"CUR_DEFINES\":[\"DefineDemo1\"]}";

	private GlobalDefineData globalDefineData;
	private bool isInitData = false;
	private bool isSaveSuccess = false;
	private const string defineHead = "-define:";
	private List<string> defineWritedList;
	private string show = "SHOW";

	private List<GlobalDefineItem> GlobalDefineItemList;
	
	[MenuItem ("Tools/Tianty Software/Global Define Manager Window")]
	public static void ShowEditor () {
		// Get existing open window or if none, make a new one:
		GlobalDefineManagerWindow window = (GlobalDefineManagerWindow)EditorWindow.GetWindow (typeof (GlobalDefineManagerWindow));
		window.minSize = new Vector2(300, 600);
	}

	void InitData()
	{
		GlobalDefineDataPath = GlobalDefineDataBasePath + "/" + ProfileName;
		defineWritedList = new List<string>();
		GlobalDefineItemList = new List<GlobalDefineItem>();
		globalDefineData = ParseJson.GetGlobalDefineData(GlobalDefineDataBasePath, ProfileName, JsonDemo);
		foreach(var i in globalDefineData.CurDefines)
		{
			GlobalDefineItem item = new GlobalDefineItem();
			item.define = i;
			GlobalDefineItemList.Add(item);
		}
		globalDefineData.ShowDefinesInfo();
		isInitData = true;
		show = "Refresh";
	}

	void OnDestroy()
	{
		isInitData = false;
		isSaveSuccess = false;
	}

	void OnGUI()
	{
		GUILayout.Space(6f);
		GUILayout.BeginVertical();

			GUI.backgroundColor = Color.green;
			GUILayout.BeginHorizontal();
			
				if(GUILayout.Button(show, GUILayout.Width(100f)))
				{	
					InitData();
				}
			
			GUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;
		
		
		if (isInitData && globalDefineData != null)
		{
			string tip = "提示：" + "\n" 
				+ "\t" + "<ProjectPath>" + GlobalDefineDataPath + "\n"
				+ "\t" +"ALL_DEFINES 为全部define " + "\n"
				+  "\t" +"CUR_DEFINES 为当前配置的define";
			GUILayout.Label(tip);
			
			DrawLine();
			foreach(var i in globalDefineData.AllDefinesForShowList)
			{
				GUILayout.Label(i);
			}

			DrawLine();

			for(int i = 0; i < GlobalDefineItemList.Count; i++)
			{
				GUILayout.BeginHorizontal();

				GlobalDefineItemList[i].define = EditorGUILayout.TextField(GlobalDefineItemList[i].define);
				if(GUILayout.Button("remove", GUILayout.Width(60f)))
				{
					Remove(GlobalDefineItemList[i]);
				}

				GUILayout.EndHorizontal();
			}
				
			GUILayout.Space(5f);


			if(GUILayout.Button("add", GUILayout.Width(200f)))
			{
				Add();
			}


			DrawLine();

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Save Define", GUILayout.Width(200f)))
			{
				Save();
				Refresh();
			}
			GUI.backgroundColor = Color.white;

			if (isSaveSuccess)
			{
				GUILayout.Space(10f);
				GUILayout.Label("Save Success");
			}
		}

		GUILayout.EndVertical();
	}


	void Add()
	{
		GlobalDefineItem item = new GlobalDefineItem();
		item.define = string.Empty;
		
		GlobalDefineItemList.Add(item);
	}

	void Remove(GlobalDefineItem item)
	{
		GlobalDefineItemList.Remove(item);
	}

	void DrawLine()
	{
		GUILayout.Space(10f);

		string go = "-";
		for(int i = 0; i < 100; i++)
		{
			go += "-";
		}

		GUILayout.Label(go);
		GUILayout.Space(10f);
	}

	void Save()
	{
		foreach(var i in GlobalDefineItemList)
		{
			Debug.Log("i ======: " + i.define);
			if (!globalDefineData.AllDefines.Contains(i.define) )
			{
				Debug.LogError("Define words Error.");
				return;
			}
			defineWritedList.Add(string.Format("{0}{1}{2}", defineHead, i.define, "\n"));
		}
		
		Utility_GlobalDefineManager.WriteToLocalFile(SMCS_FilePath, defineWritedList);
		
		isSaveSuccess = true;

		SaveJson();
	}

	void SaveJson()
	{
		GlobalDefineData date = new GlobalDefineData();
		date.AllDefines = globalDefineData.AllDefines;

		foreach(var i in GlobalDefineItemList)
		{
			date.CurDefines.Add(i.define);
		}
		date.AllDefinesForShowList = null;


		Dictionary<string, object> jsonRoot = new Dictionary<string, object>();
		jsonRoot.Add("ALL_DEFINES", date.AllDefines);
		jsonRoot.Add("CUR_DEFINES", date.CurDefines);

		string jsonStr = MiniJSON.Json.Serialize(jsonRoot);
		Utility_GlobalDefineManager.WriteToLocalFile(GlobalDefineDataPath, jsonStr);
	}

	void Refresh()
	{
		OnDestroy();
		InitData();
	}
}

namespace GlobalDefine
{
	public class GlobalDefineItem
	{
		public string define;
	}

	public class Utility_GlobalDefineManager
	{
		public static string ReadTextFileFromLocal(string path ,string name, string jsonDamo)
		{
			string srText = string.Empty;
			string filePath = path + "/" + name;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (!File.Exists(filePath))
			{
				WriteToLocalFile(filePath, jsonDamo);
			}

			StreamReader sr = File.OpenText(filePath);
			string strTemp = string.Empty;
			while ((strTemp = sr.ReadLine()) != null)
			{
				srText += strTemp;
			}
			sr.Close();
			
			return srText;
		}
		
		
		public static void WriteToLocalFile(string filePath, List<string> writeList)
		{
			if (string.IsNullOrEmpty(filePath) || writeList == null)
			{
				Debug.LogError("write file path is null or write list is null.");
				return;
			}
			
			StringBuilder sb = new StringBuilder();
			
			foreach (var i in writeList)
			{
				sb.Append(i);
			}
			
			WriteToLocalFile(filePath, sb.ToString());
		}

		public static void WriteToLocalFile(string filePath, string data)
		{
			if (string.IsNullOrEmpty(filePath) || data == null)
			{
				Debug.LogError("write file path is null or write data is null.");
				return;
			}

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			File.WriteAllText(filePath, data);
		}
	}
	
	public class ParseJson
	{
		public static GlobalDefineData GetGlobalDefineData(string path ,string name, string jsonDamo)
		{
			GlobalDefineData globalDefineData = new GlobalDefineData();
			
			//parse 
			string jsonStr = 
				Utility_GlobalDefineManager.ReadTextFileFromLocal(path, name, jsonDamo);
			if(string.IsNullOrEmpty(jsonStr))
			{
				Debug.LogError(string.Format("Error : {0} is not Exists.",  name));
				return null;
			}
			
			Dictionary<string, object> jsonRoot = MiniJSON.Json.Deserialize(jsonStr) as Dictionary<string, object>;

			if (jsonRoot.ContainsKey("ALL_DEFINES"))
			{
				foreach(var i in jsonRoot["ALL_DEFINES"] as List<object>)
				{
					globalDefineData.AllDefines.Add(i.ToString());
				}
			}

			if (jsonRoot.ContainsKey("CUR_DEFINES"))
			{
				foreach(var i in jsonRoot["CUR_DEFINES"] as List<object>)
				{
					globalDefineData.CurDefines.Add(i.ToString());
				}
			}
						
			return globalDefineData;
		}
	}
	
	
	public class GlobalDefineData
	{
		public List<string> AllDefines;
		public List<string> CurDefines;
		public List<string> AllDefinesForShowList;
		public GlobalDefineData()
		{
			AllDefines = new List<string>();
			CurDefines = new List<string>();
			AllDefinesForShowList = new List<string>();
		}
		
		public void ShowDefinesInfo()
		{
			for(int i = 0; i < AllDefines.Count; i++)
			{
				string go = string.Empty;
				if(CurDefines.Contains(AllDefines[i]))
				{
					go = string.Format("{0} {1}", "->", AllDefines[i]);
				}
				else
				{
					go = string.Format("{0} {1}", "     ", AllDefines[i]);
				}

				AllDefinesForShowList.Add(go);
			}
		}
	}
}




