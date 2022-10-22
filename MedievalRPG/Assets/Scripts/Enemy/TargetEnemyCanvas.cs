using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemyCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(FightManager.instance.currTargetEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (FightManager.instance.currTargetEnemy != null)
        {
            this.gameObject.transform.position = Camera.main.WorldToScreenPoint(FightManager.instance.currTargetEnemy.transform.position);
        }
    }
}
