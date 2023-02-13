using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightToAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float moveStopValue;

    [HideInInspector]
    public float animationSpeed = 0;

    private void CheckSpeed()
    {
        if(InventoryManager.instance.currHoldingWeight >= InventoryManager.instance.maxHoldingWeight && InventoryManager.instance.currHoldingWeight < InventoryManager.instance.maxHoldingWeight + moveStopValue)
        {
            animationSpeed = 0.35f;
        }
        if (InventoryManager.instance.currHoldingWeight >= InventoryManager.instance.maxHoldingWeight + moveStopValue)
        {
            animationSpeed = 0;
        }
        if(InventoryManager.instance.currHoldingWeight < InventoryManager.instance.maxHoldingWeight)
        {
            animationSpeed = 1;
        }

        ChangeAnimatorSpeed();
    }

    private void ChangeAnimatorSpeed()
    {
        if(animationSpeed >= 0.3f && animationSpeed <= 0.4f)
        {
            anim.speed = 0.9f;
            return;
        }

        anim.speed = animationSpeed;
    }
    
    private void Update()
    {
        if (!GameManager.instance.gameIsPaused)
        {
            CheckSpeed();
        }
    }
}
