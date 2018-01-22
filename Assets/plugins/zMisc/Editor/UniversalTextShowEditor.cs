using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Reflection;

[CustomEditor(typeof(UniversalTextShow))]
public class UniversalTextShowEditor : Editor
{
    List<string> monoMethods;

    public override void OnInspectorGUI()
    {
        UniversalTextShow textShow = (UniversalTextShow)target;
        GUILayout.Label("Select variable type", EditorStyles.boldLabel);
            textShow.sourceValueType = (UniversalTextShow.SourceValueTypes)EditorGUILayout.Popup((int)textShow.sourceValueType, System.Enum.GetNames(typeof(UniversalTextShow.SourceValueTypes)));

        GUILayout.Label("Assign source object and component", EditorStyles.boldLabel);
        if (monoMethods == null)
        {
            Type gameObjectType = textShow.gameObject.GetType();
            MemberInfo[] objMember = gameObjectType.GetMembers();
            monoMethods = new List<string>();
            for (int i = 0; i < objMember.Length; i++)
                monoMethods.Add(objMember[i].Name);
        }

        textShow.targetGameObject = (GameObject)EditorGUILayout.ObjectField(textShow.targetGameObject, typeof(GameObject), true);


        if (textShow.targetGameObject != null)
        {
            Component[] components = textShow.targetGameObject.GetComponents<Component>();
            //=EditorGUI.DropdownButton()
            string[] componentNames = new string[components.Length];
            int selectedComponentIndex = 0;
            for (int i = 0; i < componentNames.Length; i++)
            {
                if (textShow.sourceComponent == components[i])
                    selectedComponentIndex = i;
                componentNames[i] = components[i].GetType().ToString();
            }
            int selectedComponentIndex2 = EditorGUILayout.Popup(selectedComponentIndex, componentNames);
            if (selectedComponentIndex != selectedComponentIndex2)
                textShow.sourceComponent = components[selectedComponentIndex2];

            Type objType = textShow.sourceComponent.GetType();


            textShow.showPrivate = GUILayout.Toggle(textShow.showPrivate, "List Private fields");
            textShow.showVariableName = GUILayout.Toggle(textShow.showVariableName, "Show Variable Name");


            if (textShow.showVariableName)
            {
                textShow.newLineAfterVarName = GUILayout.Toggle(textShow.newLineAfterVarName, "New Line after Var Break");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Label font size");
                textShow.varNameSize = EditorGUILayout.IntField(textShow.varNameSize);
                GUILayout.EndHorizontal();
            }

            //MemberInfo[] objMember = (textShow.showPrivate? obj.GetMembers( BindingFlags.NonPublic ) : obj.GetMembers());
        
            FieldInfo[] objFields = (textShow.showPrivate ? objType.GetFields(UniversalTextShow.bindingLibrealFlag) : objType.GetFields());
            PropertyInfo[] objProps = (textShow.showPrivate ? objType.GetProperties(UniversalTextShow.bindingLibrealFlag) : objType.GetProperties());
            if (objFields.Length > 0)
                GUILayout.Label(" Fields ", EditorStyles.boldLabel);
            for (int i = 0; i < objFields.Length; i++)
            {
                {
                    String name = objFields[i].Name;

                    //	
                    string memberType = objFields[i].MemberType.ToString();
                    if (memberType == "Field")
                        if ((objFields[i].FieldType == typeof(float) && textShow.sourceValueType == UniversalTextShow.SourceValueTypes.Float)
                         || (objFields[i].FieldType == typeof(string) && textShow.sourceValueType == UniversalTextShow.SourceValueTypes.String)
                        )
                        {
                            string value = objFields[i].GetValue(textShow.sourceComponent).ToString();
                            bool ticked = false;
                            if (textShow.sourceType == UniversalTextShow.SourceTypes.Field)
                            {
                                ticked = (textShow.sourceName == name);
                            }
                            bool thisToggle = GUILayout.Toggle(ticked, name + " (" + value + ")");
                            if (thisToggle)
                            {
                                textShow.sourceName = name;
                                textShow.sourceType = UniversalTextShow.SourceTypes.Field;
                            }


                        }
                }
            }
            GUILayout.Label("");
            if (objProps.Length > 0)
                GUILayout.Label(" Properties ", EditorStyles.boldLabel);

            for (int i = 0; i < objProps.Length; i++)
            {
                // if (!monoMethods.Contains(objMember[i].Name))
                {
                    String name = objProps[i].Name;
                    if (name != "useGUILayout")
                    {
                        if ((objProps[i].PropertyType == typeof(float) && textShow.sourceValueType == UniversalTextShow.SourceValueTypes.Float)
                        || (objProps[i].PropertyType == typeof(string) && textShow.sourceValueType == UniversalTextShow.SourceValueTypes.String)
                        )
                        {
                            string memberType = objProps[i].MemberType.ToString();

                            string value = (objProps[i].GetValue(textShow.sourceComponent, null)).ToString();

                            bool ticked = false;
                            if (textShow.sourceType == UniversalTextShow.SourceTypes.Property)
                            {
                                ticked = (textShow.sourceName == name);
                            }
                            bool thisToggle = GUILayout.Toggle(ticked, name + " (" + value + ")");
                            if (thisToggle)
                            {
                                textShow.sourceName = name;
                                textShow.sourceType = UniversalTextShow.SourceTypes.Property;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        GUILayout.Label("CurrentcValue", EditorStyles.boldLabel);
        GUILayout.Label(textShow.GetCurrentValue().ToString());
        //    GUILayout.Space(60);
        //     GUILayout.Label("Default Inspector");
        //      GUILayout.Space(20);
        //        DrawDefaultInspector();

    }




}
