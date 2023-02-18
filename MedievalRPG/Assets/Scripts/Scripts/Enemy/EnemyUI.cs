using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public Transform enemyParentObjTrans;

    // Start is called before the first frame update
    void Start()
    {
        enemyParentObjTrans = this.gameObject.transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
