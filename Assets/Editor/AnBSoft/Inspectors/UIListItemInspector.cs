//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEditor;
using UnityEngine;
using System.Collections;


// Only compile if not using Unity iPhone
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_3)
[CustomEditor(typeof(UIListItem))]
#endif
public class UIListItemInspector : UICtlInspector
{
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_3)
	public override void DrawPrestateSettings()
	{
		base.DrawPrestateSettings();

		BeginMonitorChanges();

		UIListItem li = (UIListItem)control;

		li.activeOnlyWhenSelected = EditorGUILayout.Toggle("Active Only When Selected", li.activeOnlyWhenSelected);

		EndMonitorChanges();
	}
#endif
}