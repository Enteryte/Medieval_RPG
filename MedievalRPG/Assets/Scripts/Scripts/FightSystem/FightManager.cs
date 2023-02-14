using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;
using Cinemachine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    public GameObject targetEnemyCanvasObj;

    public GameObject currTargetEnemy;

    public CinemachineVirtualCamera targetCVC;

    public GameObject crosshairGO;

    [Header("Enemies")]
    public ArrowPool enemyArrowPool;

    [Header("In Fight")]
    public bool isInFight = false;

    public AudioClip fightMusic;
    public static AudioClip lastMusicClip;

    public List<BaseEnemyKI> enemiesInFight;

    //public int CurrArrowAmount
    //{
    //    get
    //    {
    //        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
    //        {
    //            if (InventoryManager.instance.inventory.slots[i].itemBase == FightManager.instance.arrowIB)
    //            {
    //                return InventoryManager.instance.inventory.slots[i].itemAmount;
    //            }
    //        }

    //        return 0;
    //    }
    //    set
    //    {
    //        currArrowCountTxt.text = CurrArrowAmount.ToString();

    //        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
    //        {
    //            if (InventoryManager.instance.inventory.slots[i].itemBase == FightManager.instance.arrowIB)
    //            {
    //                return InventoryManager.instance.inventory.slots[i].itemAmount;
    //            }
    //        }
    //    }
    //}

    [Header("Arrows")]
    public ItemBaseProfile arrowIB;

    public TMP_Text currArrowCountTxt;
    public Image arrowHUDImg;
    public Image arrowHUDBackgroundImg;

    public int currArrowAmount;

    [Header("Tutorials")]
    public TutorialBaseProfile doARollTutorial;
    public TutorialBaseProfile shildBlockTutorial;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currTargetEnemy != null)
        {
            if (Vector3.Distance(currTargetEnemy.transform.position, GameManager.instance.playerGO.transform.position) 
                > 7)
            {
                currTargetEnemy = null;

                targetEnemyCanvasObj.SetActive(false);

                targetCVC.gameObject.SetActive(false);
            }
        }

        // ---------------------------------> NUR ZUM TESTEN
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("UUUUUUUUUUUUUUUUUUUU");

            isInFight = true;
            StartCoroutine(FadeOldMusicOut());
        }

        // ---------------------------------> NUR ZUM TESTEN
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("OOOOOOOOOOOOOOOOO");

            isInFight = false;
            StartCoroutine(FadeOldMusicOut());
        }
    }

    public void UpdateArrowHUDDisplay()
    {
        arrowHUDImg.color = Color.red;
        arrowHUDBackgroundImg.color = Color.red;

        currArrowCountTxt.text = "0";

        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == arrowIB)
            {
                currArrowCountTxt.text = InventoryManager.instance.inventory.slots[i].itemAmount.ToString();

                arrowHUDImg.color = Color.white;
                arrowHUDBackgroundImg.color = Color.black;
            }
        }
    }

    public void TargetEnemy(GameObject currInteractTarget)
    {
        if (currTargetEnemy == currInteractTarget)
        {
            currTargetEnemy = null;

            targetEnemyCanvasObj.SetActive(false);

            targetCVC.gameObject.SetActive(false);

            Debug.Log("TARGETED ENEMY 1");
        }
        else
        {
            currTargetEnemy = currInteractTarget;

            targetEnemyCanvasObj.SetActive(true);

            targetCVC.gameObject.SetActive(true);

            Debug.Log("TARGETED ENEMY 2");
        }

        Debug.Log("TARGETED ENEMY - ");
    }

    public IEnumerator FadeOldMusicOut()
    {
        float currentTime = 0;
        float start = GameManager.instance.musicAudioSource.volume;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            GameManager.instance.musicAudioSource.volume = Mathf.Lerp(start, 0, currentTime / 1f);

            yield return null;
        }

        if (isInFight)
        {
            lastMusicClip = GameManager.instance.musicAudioSource.clip;

            GameManager.instance.musicAudioSource.clip = fightMusic;
        }
        else
        {
            GameManager.instance.musicAudioSource.clip = lastMusicClip;
        }

        GameManager.instance.musicAudioSource.Play();

        StartCoroutine(FadeNewMusicIn());

        yield break;
    }

    public IEnumerator FadeNewMusicIn()
    {
        Debug.Log("NEW MUSIC");

        float currentTime = 0;
        float start = 0;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            GameManager.instance.musicAudioSource.volume = Mathf.Lerp(start, OptionManager.instance.musicSlider.value, currentTime / 1f);

            yield return null;
        }

        yield break;
    }
}
