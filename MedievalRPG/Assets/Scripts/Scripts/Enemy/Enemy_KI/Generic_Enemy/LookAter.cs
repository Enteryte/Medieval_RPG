using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LookAter : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    public Transform Head = null;

     public Vector3 LookAtTargetPos;

    [FormerlySerializedAs("LookAtCoolDown")] public float LookAtCoolTime = 0.2f;
    public float LookAtHeatTime = 0.2f;
    public bool IsLooking = true;

    private Vector3 LookAtPos;
    private float LookAtWeight = 0.0f;
    
    void Start()
    {
        if (!Head)
        {
            enabled = false;
            return;
        }

        LookAtTargetPos = Head.position + transform.forward;
        LookAtPos = LookAtTargetPos;

    }

    private void OnAnimatorIK()
    {
        LookAtTargetPos.y = Head.position.y;
        float lookAtTargetWeight = IsLooking ? 1.0f : 0.0f;
        Vector3 currentDir = LookAtPos - Head.position;
        Vector3 futureDir = LookAtTargetPos - Head.position;

        currentDir = Vector3.RotateTowards(currentDir, futureDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        LookAtPos = Head.position + currentDir;

        float blendTime = lookAtTargetWeight > LookAtWeight ? LookAtHeatTime : LookAtCoolTime;
        LookAtWeight = Mathf.MoveTowards(LookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
        Anim.SetLookAtWeight(LookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        Anim.SetLookAtPosition(LookAtPos);
    }
}
