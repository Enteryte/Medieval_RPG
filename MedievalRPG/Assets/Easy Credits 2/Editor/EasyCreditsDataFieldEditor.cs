#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using TMPro;

namespace Onerat.EasyCredits
{
    [CustomEditor(typeof(EasyCreditsSectionElement))]
    public class EasyCreditsSectionElementEditor : Editor
    {
        public Texture oneratLogo;
        [SerializeField] private bool toggleDataField;

        public override void OnInspectorGUI()
        {
            var texture = Resources.Load<Texture2D>("Tool_creadits_logo");
            var button_texture = Resources.Load<Texture2D>("EasyCreditsButtonTexture");
            var button_clicked_texture = Resources.Load<Texture2D>("EasyCreditsButtonClickedTexture");

            //define label styling
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 15, wordWrap = true  }; style.richText = true;
            var stylePreset = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12, wordWrap = true }; stylePreset.richText = true;

            var styleButton = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, wordWrap = true, fixedWidth = 100, richText = true };
            var stylePresetButton = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.LowerCenter, fontSize = 12, wordWrap = true, richText = true};

            stylePresetButton.active.textColor = Color.black;
            stylePresetButton.active.background = button_clicked_texture;
            stylePresetButton.normal.textColor = Color.white;
            stylePresetButton.normal.background = button_texture;

            var styleTop = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }; styleTop.richText = true;
            EasyCreditsSectionElement easyCreditsDataField = (EasyCreditsSectionElement)target;

            //Rect space = EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextArea("", GUIStyle.none, GUILayout.Height(180));
            //EditorGUILayout.EndHorizontal();
            //GUI.Button(space, texture, styleTop);

            //start box
            GUILayout.BeginVertical("HelpBox");


            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Easy Credits Section Element</color>", style);
            //if (GUILayout.Button("Show/Hide", styleButton))
            //{
            //    if (toggleDataField)
            //        toggleDataField = false;
            //    else
            //        toggleDataField = true;
            //}
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            //if (toggleDataField) {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("SectionTitle"));
                ReorderableListUtility.DoLayoutListWithFoldout(this.InlayList);
            //}


            GUILayout.EndVertical();
            GUILayout.EndVertical();
            //end

            GUILayout.Label("Easy Credits 2.06 - Created By @oneratdylan", stylePreset);
            if (GUILayout.Button("Join the Onerat Discord", stylePresetButton)) {
                Application.OpenURL("https://discord.com/invite/oneratgames");
            }
            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();

        }

        private ReorderableList InlayList;
        //private float?[] spacing = new float?[] { 180, 170, 80, 40 };
       
        private void OnEnable()
        {
            UpdateList();
        }
       
        private void UpdateList()
        {
            var inlays = this.serializedObject.FindProperty("Content");
       
            EasyCreditsSectionElement easyCreditsDataField = (EasyCreditsSectionElement)target;
       
                this.InlayList = ReorderableListUtility.CreateAutoLayout(
                    inlays
                );
       
        }
    }
}
#endif
