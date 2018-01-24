using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
namespace zUI
{


    [CustomEditor(typeof(LayoutHelper))]

    public class LayoutHelperEditor : Editor
    {

        public override void OnInspectorGUI()
        {
			  DrawDefaultInspector ();
        }


    }
}