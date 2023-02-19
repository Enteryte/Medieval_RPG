using UnityEngine;

//[CreateAssetMenu(fileName = "New Enemy AI Pattern", menuName = "Scriptable Objects/Enemies/Enemy AI Profile", order = 0)]
public class SO_KI_Stats : ScriptableObject
{
    //True if the Enemy should move around a bit while not aware of the player
    public bool PatrolsWhileVibing = false;

    //Only matters if Enemy Patrols
    public float PatrollingRange = 0f;

    //True if the Enemy can be knocked down with Heavy Attacks
    public bool IsKnockDownable = true;

    //How many Detectors for sight there are, halved due to the way the detectors being set up requiring the number to be divisible by 2
    public int SightDetectorCountHalf = 3;

    //How far the enemies view-range is
    public float DetectionRange;

    //How wide the Enemies view-range is
    [Range(10f, 360f)] public float DetectionFOV = 10f;

    //How many Detectors for attack range there are, halved due to the way the detectors being set up requiring the number to be divisible by 2
    public int AttackDetectorCountHalf = 3;

    //In what range the player has to be in order for the enemy to consider attacking you
    public float AttackRange;

    //In what area you have to be in for the enemy to attack you without turning
    [Range(10f, 360f)] public float AttackRangeFOV = 10f;

    //How long it takes after an attack to attack again.
    public float AttackCoolDown;

    //How long it takes leaving sight to return to original position.
    public float AlertCoolDown;

    //Value to determine how long it takes for the patroller to look for a new place.
    public float Patience;
}