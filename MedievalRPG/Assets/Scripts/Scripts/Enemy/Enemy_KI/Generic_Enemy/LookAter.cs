using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LookAter : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    [SerializeField] private Transform Head;

    public Vector3 LookAtTargetPos;

    private const float LOOK_AT_COOL_TIME = 0.2f;
    private const float LOOK_AT_HEAT_TIME = 0.2f;
    [SerializeField]private bool IsLooking = true;

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
        Vector3 headPos = Head.position;
        Vector3 currentDir = LookAtPos - headPos;
        Vector3 futureDir = LookAtTargetPos - headPos;

        currentDir = Vector3.RotateTowards(currentDir, futureDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        LookAtPos = headPos + currentDir;

        float blendTime = lookAtTargetWeight > LookAtWeight ? LOOK_AT_HEAT_TIME : LOOK_AT_COOL_TIME;
        LookAtWeight = Mathf.MoveTowards(LookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
        Anim.SetLookAtWeight(LookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        Anim.SetLookAtPosition(LookAtPos);
    }
}