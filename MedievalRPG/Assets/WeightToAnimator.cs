using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightToAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float moveStopValue;

    [SerializeField] private float animationSpeed = 0;

    private void CheckSpeed()
    {
        if(InventoryManager.instance.currHoldingWeight >= InventoryManager.instance.maxHoldingWeight && InventoryManager.instance.currHoldingWeight < InventoryManager.instance.maxHoldingWeight + moveStopValue)
        {
            animationSpeed = 0.5f;
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
        anim.speed = animationSpeed;
    }
    
    private void Update()
    {
        CheckSpeed();
    }
}
