using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class LocomotionAgent : MonoBehaviour
{
    private Animator Anim;
    private NavMeshAgent Agent;
    private LookAter LA;
    private Transform Parent;
    private Vector2 SmoothDeltaPos = Vector2.zero;
    private Vector2 Velocity = Vector2.zero;
    private const float TOLERANCE = 0.5f;


    private void Start()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponentInParent<NavMeshAgent>();
        LA = GetComponent<LookAter>();
        Parent = transform.parent;
        Agent.updatePosition = false;
    }

    private void Update()
    {
        Transform transformAcc = Parent.transform;
        Vector3 worldDeltaPos = Agent.nextPosition - transformAcc.position;

        float deltaX = Vector3.Dot(transformAcc.right, worldDeltaPos);
        float deltaY = Vector3.Dot(transformAcc.forward, worldDeltaPos);
        Vector2 deltaPos = new Vector2(deltaX, deltaY);

        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        SmoothDeltaPos = Vector2.Lerp(SmoothDeltaPos, deltaPos, smooth);

        if (Time.deltaTime > 1e-5f)
            Velocity = SmoothDeltaPos / Time.deltaTime;

        bool shouldMove = Velocity.magnitude > TOLERANCE && Agent.remainingDistance > Agent.radius;

        Anim.SetBool(Animator.StringToHash("IsMoving"), shouldMove);
        Anim.SetFloat(Animator.StringToHash("VelX"), Velocity.x);
        Anim.SetFloat(Animator.StringToHash("VelY"), Velocity.y);

        if (LA)
            LA.LookAtTargetPos = Agent.steeringTarget + transform.forward;
    }

    private void OnAnimatorMove()
    {
        transform.position = Agent.nextPosition;
    }
}