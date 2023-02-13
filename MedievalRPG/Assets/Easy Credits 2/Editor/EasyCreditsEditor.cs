#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using TMPro;

namespace Onerat.EasyCredits
{
    [CustomEditor(typeof(EasyCreditsDataManager))]
    public class EasyCreditsEditor : Editor
    {
        public Texture oneratLogo;
        [SerializeField]
        private bool togglePresets ,toggleCentering, toggleSectionSpacing, toggleGameTitle, toggleRoles, toggleTitles, toggleSettings, toggleLeagal, toggleInlays, togglePreview, ToggleDataFields, toggleAdvanced;
        public string CSV_Name;
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
            EasyCreditsDataManager easyCredits = (EasyCreditsDataManager)target;

            Rect space = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("", GUIStyle.none, GUILayout.Height(180));
            EditorGUILayout.EndHorizontal();
            GUI.Button(space, texture, styleTop);

            //start box
            GUILayout.BeginVertical("HelpBox");

            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Preview</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (togglePreview)
                    togglePreview = false;
                else
                    togglePreview = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (togglePreview)
            {
                SerializedProperty EnablePreview = serializedObject.FindProperty("EnablePreview");
                EditorGUILayout.PropertyField(EnablePreview);

                SerializedProperty PreviewScroll = serializedObject.FindProperty("PreviewScroll");
                EditorGUILayout.PropertyField(PreviewScroll);
            }

           
            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>General</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleSectionSpacing)
                    toggleSectionSpacing = false;
                else
                    toggleSectionSpacing = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleSectionSpacing)
            {
                if (easyCredits.DataFormat == EasyCreditsDataManager.DataFormatType.CSVCommaSeparatedValues)
                {
                    SerializedProperty CVSfileName = serializedObject.FindProperty("fileName");
                    EditorGUILayout.PropertyField(CVSfileName);
                }

                SerializedProperty DataFormat = serializedObject.FindProperty("DataFormat");
                EditorGUILayout.PropertyField(DataFormat);             

                SerializedProperty backgroundColor = serializedObject.FindProperty("backgroundColor");
                EditorGUILayout.PropertyField(backgroundColor);

                SerializedProperty scrollSpeed = serializedObject.FindProperty("scrollSpeed");
                EditorGUILayout.PropertyField(scrollSpeed);

                SerializedProperty SectionLineSpacing = serializedObject.FindProperty("SectionLineSpacing");
                EditorGUILayout.PropertyField(SectionLineSpacing);

                SerializedProperty AdvancedOptions = serializedObject.FindProperty("AdvancedOptions");
                EditorGUILayout.PropertyField(AdvancedOptions);
            }

            if(easyCredits.DataFormat == EasyCreditsDataManager.DataFormatType.ECSEEasyCreditSectionElements)
            {
                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("<color=white>Section Elements</color>", style);
                if (GUILayout.Button("Show/Hide", styleButton))
                {
                    if (ToggleDataFields)
                        ToggleDataFields = false;
                    else
                        ToggleDataFields = true;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (ToggleDataFields)
                {
                    ReorderableListUtility.DoLayoutListWithFoldout(this.DataFields);
                }
            }
            

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Centering</color>", style);
            if(GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleCentering)
                    toggleCentering = false;
                else
                    toggleCentering = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleCentering)
            {
                SerializedProperty TextCentering = serializedObject.FindProperty("TextCentering");
                EditorGUILayout.PropertyField(TextCentering);
        
                SerializedProperty KeepTitleCentered = serializedObject.FindProperty("KeepTitleCentered");
                EditorGUILayout.PropertyField(KeepTitleCentered);

                if(easyCredits.KeepTitleCentered == false)
                {
                    SerializedProperty TitleLogoCentering = serializedObject.FindProperty("TitleLogoCentering");
                    EditorGUILayout.PropertyField(TitleLogoCentering);
                }

                SerializedProperty creditType = serializedObject.FindProperty("creditType");
                EditorGUILayout.PropertyField(creditType);

                if (easyCredits.creditType == EasyCreditsDataManager.CreditType.Cinematic)
                {
                    SerializedProperty cinematicRoleCentering = serializedObject.FindProperty("cinematicRoleCentering");
                    EditorGUILayout.PropertyField(cinematicRoleCentering);
                }
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Game Title</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleGameTitle)
                    toggleGameTitle = false;
                else
                    toggleGameTitle = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleGameTitle)
            {
                SerializedProperty GameTitleImage = serializedObject.FindProperty("GameTitleImage");
                EditorGUILayout.PropertyField(GameTitleImage);

                SerializedProperty gameTitleScale = serializedObject.FindProperty("gameTitleScale");
                EditorGUILayout.PropertyField(gameTitleScale);

                SerializedProperty GameTitle = serializedObject.FindProperty("GameTitle");
                EditorGUILayout.PropertyField(GameTitle);

                SerializedProperty gameTitleFont = serializedObject.FindProperty("gameTitleFont");
                EditorGUILayout.PropertyField(gameTitleFont);

                SerializedProperty roleAnchor = serializedObject.FindProperty("gameTitleAnchor");
                EditorGUILayout.PropertyField(roleAnchor);

                SerializedProperty GameTitleLineSpacing = serializedObject.FindProperty("GameTitleLineSpacing");
                EditorGUILayout.PropertyField(GameTitleLineSpacing);

                SerializedProperty GameTitlefontSize = serializedObject.FindProperty("GameTitlefontSize");
                EditorGUILayout.PropertyField(GameTitlefontSize);

                SerializedProperty GameTitleWordSpacing = serializedObject.FindProperty("GameTitleWordSpacing");
                EditorGUILayout.PropertyField(GameTitleWordSpacing);

                SerializedProperty GameTitleCharacterSpacing = serializedObject.FindProperty("GameTitleCharacterSpacing");
                EditorGUILayout.PropertyField(GameTitleCharacterSpacing);

                SerializedProperty GameTitleColor = serializedObject.FindProperty("GameTitleColor");
                EditorGUILayout.PropertyField(GameTitleColor);
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Roles</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleRoles)
                    toggleRoles = false;
                else
                    toggleRoles = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleRoles)
            {
                SerializedProperty roleFont = serializedObject.FindProperty("roleFont");
                EditorGUILayout.PropertyField(roleFont);

                SerializedProperty roleAnchor = serializedObject.FindProperty("roleAnchor");
                EditorGUILayout.PropertyField(roleAnchor);

                SerializedProperty RoleLineSpacing = serializedObject.FindProperty("RoleLineSpacing");
                EditorGUILayout.PropertyField(RoleLineSpacing);

                SerializedProperty RolefontSize = serializedObject.FindProperty("RolefontSize");
                EditorGUILayout.PropertyField(RolefontSize);

                SerializedProperty RoleWordSpacing = serializedObject.FindProperty("RoleWordSpacing");
                EditorGUILayout.PropertyField(RoleWordSpacing);

                SerializedProperty RoleCharacterSpacing = serializedObject.FindProperty("RoleCharacterSpacing");
                EditorGUILayout.PropertyField(RoleCharacterSpacing);

                SerializedProperty roleColor = serializedObject.FindProperty("roleColor");
                EditorGUILayout.PropertyField(roleColor);
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Titles</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleTitles)
                    toggleTitles = false;
                else
                    toggleTitles = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleTitles)
            {
                SerializedProperty titleFont = serializedObject.FindProperty("titleFont");
                EditorGUILayout.PropertyField(titleFont);

                SerializedProperty titleAnchor = serializedObject.FindProperty("titleAnchor");
                EditorGUILayout.PropertyField(titleAnchor);

                SerializedProperty TitleLineSpacing = serializedObject.FindProperty("TitleLineSpacing");
                EditorGUILayout.PropertyField(TitleLineSpacing);

                SerializedProperty TitlefontSize = serializedObject.FindProperty("TitlefontSize");
                EditorGUILayout.PropertyField(TitlefontSize);

                SerializedProperty TitleWordSpacing = serializedObject.FindProperty("TitleWordSpacing");
                EditorGUILayout.PropertyField(TitleWordSpacing);

                SerializedProperty TitleCharacterSpacing = serializedObject.FindProperty("TitleCharacterSpacing");
                EditorGUILayout.PropertyField(TitleCharacterSpacing);

                SerializedProperty titleColor = serializedObject.FindProperty("titleColor");
                EditorGUILayout.PropertyField(titleColor);
            }

            //end first grouping
            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Inlays</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleInlays)
                    toggleInlays = false;
                else
                    toggleInlays = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleInlays)
            {
                EditorGUI.BeginChangeCheck();
                SerializedProperty inlayType = serializedObject.FindProperty("inlayType");
                EditorGUILayout.PropertyField(inlayType);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateList();
                }


                if (easyCredits.inlayType == EasyCreditsDataManager.InlayType.Stationary || easyCredits.inlayType == EasyCreditsDataManager.InlayType.StationaryAndScrollingInline)
                {
                    SerializedProperty InlayPostionLeftRight = serializedObject.FindProperty("InlayPostionLeftRight");
                    EditorGUILayout.PropertyField(InlayPostionLeftRight);

                    SerializedProperty InlayPostionUpDown = serializedObject.FindProperty("InlayPostionUpDown");
                    EditorGUILayout.PropertyField(InlayPostionUpDown);

          
                        ReorderableListUtility.DoLayoutListWithFoldout(this.InlayListTwo);
                    
                }
                if(easyCredits.inlayType != EasyCreditsDataManager.InlayType.None && easyCredits.inlayType != EasyCreditsDataManager.InlayType.Stationary)
                {
                    ReorderableListUtility.DoLayoutListWithFoldout(this.InlayList);

                }
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Begin/End</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleSettings)
                    toggleSettings = false;
                else
                    toggleSettings = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleSettings)
            {
                SerializedProperty startDelay = serializedObject.FindProperty("startDelay");
                EditorGUILayout.PropertyField(startDelay);

                SerializedProperty startPointOffset = serializedObject.FindProperty("startPointOffset");
                EditorGUILayout.PropertyField(startPointOffset);

                SerializedProperty exitTime = serializedObject.FindProperty("exitTime");
                EditorGUILayout.PropertyField(exitTime);

                SerializedProperty ExitToSceneOnEnd = serializedObject.FindProperty("ExitToSceneOnEnd");
                EditorGUILayout.PropertyField(ExitToSceneOnEnd);

                if(easyCredits.ExitToSceneOnEnd)
                {
                    SerializedProperty SceneToExitTo = serializedObject.FindProperty("SceneToExitTo");
                    EditorGUILayout.PropertyField(SceneToExitTo);
                }

                EditorGUILayout.Space();

                SerializedProperty OnCreditsStart = serializedObject.FindProperty("OnCreditsStart");
                EditorGUILayout.PropertyField(OnCreditsStart);

                SerializedProperty OnCreditsEnd = serializedObject.FindProperty("OnCreditsEnd");
                EditorGUILayout.PropertyField(OnCreditsEnd);
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Legal Line</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (toggleLeagal)
                    toggleLeagal = false;
                else
                    toggleLeagal = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (toggleLeagal)
            {
                SerializedProperty KeepLegalLineCentred = serializedObject.FindProperty("KeepLegalLineCentred");
                EditorGUILayout.PropertyField(KeepLegalLineCentred);

                SerializedProperty legalAnchor = serializedObject.FindProperty("legalAnchor");
                EditorGUILayout.PropertyField(legalAnchor);

                SerializedProperty LegalFont = serializedObject.FindProperty("LegalFont");
                EditorGUILayout.PropertyField(LegalFont);
                
                SerializedProperty LegalFontSize = serializedObject.FindProperty("LegalFontSize");
                EditorGUILayout.PropertyField(LegalFontSize);

                SerializedProperty LegalWordSpacing = serializedObject.FindProperty("LegalWordSpacing");
                EditorGUILayout.PropertyField(LegalWordSpacing);

                SerializedProperty LegalCharacterSpacing = serializedObject.FindProperty("LegalCharacterSpacing");
                EditorGUILayout.PropertyField(LegalCharacterSpacing);

                SerializedProperty LegalLineSpacing = serializedObject.FindProperty("LegalLineSpacing");
                EditorGUILayout.PropertyField(LegalLineSpacing);

                SerializedProperty LegalColor = serializedObject.FindProperty("LegalColor");
                EditorGUILayout.PropertyField(LegalColor);

                SerializedProperty LegalLine = serializedObject.FindProperty("LegalLine");
                EditorGUILayout.PropertyField(LegalLine);
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Presets</color>", style);
            if (GUILayout.Button("Show/Hide", styleButton))
            {
                if (togglePresets)
                    togglePresets = false;
                else
                    togglePresets = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (togglePresets)
            {
                GUILayout.BeginVertical("GroupBox");
                EditorGUILayout.LabelField("<color=white>General</color>", stylePreset);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button(" Scrolling Credits ", stylePresetButton))
                {
                    easyCredits.SectionLineSpacing = 20;
                    easyCredits.RoleLineSpacing = 10;
                    easyCredits.TitleLineSpacing = 5;
                    easyCredits.RolefontSize = 22;
                    easyCredits.TitlefontSize = 18;
                    easyCredits.startPointOffset = 0;
                    easyCredits.scroll = true;
                    easyCredits.GameTitleLineSpacing = 70;
                    easyCredits.exitTime = 0;
                    easyCredits.PreviewPresetUpdate();
                }
                if (GUILayout.Button("Stationary Credits", stylePresetButton))
                {
                    easyCredits.SectionLineSpacing = 3;
                    easyCredits.RoleLineSpacing = 6;
                    easyCredits.TitleLineSpacing = 4;
                    easyCredits.RolefontSize = 17;
                    easyCredits.RoleLineSpacing = 5;
                    easyCredits.TitlefontSize = 13;
                    easyCredits.startPointOffset = 45;
                    easyCredits.GameTitleLineSpacing = 20;
                    easyCredits.LegalLineSpacing = 0;
                    easyCredits.scroll = false;
                    easyCredits.exitTime = 10;
                    easyCredits.inlayType = EasyCreditsDataManager.InlayType.None;

                    easyCredits.creditType = EasyCreditsDataManager.CreditType.Cinematic;
                    easyCredits.roleAnchor = TextAlignmentOptions.Right;
                    easyCredits.titleAnchor = TextAlignmentOptions.Left;
                    easyCredits.TextCentering = 0.15f;
                    easyCredits.TitleLogoCentering = -0.15f;
                    easyCredits.PreviewPresetUpdate();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                EditorGUILayout.LabelField("<color=white>Roles</color>", stylePreset);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Cinematic Roles", stylePresetButton))
                {
                    easyCredits.creditType = EasyCreditsDataManager.CreditType.Cinematic;
                    easyCredits.roleAnchor = TextAlignmentOptions.Right;
                    easyCredits.titleAnchor = TextAlignmentOptions.Left;
                    easyCredits.TextCentering = 0.15f;
                    easyCredits.TitleLogoCentering = -0.15f;
                    easyCredits.PreviewPresetUpdate();
                }
                if (GUILayout.Button("    Inline Roles    ", stylePresetButton))
                {
                    easyCredits.creditType = EasyCreditsDataManager.CreditType.Inline;
                    easyCredits.roleAnchor = TextAlignmentOptions.Center;
                    easyCredits.titleAnchor = TextAlignmentOptions.Center;
                    easyCredits.TextCentering = 0f;
                    easyCredits.TitleLogoCentering = 0f;
                    easyCredits.PreviewPresetUpdate();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                EditorGUILayout.LabelField("<color=white>Inlays</color>", stylePreset);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("        Inlays Inline        ", stylePresetButton))
                {
                    easyCredits.inlayType = EasyCreditsDataManager.InlayType.ScrollingInline;
                    easyCredits.PreviewPresetUpdate();

                }
                if (GUILayout.Button("Inlay Side by Side Roles", stylePresetButton))
                {
                    easyCredits.inlayType = EasyCreditsDataManager.InlayType.ScrollingSideBySideRole;
                    easyCredits.TextCentering = -.3f;
                    easyCredits.TitleLogoCentering = .3f;
                    easyCredits.PreviewPresetUpdate();
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Stationary Inlays left of credits", stylePresetButton))
                {
                    easyCredits.inlayType = EasyCreditsDataManager.InlayType.Stationary;
                    easyCredits.TextCentering = 0.4f;
                    easyCredits.TitleLogoCentering = -0.4f;
                    easyCredits.InlayPostionLeftRight = 0.3f;
                    easyCredits.PreviewPresetUpdate();
                }
                if (GUILayout.Button("Stationary Inlays right of credits", stylePresetButton))
                {
                    easyCredits.inlayType = EasyCreditsDataManager.InlayType.Stationary;
                    easyCredits.TextCentering = -0.4f;
                    easyCredits.TitleLogoCentering = 0.4f;
                    easyCredits.InlayPostionLeftRight = 0.7f;
                    easyCredits.PreviewPresetUpdate();
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }

            if(easyCredits.AdvancedOptions)
            {
                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("<color=white>Advanced Options</color>", style);
                if (GUILayout.Button("Show/Hide", styleButton))
                {
                    if (toggleAdvanced)
                        toggleAdvanced = false;
                    else
                        toggleAdvanced = true;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (toggleAdvanced)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("CanvasRenderMode"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraOrthographic"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Camera"));                   
                }
            }
            

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
        private ReorderableList InlayListTwo;
        private ReorderableList DataFields;

        private float?[] spacing = new float?[] { 180, 170, 80, 40 };

        private void OnEnable()
        {
            UpdateList();
        }

        private void UpdateList()
        {
            var inlays = this.serializedObject.FindProperty("Inlays");
            var stationaryInlays = this.serializedObject.FindProperty("StationaryInlays");
            var dataFields = this.serializedObject.FindProperty("SectionElements");

            EasyCreditsDataManager easyCredits = (EasyCreditsDataManager)target;

                this.InlayList = ReorderableListUtility.CreateAutoLayout(
                    inlays
                );
                this.InlayListTwo = ReorderableListUtility.CreateAutoLayout(
                    stationaryInlays
                );
                this.DataFields = ReorderableListUtility.CreateAutoLayout(
                     dataFields
                 );
        }
    }
}
#endif