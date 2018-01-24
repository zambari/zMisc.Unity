using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(CurveDrawer))]
public class CurveDrawerDrawer : PropertyDrawer
{
    const int curveSize = 30;

    const int defSize = 16;
    bool expanded;
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
         EditorGUI.BeginProperty(rect, label, property);
        //	GUILayout.Label("Hello world");

        Rect nameRect = new Rect(rect.x, rect.y, rect.width, defSize);
        Rect curveRect = new Rect(rect.x, rect.y + defSize, rect.width, curveSize);
        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative ("curveName"), GUIContent.none);
        string name = property.FindPropertyRelative("curveName").stringValue;
        EditorGUI.LabelField(nameRect, name);

        EditorGUI.PropertyField(curveRect, property.FindPropertyRelative("curve"), GUIContent.none);
        if (CurveStore.instance == null)
        {
            GUILayout.Label("No Curve Store On Scene");
            expanded = false;
            return;
        }
        expanded = GUILayout.Toggle(expanded, "Expand Selector");
        if (expanded)
        {
			for (int i=0;i<CurveStore.curveCount;i++)
			{

				if (GUILayout.Button(CurveStore.instance.curveNames[i],EditorStyles.miniButton))
				{
		
					property.FindPropertyRelative("curve").animationCurveValue=CurveStore.getCurve(i);
				}

			}
		}

       EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

       /* if (expanded)
        {
            return curveSize + defSize + defSize * CurveStore.curveCount;

		}
        else*/	
		 return curveSize + defSize;
    }

}
