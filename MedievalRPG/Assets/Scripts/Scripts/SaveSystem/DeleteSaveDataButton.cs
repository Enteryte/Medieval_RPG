using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveDataButton : MonoBehaviour
{
    public LoadSlot correspondingLoadSlot;
    public AnimationClip openAreYouSureDeleteScreenAnim;

    public void OpenAreYouSureToDeleteScreen()
    {
        StartScreenManager.currClickedLoadSlot = correspondingLoadSlot;

        if (!StartScreenManager.instance.dontAskOnDeleteDataAgain)
        {
            StartScreenManager.instance.mainAnimator.Play(openAreYouSureDeleteScreenAnim.name);
        }
        else
        {
            // WIP: Delete SaveData of currClickedLoadSlot

            StartScreenManager.instance.saveGameScreenshot.enabled = false;
            StartScreenManager.instance.saveGameScreenshot.sprite = null;

            if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
            {
                var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

                for (int i = 0; i < dirInfo.Length; i++)
                {
                    if (dirInfo[i].ToString() == StartScreenManager.currClickedLoadSlot.correspondingSaveDataDirectory.ToString())
                    {
#if UNITY_EDITOR
                        FileUtil.DeleteFileOrDirectory(StartScreenManager.currClickedLoadSlot.correspondingSaveDataDirectory.ToString());
#endif
                    }
                }
            }

            StartScreenManager.currClickedLoadSlot = null;
            Destroy(correspondingLoadSlot.gameObject);
        }
    }
}
