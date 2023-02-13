using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Onerat.EasyCredits
{
    public class EasyCreditsDataManager : MonoBehaviour
    {
        private EasyCreditsCanvasManager canvasManager => GetComponent<EasyCreditsCanvasManager>();
        public string fileName = "Credits";
        private TextAsset CSV;
        public string[] data;
        [HideInInspector] public bool startingPassComplete = false;

        public float startDelay = 1;
        public Color backgroundColor = Color.black;
        [Range(0, 10)] public int scrollSpeed = 2;

        [Range(-1.0f, 1.0f)] public float TextCentering = 0f;
        [Range(-1.0f, 1.0f)] public float TitleLogoCentering = 0f;
        public bool KeepTitleCentered = true;

        public Sprite GameTitleImage;
        public int gameTitleScale = 0;
        public string GameTitle;
        public TMP_FontAsset gameTitleFont;
        [Range(0.0f, 100.0f)] public float GameTitleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float GameTitleCharacterSpacing = 0;
        [Range(0.0f, 100.0f)] public int GameTitleLineSpacing = 10;
        [Range(0.0f, 50.0f)] public int GameTitlefontSize = 22;
        public Color GameTitleColor = Color.grey;
        [Space]
        [Range(0.0f, 50.0f)] public int SectionLineSpacing = 20;
        [Space]
        public TMP_FontAsset roleFont;
        [Range(0.0f, 100.0f)] public float RoleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float RoleCharacterSpacing = 0;
        [Range(0.0f, 50.0f)] public int RoleLineSpacing = 10;
        [Range(0.0f, 50.0f)] public int RolefontSize = 22;
        public Color roleColor = Color.grey;
        [Space]
        public TMP_FontAsset titleFont;
        [Range(0.0f, 100.0f)] public float TitleWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float TitleCharacterSpacing = 0;

        public TMP_FontAsset LegalFont;
        [Range(0.0f, 100.0f)] public float LegalWordSpacing = 0;
        [Range(0.0f, 100.0f)] public float LegalCharacterSpacing = 0;
        public Color LegalColor = Color.white;
        [Range(0.0f, 50.0f)] public int LegalFontSize = 18;

        [Range(0.0f, 50.0f)] public int TitleLineSpacing = 5;
        [Range(0.0f, 50.0f)] public int LegalLineSpacing = 5;
        [Range(0.0f, 50.0f)] public int TitlefontSize = 18;
        public Color titleColor = Color.white;
        [Space]
        [Range(-100.0f, 100.0f)] public int startPointOffset = 0;

        public bool ExitToSceneOnEnd = true;
        public float exitTime = 0;
        public int SceneToExitTo;

        public bool KeepLegalLineCentred;
        public string LegalLine = "Credits powered by Easy Credits";

        public bool EnablePreview;
        [Range(0.0f, 1.0f)] public float PreviewScroll = 0;

        public UnityEvent OnCreditsStart;
        public UnityEvent OnCreditsEnd;

        public InlayType inlayType;
        public List<InlayCase> Inlays = new List<InlayCase>();
        public List<StationaryInlayCase> StationaryInlays = new List<StationaryInlayCase>();

        [Range(-100.0f, 100.0f)] public int cinematicRoleCentering;
        public CreditType creditType;
        public enum CreditType { Inline, Cinematic }
        public enum InlayType { None, ScrollingInline, ScrollingSideBySideRole, ScrollingManual, Stationary, StationaryAndScrollingInline }

        [Range(0, 1f)] public float InlayPostionLeftRight, InlayPostionUpDown;

        public TextAlignmentOptions gameTitleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions roleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions titleAnchor = TextAlignmentOptions.Center;
        public TextAlignmentOptions legalAnchor = TextAlignmentOptions.Center;

        public enum DataFormatType { CSVCommaSeparatedValues, ECSEEasyCreditSectionElements }
        public DataFormatType DataFormat;

        public List<SectionElementCase> SectionElements = new List<SectionElementCase>();
        [System.Serializable] public class SectionElementCase {
            public EasyCreditsSectionElement SectionElement;            
        }

        [System.Serializable] public class InlayCase {
            public EasyCreditsInlay EasyCreditsInlays;
            [Range(-1.0f, 1.0f)]
            public float Centering;
            public int Spacing;
            public int Scale;
        }

        [System.Serializable] public class StationaryInlayCase {
            public EasyCreditsInlay EasyCreditsInlays;
            public float StartTime;
            public int EndTime;
            public float Scale;
        }

        [HideInInspector] public bool scroll = true;
        [HideInInspector] public bool AdvancedOptions = false;

        [HideInInspector] public RenderMode CanvasRenderMode = RenderMode.ScreenSpaceCamera;
        [HideInInspector] public bool CameraOrthographic = true;
        [HideInInspector] public Camera Camera;


        public void PreviewPresetUpdate() {
            this.GetComponent<EasyCreditsPreviewManager>().ClearCredits();
            this.GetComponent<EasyCreditsPreviewManager>().GeneratePrevis();
        }

        private void Start() { Init(); }

        public void Init() {
            startingPassComplete = false;
            endOfInlays = false;
            endOfStationaryInlays = false;
            inlayCount = 0;
            stationaryInlayCount = 0;
            stationaryInlayCount = 0;
            canvasManager.scroll = scroll;
            canvasManager.offset = startPointOffset;
            canvasManager.backgroundColor = backgroundColor;
            canvasManager.startDelay = startDelay;
            canvasManager.scrollSpeed = scrollSpeed;
            canvasManager.CreateCanvas();
            canvasManager.CreatePannel();

            if (OnCreditsStart == null)
                OnCreditsStart = new UnityEvent();
            if (OnCreditsEnd == null)
                OnCreditsEnd = new UnityEvent();

            CreateData();
        }

        private int inlayCount = 0;
        private int stationaryInlayCount = 0;
        private bool endOfInlays = false;
        private bool endOfStationaryInlays = false;

        public void CreateData() {
            OnCreditsStart.Invoke();

            if (GameTitleImage) {
                canvasManager.PlaceLogo(GameTitleImage, gameTitleScale);
                canvasManager.PopulatePannel(GameTitle, GameTitleLineSpacing, gameTitleFont, gameTitleAnchor, GameTitlefontSize, GameTitleColor, false, GameTitleWordSpacing, TitleCharacterSpacing, false, true);
            }
            else if (GameTitle != "") {
                canvasManager.PopulatePannel(GameTitle, GameTitleLineSpacing, gameTitleFont, gameTitleAnchor, GameTitlefontSize, GameTitleColor, false, GameTitleWordSpacing, TitleCharacterSpacing, false, true);
            }

            if (DataFormat == DataFormatType.CSVCommaSeparatedValues) {
                CSV = Resources.Load<TextAsset>(fileName); // load credits file
                data = CSV.text.Split(new char[] { '\n' }); // create array of rows         
            }
            else if (DataFormat == DataFormatType.ECSEEasyCreditSectionElements) {
                int highestTitleCount = 0;
                foreach (var item in SectionElements) { //find height title count
                    if (item.SectionElement.Content.Count > highestTitleCount) { highestTitleCount = item.SectionElement.Content.Count; }
                }

                data = new string[highestTitleCount + 1]; // +1 to account for roles row
                for (int i = 0; i < SectionElements.Count; i++) { //Foreach data field
                    if (SectionElements.Count == i + 1) { //dont "," for last
                        data[0] = data[0] + SectionElements[i].SectionElement.SectionTitle;
                    } else {
                        data[0] = data[0] + SectionElements[i].SectionElement.SectionTitle + ",";
                    }

                    for (int x = 0; x < highestTitleCount; x++) {
                        if (SectionElements[i].SectionElement.Content.Count > x) { // account for this field having less then the max
                            if (SectionElements.Count == i + 1) { //dont "," for last
                                data[x + 1] = data[x + 1] + SectionElements[i].SectionElement.Content[x].Name ;
                            } else {
                                data[x + 1] = data[x + 1] + SectionElements[i].SectionElement.Content[x].Name + ",";
                            }
                        } else { //had under max so fill rest with empty slots
                            if (SectionElements.Count == i + 1) { //dont "," for last
                                data[x + 1] = data[x + 1] ;
                            } else {
                                data[x + 1] = data[x + 1] + ",";
                            }
                        }
                    }
                }
            }

            string[] topRowData = data[0].Split(new char[] { ',' }); // create string with role data

            for (int i = 0; i < topRowData.Length; i++) { // loop through roles
                if (topRowData[i] != "") { // if role isnt null
                    if (startingPassComplete == false) {
                        startingPassComplete = true;
                    } else {
                        canvasManager.PopulatePannel("", SectionLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing);
                        TriggerInlayPlacement();
                    }

                    if (creditType == CreditType.Inline) {
                        canvasManager.PopulatePannel(topRowData[i], RoleLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing);
                    } else if (creditType == CreditType.Cinematic) {
                        canvasManager.PopulatePannel(topRowData[i], RoleLineSpacing, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing, true);
                    }

                    for (int x = 1; x < data.Length; x++) {
                        string[] RowData = data[x].Split(new char[] { ',' }); // create string with role data
                        if (RowData[i] != "") {
                            canvasManager.PopulatePannel(RowData[i], TitleLineSpacing, titleFont, titleAnchor, TitlefontSize, titleColor, false, TitleWordSpacing, TitleCharacterSpacing);
                        }
                    }
                }
            }
            if(DataFormat == DataFormatType.ECSEEasyCreditSectionElements) { // account for 15 pixel off set when using data field. title spacing gets called with blank on csv
                canvasManager.PopulatePannel("", TitleLineSpacing * 3, roleFont, roleAnchor, RolefontSize, roleColor, false, RoleWordSpacing, RoleCharacterSpacing);
            }
            canvasManager.PopulatePannel(LegalLine, LegalLineSpacing, LegalFont, legalAnchor, LegalFontSize, LegalColor, true, LegalWordSpacing, LegalCharacterSpacing);

            canvasManager.panel.transform.rotation = canvasManager.transform.rotation;
        }

        //private bool isVis = false;
        
        private void Update() {
            if(canvasManager.LastElement.transform.position.y > this.transform.position.y) {
                StartCoroutine(Exit());
            }
            //if (canvasManager.LastElement.GetComponent<Renderer>().isVisible) {
            //    isVis = true;
            //}
            //if (isVis) {
            //    if (canvasManager.LastElement.GetComponent<Renderer>().isVisible == false) { StartCoroutine(Exit()); }
            //}
            if (!scroll) { StartCoroutine(Exit()); }
        }

        IEnumerator Exit() {
            yield return new WaitForSeconds(exitTime);
            OnCreditsEnd.Invoke();
            if(ExitToSceneOnEnd) { UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToExitTo); }
        }

        void TriggerInlayPlacement() {
            if (Inlays.Count < 1) { inlayType = InlayType.None; }
            //if not stationary
            else if (inlayType != InlayType.None && inlayType != InlayType.Stationary && inlayType != InlayType.StationaryAndScrollingInline && endOfInlays == false) {
                EasyCreditsInlay inlay = Inlays[inlayCount].EasyCreditsInlays;
                canvasManager.AddInlay(inlay, inlayType, inlayCount);

                if (inlayCount < Inlays.Count - 1) { inlayCount++; }
                else { endOfInlays = true; }
            }
            //if statationary
            else if (inlayType == InlayType.Stationary && endOfStationaryInlays == false) {
                EasyCreditsInlay inlay = StationaryInlays[stationaryInlayCount].EasyCreditsInlays;
                canvasManager.AddInlay(inlay, inlayType, stationaryInlayCount);

                if (stationaryInlayCount < StationaryInlays.Count - 1) { stationaryInlayCount++; }
                else { endOfStationaryInlays = true; }                   
            }
            //if startionary and inline
            else if (inlayType == InlayType.StationaryAndScrollingInline) {
                if (endOfInlays == false) {
                    EasyCreditsInlay inlay = Inlays[inlayCount].EasyCreditsInlays;
                    canvasManager.AddInlay(inlay, InlayType.ScrollingInline, inlayCount);

                    if (inlayCount < Inlays.Count - 1) { inlayCount++; }
                    else { endOfInlays = true; }
                }
                if (endOfStationaryInlays == false) {
                    EasyCreditsInlay inlay = StationaryInlays[stationaryInlayCount].EasyCreditsInlays;
                    canvasManager.AddInlay(inlay, InlayType.Stationary, stationaryInlayCount);

                    if (stationaryInlayCount < StationaryInlays.Count - 1) { stationaryInlayCount++; }                       
                    else { endOfStationaryInlays = true; }                        
                }
            }
        }
    }
}


