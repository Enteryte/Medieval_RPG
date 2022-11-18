using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingActions : MonoBehaviour
{
    public GameObject equippedWeapon;

    private Animator anim;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    private void OnLightAttack()
    {
        //anim.SetTrigger("LightAttackSword");
    }

    void Update()
    {
        
    }
}
