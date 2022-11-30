using UnityEngine;

public class RayDetection : MonoBehaviour
{
    public float Range;
    public void Initialize(float _range)
    {
        Range = _range;
    }
    /// <summary>
    /// The function that Throws Rays out and checks if there is a player inside.
    /// </summary>
    public bool Sight() =>  Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Range) &&
                     hit.transform.gameObject == GameManager.instance.playerGO;

}