using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveImpulse : SkeletonBossActions
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject WaveObject;
    [SerializeField] private GameObject WaveObjectP2;
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float amountOfWaves = 1;

    public override void UseAction()
    {
        anim.SetTrigger("ShockWave");
    }

    public void CreateWave()
    {
        if (KI.phase1 == false)
        {
            StartCoroutine(MultipleWaves());
            return;
        }

        GameObject wave = Instantiate(WaveObject);
        wave.transform.parent = null;
        wave.transform.position = transform.position;
    }

    IEnumerator MultipleWaves()
    {
        for (int i = 0; i < amountOfWaves; i++)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            GameObject wave = Instantiate(WaveObjectP2);
            wave.transform.parent = null;
            wave.transform.position = transform.position;
        }
    }
}
