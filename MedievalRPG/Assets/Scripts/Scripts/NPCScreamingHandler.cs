using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScreamingHandler : MonoBehaviour
{
    public AudioSource nPCAudioSource;
    public AudioClip audioToPlay;

    public Animator animator;
    public AnimationClip screamAnim;

    public bool isPlayingAudio = false;

    public LayerMask targetMask;

    public float viewRadius;

    [Range(0, 360)] public float viewAngle;

    public NPCMissionSymbol nPCMS;

    public float waitTime;
    public float currWaitTime = 0;

    public void Start()
    {
        GameManager.instance.allNPCScreamingHandler.Add(this);
    }

    public void Update()
    {
        if (nPCMS.exclaImg.activeSelf && ThirdPersonController.instance.canMove || nPCMS.questionImg.activeSelf && ThirdPersonController.instance.canMove)
        {
            if (!isPlayingAudio)
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

                if (targetsInViewRadius.Length > 0)
                {
                    PlayScreamAudio();

                    animator.SetBool("AudioIsPlaying", true);
                    animator.Play("Waving");
                }
            }
            else if (isPlayingAudio && currWaitTime < waitTime)
            {
                if (ThirdPersonController.instance.canMove)
                {
                    currWaitTime += Time.deltaTime;

                    if (nPCAudioSource.clip != null)
                    {
                        if (currWaitTime > nPCAudioSource.clip.length)
                        {
                            animator.SetBool("AudioIsPlaying", false);
                        }

                        if (currWaitTime >= waitTime)
                        {
                            isPlayingAudio = false;

                            currWaitTime = 0;

                            //animator.enabled = false;
                        }
                    }
                }
            }
        }
    }

    public void SetValuesAfterHadDialogue()
    {
        animator.SetBool("AudioIsPlaying", false);

        isPlayingAudio = true;

        currWaitTime = 0;
    }

    public void PlayScreamAudio()
    {
        isPlayingAudio = true;

        nPCAudioSource.Play();
    }

    public Vector3 DirFromAngles(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
