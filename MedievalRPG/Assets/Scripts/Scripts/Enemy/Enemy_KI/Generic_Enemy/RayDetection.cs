using System;
using UnityEngine;

public class RayDetection : MonoBehaviour
{
    private float Range;
    private Color Color;
    private GameObject HardTarget;
    private bool r;

    
    public void Initialize(float _range, Color _color, GameObject _hardTarget = null)
    {
        Range = _range;
        Color = _color;
        HardTarget = _hardTarget;
    }

    /// <summary>
    /// The function that Throws Rays out and checks if there is a player inside.
    /// </summary>
    public bool Sight()
    {
            return Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Range) &&
                   hit.transform.gameObject == GameManager.instance.playerGO;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color;
        Transform t = transform;
        Gizmos.DrawLine(t.position, t.forward * Range + t.position);
    }
}