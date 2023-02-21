using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerValueManager : MonoBehaviour
{
    public static PlayerValueManager instance;

    #region Properties
    public float CurrHP
    {
        get { return currHP; }
        set 
        {
            if (invincible == false)
            {
                if (value <= 0 && !isDead)
                {
                    currHP = 0;
                    Die();
                }

                if (currHP - value > 0)
                {
                    currHP = value;
                }

                if (value > currHP)
                {
                    currHP = value;
                }

                healthSlider.value = CurrHP;
            }
        }
    }
    #endregion

    public float money;

    [Header("Health")]
    public bool invincible = false;
    public float normalHP;
    private float currHP;
    public Slider healthSlider;

    [Header("Stamina")]
    public float normalStamina;
    public float currStamina;
    public Slider staminaSlider;

    public float timeTillRefillStamina;
    public float currWaitingTime;
    public float refillSpeed;

    [Header("Strength")]
    public float normalStrength;
    public float currStrength;

    [Header("Armor")]
    public float normalArmor;
    public float currArmor;

    [Header("Tutorial")]
    public TutorialBaseProfile staminaTutorial;

    [Header("Death")]
    public TimelineAsset whenDeathTL;
    public LoadingScreenProfile deathLSP;

    public float afterPlayerDiedSkippingTime1;

    public bool isDead = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

            currHP = normalHP;

            if (staminaSlider != null)
            {
                staminaSlider.maxValue = normalStamina;
                staminaSlider.value = currStamina;

                healthSlider.maxValue = normalHP;
                healthSlider.value = currHP;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!DebuffManager.instance.lowerStaminaDebuff)
        {
            if (currStamina < normalStamina && currWaitingTime < timeTillRefillStamina)
            {
                currWaitingTime += Time.deltaTime;
            }
            else if (currWaitingTime >= timeTillRefillStamina)
            {
                if(staminaSlider != null)
                {
                    staminaSlider.value += Mathf.Lerp(currStamina, normalStamina, Time.deltaTime * 1f);
                    currStamina = staminaSlider.value;
                }

                if (currStamina >= normalStamina)
                {
                    currWaitingTime = 0;
                }
            }
        }
        else
        {
            if (currStamina < DebuffManager.instance.loweredMaxStamina && currWaitingTime < timeTillRefillStamina)
            {
                currWaitingTime += Time.deltaTime;
            }
            else if (currWaitingTime >= timeTillRefillStamina)
            {
                staminaSlider.value += Mathf.Lerp(currStamina, DebuffManager.instance.loweredMaxStamina, Time.deltaTime * 1f);
                currStamina = staminaSlider.value;

                if (currStamina >= DebuffManager.instance.loweredMaxStamina)
                {
                    currWaitingTime = 0;
                }
            }
        }

        if (staminaSlider.value <= normalStamina / 2)
        {
            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(staminaTutorial);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrHP = 0;
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    CurrHP = 0;
        //    Debug.Log(CurrHP);
        //}
    }

    public void RemoveStamina(float staminaAmountToRemove)
    {
        currStamina -= staminaAmountToRemove;

        if(staminaSlider != null)
        {
            staminaSlider.value = currStamina;
        }

        currWaitingTime = 0;
    }

    public void Die()
    {
        //Sterbegedöns einfügen

        //ThirdPersonController.instance._animator.Play("Death");

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            LoadingScreen.instance.playerWasDead = true;
        }

        isDead = true;

        CutsceneManager.instance.currCP = null;
        CutsceneManager.instance.playableDirector.playableAsset = whenDeathTL;
        CutsceneManager.instance.playableDirector.Play();

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            LoadingScreen.currLSP = PlayerValueManager.instance.deathLSP;
            LoadingScreen.currSpawnPos = new Vector3(0, 0, 0);
            LoadingScreen.currSpawnRot = Quaternion.identity;

            LoadingScreen.instance.placeNameTxt.text = PlayerValueManager.instance.deathLSP.placeName;
            LoadingScreen.instance.backgroundImg.sprite = PlayerValueManager.instance.deathLSP.backgroundSprite;
            LoadingScreen.instance.descriptionTxt.text = PlayerValueManager.instance.deathLSP.descriptionTextString;

            //SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
            SceneChangeManager.instance.GetComponent<Animator>().Rebind();
            //SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

            SceneChangeManager.instance.loadingScreen.SetActive(true);

            LoadingScreen.instance.gameObject.SetActive(true);
            LoadingScreen.instance.ActivateAnimator();
            SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");

            SceneChangeManager.instance.wentThroughTrigger = true;
        }
    }
}
