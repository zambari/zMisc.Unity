using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

  //  [CustomEditor(typeof(CurveStore))]
    public class StyleStoreEditor : Editor
    {
        const int BUTTONWIDTH = 40;
        string[] SheetNames;
        //CurveStore StyleStore;


        /// <summary>
        /// Main style editing function
        /// </summary>

       /* bool editCurrentSheet()
        {
            bool changed = false;
    //        List<Style> currentSheet = StyleStore.currentStyleSheet.styles;
            for (int i = 0; i < currentSheet.Count; i++)
            {
                Style thisStyle = currentSheet[i];
                Separator();
                EditorGUILayout.BeginHorizontal();
                thisStyle.editorOptionsExpanded = EditorGUILayout.Foldout(thisStyle.editorOptionsExpanded, thisStyle.styleName, true);
                GUILayout.FlexibleSpace();
                Color c = EditorGUILayout.ColorField(thisStyle.color, GUILayout.Width(120));
                if (thisStyle.color != c)
                {
                    thisStyle.color = c;
                    changed = true;
                }
                if (GUILayout.Button("Copy", EditorStyles.miniButton))
                    copiedColor = thisStyle.color;
                if (GUILayout.Button("Paste", EditorStyles.miniButton))
                {
                    Undo.RecordObject(StyleStore, "Pasted Color");
                    thisStyle.color = copiedColor;
                    changed = true;
                }
                EditorGUILayout.EndHorizontal();
                if (thisStyle.editorOptionsExpanded)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (thisStyle.useFontSize && editFontSize(ref thisStyle.fontSize))
                        changed = true;
                    {
                        bool b = GUILayout.Toggle(thisStyle.useFontSize, "font size", GUILayout.Width(70));
                        if (b != thisStyle.useFontSize)
                        {
                            Undo.RecordObject(StyleStore, "Use FontColor");
                            thisStyle.useFontSize = b;
                            changed = true;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    addSpace();
                    addSpace();
                }
            }
            return changed;
        }

        /// <summary>
        /// Draws top dropdown with two extra buttons
        /// </summary>
        bool drawSelectionDropdown()
        {
            bool changed = false;
            SheetNames = new string[StyleStore.styleSheets.Count + 2];
            for (int i = 2; i < SheetNames.Length; i++)
            {
                SheetNames[i] = StyleStore.styleSheets[i - 2].SheetName;
            }
            SheetNames[0] = "Edit Style Sheets (Variants)";
            SheetNames[1] = "Edit Style Element definitions";

            GUILayout.Space(10);
            int styleIndex = (StyleStore.currentStyleIndex + 2);
            if (StyleStore.isEditingSheets) styleIndex = 0;
            if (StyleStore.isEditingStyles) styleIndex = 1;

            styleIndex = EditorGUILayout.Popup(styleIndex, SheetNames);
            if (styleIndex == 0)
            {
                StyleStore.isEditingSheets = true;
                StyleStore.isEditingStyles = false;
            }
            else
            if (styleIndex == 1)
            {
                StyleStore.isEditingSheets = false;
                StyleStore.isEditingStyles = true;
            }
            else
            {
                StyleStore.isEditingSheets = false;
                StyleStore.isEditingStyles = false;
                if (StyleStore.currentStyleIndex != styleIndex - 2)
                {
                    Undo.RecordObject(StyleStore, "StyleSheet / View changed");
                    StyleStore.currentStyleIndex = styleIndex - 2;
                    changed = true;
                }
            }
            return changed;
        }
        void editStyleNames()
        {
            bool showRemove = StyleStore.currentStyleSheet.styles.Count > 1;
            GUILayout.Label("Define style names here", EditorStyles.boldLabel);
            for (int i = 0; i < StyleStore.currentStyleSheet.styles.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string currentName = StyleStore.getStyleName(i);
                string newName = EditorGUILayout.TextField(currentName);
                if (newName != currentName)
                {
                    Undo.RecordObject(StyleStore, "Renamed Style");
                    StyleStore.setStyleName(i, newName);
                    Repaint();
                }
                if (GUILayout.Button("▲", EditorStyles.miniButton))
                {
                    if (i > 0)
                    {
                        Undo.RecordObject(StyleStore, "Moved Style");
                        StyleStore.MoveStyle(i, i - 1);
                        Repaint();
                        return;
                    }
                }
                if (GUILayout.Button("▼", EditorStyles.miniButton))
                {
                    if (i < StyleStore.currentStyleSheet.styles.Count - 1)
                    {
                        Undo.RecordObject(StyleStore, "Moved Style");
                        StyleStore.MoveStyle(i, i + 1);
                        Repaint();
                        return;
                    }
                }
                if (GUILayout.Button("Clone", EditorStyles.miniButton))
                {
                    Undo.RecordObject(StyleStore, "Cloned Sheet");
                    StyleStore.DuplicateStyle(i);
                    return;
                }
                if (showRemove && GUILayout.Button("X", EditorStyles.miniButton))
                {
                    Undo.RecordObject(StyleStore, "Removed Sheet");
                    StyleStore.RemoveStyle(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        void editSheetNames()
        {
            bool showRemove = StyleStore.styleSheets.Count > 1;
            GUILayout.Label("Define Sheet names here", EditorStyles.boldLabel);
            for (int i = 0; i < StyleStore.styleSheets.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                StyleStore.styleSheets[i].SheetName = EditorGUILayout.TextField(StyleStore.styleSheets[i].SheetName);

                if (GUILayout.Button("▲", EditorStyles.miniButton))
                    if (i > 0)
                    {
                        Undo.RecordObject(StyleStore, "Moved styleSheet");
                        StyleStore.MoveSheet(i, i - 1);
                    }
                if (GUILayout.Button("▼", EditorStyles.miniButton))
                    if (i < StyleStore.styleSheets.Count - 1)
                    {
                        Undo.RecordObject(StyleStore, "Moved styleSheet");
                        StyleStore.MoveSheet(i, i + 1);
                    }
                if (GUILayout.Button("Clone", EditorStyles.miniButton))
                {
                    Undo.RecordObject(StyleStore, "Cloned Sheet");
                    StyleStore.DuplicateSheet(i);
                }
                if (showRemove && GUILayout.Button("X", EditorStyles.miniButton))
                {
                    Undo.RecordObject(StyleStore, "Removed Sheet");
                    StyleStore.styleSheets.RemoveAt(i);

                }
                if (GUILayout.Button("Edit", EditorStyles.miniButton))
                {
                    StyleStore.currentStyleIndex = i;
                    StyleStore.isEditingSheets = false;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        void addSpace()
        {
            GUILayout.Space(6);
        }
        void updateTarget()
        {
            if (StyleStore.OnStyleChange != null)
                StyleStore.OnStyleChange.Invoke();
            //if (target != null)
            EditorUtility.SetDirty(StyleStore.instance);
        }
        void Separator()
        {
            // ALTERNATIVE LINES
            //GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        /// <summary>
        /// Shows a [+][-] section
        /// </summary>
        /// <param name="fontSize"></param>

        bool editFontSize(ref int fontSize)
        {
            bool changed = false;
            EditorGUILayout.BeginHorizontal();
            int currentSize = fontSize;

            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(22)))
            {
                Undo.RecordObject(StyleStore, "Size Changed");
                currentSize++;
                changed = true;
            }
            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(22)))
            {
                Undo.RecordObject(StyleStore, "Size changed");
                currentSize--;
                changed = true;
            }
            currentSize = EditorGUILayout.IntField(currentSize, GUILayout.Width(50));
            GUILayout.Label("  ", GUILayout.Width(27));
            EditorGUILayout.EndHorizontal();

            if (fontSize != currentSize)
            {
                fontSize = currentSize;
                if (fontSize < 3) fontSize = 3;
                changed = true;
            }
            return changed;
        }*/

        /// <summary>
        /// Inspector gui
        /// </summary>

        public override void OnInspectorGUI()
        {
           // StyleStore = target as CurveStore;
          //  GUILayout.Space(5);
          /*  drawSelectionDropdown();
            GUILayout.Space(15);
            if (StyleStore.isEditingSheets)
                editSheetNames();
            else
            if (StyleStore.isEditingStyles)
            {
                editStyleNames();
            }
            else
            {
                if (editCurrentSheet())
                    updateTarget();
            }*/
         //   GUILayout.Space(60);
          DrawDefaultInspector();
        }
    }