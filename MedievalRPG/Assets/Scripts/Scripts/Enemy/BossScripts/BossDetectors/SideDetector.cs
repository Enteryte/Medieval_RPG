using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetector : MonoBehaviour
{
    private BossKI Parent;

    public void Init(BossKI _parent)
    {
        Parent = _parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
            Parent.SetIsOnSides(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
            Parent.SetIsOnSides(false);
    }
}