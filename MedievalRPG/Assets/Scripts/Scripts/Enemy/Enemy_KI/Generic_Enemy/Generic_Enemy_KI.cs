using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Generic_Enemy_KI : MonoBehaviour
{
    [SerializeField] private SO_KI_Stats KiStats;
    public SO_KI_Stats GetKiStats => KiStats;
    [SerializeField] private Animator Animator;
    [SerializeField] private NavMeshAgent Agent;

    private bool IsSeeingPlayer = false;
    private bool HasSeenPlayer = false;
    private bool IsInAttackRange = false;

    private bool HasDied = false;

    private Vector3 RandomTarget;
    private Transform Target;
    private Transform StartPos;

    //The Transforms for the Detectors, must be an amount divisible by 2
    public RayDetection[] RayDetectorsSight;

    //The Transforms for the Detectors, must be an amount divisible by 2
    public RayDetection[] RayDetectorsAttack;

    //Value to determine how long it takes for the patroller to look for a new place.
    [SerializeField] private float Patience = 6f;

    private int CheckValue = 0;

    void Start()
    {
        //TODO: Temp solution, will get replaced once all Generic Enemy Animations are there for a modular system to just give the ID's properly
        Animator.SetBool("PatrolsWhileVibing", KiStats.PatrolsWhileVibing);
        Animator.SetBool("IsKnockDownable", KiStats.IsKnockDownable);
        SetDetectors(KiStats.DetectionFOV, RayDetectorsSight);
        SetDetectors(KiStats.AttackRangeFOV, RayDetectorsAttack);
    }

    void Update()
    {
        IsSeeingPlayer = DetectorCheck(RayDetectorsSight);
        SightEvent(IsSeeingPlayer);
        //Putting the Attack Detection into an if so it only checks when it has the player within it's sight for better performance.
        if (IsSeeingPlayer)
        {
            IsInAttackRange = DetectorCheck(RayDetectorsAttack);
            //TODO: Set the animation Parameters somewhere
        }
    }

    private bool DetectorCheck(RayDetection[] _detectors)
    {
        for (int i = 0, CheckValue = 0; i < _detectors.Length; i++)
            CheckValue += _detectors[i].Sight() ? 1 : 0;
        return (CheckValue > 0) ? true : false;
    }
    /// <summary>
    /// A vector3 that generates a random position on the map that the NPC can use as a target while he isn't chasing anything. Also checks the position on the mesh for improved navigation.
    /// </summary>
    /// <returns>A random Vector3 on the Mesh</returns>
    private Vector3 GenerateRandomTarget()
    {
        //TODO: Alter this later when I find out how to measure the dungeon area. Probs make it editable via the editor
        Vector3 myTarget = new Vector3(Random.Range(0, KiStats.PatrollingRange), 0,
            Random.Range(0, KiStats.PatrollingRange));
        if (NavMesh.SamplePosition(myTarget, out NavMeshHit hit, KiStats.PatrollingRange, NavMesh.AllAreas))
            myTarget = hit.position;
        return myTarget;
    }

    /// <summary>
    /// A coroutine that just waits for how long the Patience of the NPC lasts before requesting a new random Target.
    /// </summary>
    private IEnumerator TimeToLookAtNewRandomTarget()
    {
        while (!IsSeeingPlayer)
        {
            yield return new WaitForSeconds(Patience);
            RandomTarget = GenerateRandomTarget();
        }
    }

    private void NoticeEnemy()
    {
        Target = GameManager.instance.playerGO.transform;
        HasSeenPlayer = true;
        Animator.SetTrigger("NoticedYou");
        Animator.SetBool("KnowsAboutYou", true);
    }

    public void SightEvent(bool _isSeeingPlayer)
    {
        switch (HasSeenPlayer)
        {
            case false when _isSeeingPlayer:
                NoticeEnemy();
                break;
            case true when _isSeeingPlayer:
                /*TODO: Do the Targeting and move into attacking range*/
                break;
            case true when !_isSeeingPlayer:
                //TODO: Do a save of the players current Position and move there and look around to see if it can find him.
                //TODO: If not, move back to origin point but remain in alert position
                break;
            case false when !_isSeeingPlayer:
                //TODO: Do the default behaviour
                break;
        }
    }

    private void CheckAttackPossible()
    {
    }

    private void Death()
    {
    }

    /// <summary>
    /// The Function that appoints the Detectors into their Position according to the Field of View
    /// TODO: Create the Detectors via code and just multiply the amount by 2, maybe even with a range
    /// </summary>
    /// <param name="_fov">The Field of View for the set of Detectors</param>
    /// <param name="_detectors">The Detectors for this particular Function</param>
    private void SetDetectors(float _fov, RayDetection[] _detectors)
    {
        if (_detectors.Length % 2 != 0)
            throw new Exception("Amount of Detectors not divisible by 2");
        float angleSteps = (_fov / _detectors.Length) / 2f;
        for (int i = 0; i < _detectors.Length; i += 2)
        {
            _detectors[i].gameObject.transform.localRotation = Quaternion.AngleAxis(angleSteps * (i + 1), Vector3.up);
            _detectors[i + 1].gameObject.transform.localRotation =
                Quaternion.AngleAxis(-angleSteps * (i + 1), Vector3.up);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.forward * KiStats.DetectionRange + transform.position);
        for (int i = 0; i < RayDetectorsSight.Length; i++)
            Gizmos.DrawLine(transform.position,
                RayDetectorsSight[i].gameObject.transform.forward * KiStats.DetectionRange + RayDetectorsSight[i].gameObject.transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * KiStats.AttackRange + transform.position);
        for (int i = 0; i < RayDetectorsAttack.Length; i++)
            Gizmos.DrawLine(transform.position,
                RayDetectorsAttack[i].gameObject.transform.forward * KiStats.AttackRange + RayDetectorsAttack[i].gameObject.transform.position);
    }
}