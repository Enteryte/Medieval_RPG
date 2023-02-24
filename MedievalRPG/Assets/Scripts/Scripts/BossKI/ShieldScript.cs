using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    [SerializeField] private MeshRenderer mat1;
    [SerializeField] private MeshRenderer mat2;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource source1;

    private SkeletonBossStats stats;
    private bool fadeIn = true;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("BossHitbox").GetComponent<SkeletonBossStats>();
        stats.invincible = true;
        source.volume = AudioManager.Instance.EffectsVolume * AudioManager.Instance.MasterVolume;
        source1.volume = AudioManager.Instance.EffectsVolume * AudioManager.Instance.MasterVolume;
    }

    public void Deactivate()
    {
        fadeIn = false;
        stats.invincible = false;
    }

    private void Fade()
    {
        if (fadeIn == false && mat1.material.GetFloat("_DissolveAmount") < 1)
        {
            mat1.material.SetFloat("_DissolveAmount", mat1.material.GetFloat("_DissolveAmount") + Time.deltaTime * fadeSpeed);
            mat2.material.SetFloat("_DissolveAmount", mat2.material.GetFloat("_DissolveAmount") + Time.deltaTime * fadeSpeed);
        }
        if (fadeIn == true && mat1.material.GetFloat("_DissolveAmount") > 0)
        {
            mat1.material.SetFloat("_DissolveAmount", mat1.material.GetFloat("_DissolveAmount") - Time.deltaTime * fadeSpeed);
            mat2.material.SetFloat("_DissolveAmount", mat2.material.GetFloat("_DissolveAmount") - Time.deltaTime * fadeSpeed);
        }
    }

    private void Update()
    {
        Fade();
    }
}
