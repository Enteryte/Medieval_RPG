using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onerat.EasyCredits
{
    [ExecuteInEditMode]
    public class EasyCreditsPreviewManager : MonoBehaviour
    {
        private Onerat.EasyCredits.EasyCreditsDataManager dataManager => this.GetComponent<Onerat.EasyCredits.EasyCreditsDataManager>();
        private Onerat.EasyCredits.EasyCreditsCanvasManager canvasManager => this.GetComponent<Onerat.EasyCredits.EasyCreditsCanvasManager>();
        private bool active = false;

        public void ClearCredits()
        {
            //camera needs to be in persective mode when generating canvas to get correct scale
            if (canvasManager.Camera == null) { Camera.main.orthographic = false; }
            else { canvasManager.Camera.orthographic = false; }
            DestroyImmediate(this.transform.GetChild(1).transform.GetChild(0).gameObject);
            canvasManager.DistanceToMove = 0;
            DestroyImmediate(this.transform.GetChild(1).GetComponent<UnityEngine.UI.CanvasScaler>());
            DestroyImmediate(this.transform.GetChild(1).GetComponent<Canvas>());
            canvasManager.ClearStationaryList();
        }

        public void GeneratePrevis()
        {
            if (canvasManager.Camera == null) { Camera.main.orthographic = false; }
            else { canvasManager.Camera.orthographic = false; }
            dataManager.Init();
        }

        public void GenerateCanvas()
        {
            dataManager.Init();
        }

        public void Update()
        {
            if (Application.isPlaying == false)
            {
                if (dataManager.EnablePreview == false)
                {
                    if (active == true)
                    {
                        active = false;
                        ClearCredits();
                        DestroyImmediate(this.transform.GetChild(1).GetComponent<UnityEngine.UI.CanvasScaler>());
                        DestroyImmediate(this.transform.GetChild(1).GetComponent<Canvas>());
                    }
                    return;
                }
                else if (active == false)
                {
                    active = true;
                    GenerateCanvas();
                }
                ClearCredits();
                GeneratePrevis();

                canvasManager.panel.transform.position = this.transform.position +
                    new Vector3(
                        canvasManager.panel.transform.position.x,
                        -canvasManager.LastElement.transform.position.y * dataManager.PreviewScroll,
                        canvasManager.panel.transform.position.z
                    );
            }
        }
    }

}