using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernKeeper : MonoBehaviour
{
    public static TavernKeeper instance;

    public int maxDrunkBeer = 5;
    public int currBeerCount = 0;

    public float timeTillNotDrunk = 25;

    public Coroutine beereDebuffCoro;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DrinkBeer();
        }
    }

    public void DrinkBeer()
    {
        currBeerCount += 1;

        if (beereDebuffCoro != null)
        {
            StopCoroutine(beereDebuffCoro);
        }

        var gettingDrunkChanceRndmValue = Random.value;

        if (currBeerCount <= maxDrunkBeer)
        {
            if (currBeerCount == 2)
            {
                if (gettingDrunkChanceRndmValue > 0.98f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 3)
            {
                if (gettingDrunkChanceRndmValue > 0.75f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 4)
            {
                if (gettingDrunkChanceRndmValue > 0.5f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 5)
            {
                if (gettingDrunkChanceRndmValue > 0.25f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
        }
        else
        {
            FaintingAfterGotDrunk();

            return;
        }

        beereDebuffCoro = StartCoroutine(ResetBeerDebuff());
    }

    public IEnumerator ResetBeerDebuff()
    {
        float time = 0;

        while (time < timeTillNotDrunk)
        {
            time += Time.deltaTime;

            yield return null;
        }

        currBeerCount = 0;

        beereDebuffCoro = null;
    }

    public void FaintingAfterGotDrunk() // If the player drank too much.
    {
        Debug.Log("DRANK TOO MUCH");

        StopCoroutine(beereDebuffCoro);
        beereDebuffCoro = null;

        currBeerCount = 0;
    }
}
