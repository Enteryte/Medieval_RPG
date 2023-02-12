using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public GameObject testPlayer;

    public EnemyState currentState;

    public float viewRadius;

    [Range(0, 360)] public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public float nearestDistance;
    [HideInInspector] public Transform nearestObjTrans;

    public void Update()
    {
        RunStateMachine();
    }

    public void RunStateMachine()
    {
        EnemyState nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }

    public void SwitchToTheNextState(EnemyState nextState)
    {
        currentState = nextState;
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
