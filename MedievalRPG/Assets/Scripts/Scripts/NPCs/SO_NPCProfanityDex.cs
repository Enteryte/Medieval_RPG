using UnityEngine;
[CreateAssetMenu(fileName = "New NPC ProfanityDex", menuName = "Scriptable Objects/NPCs/NPC ProfanityDex")]
public class SO_NPCProfanityDex : ScriptableObject
{
    [SerializeField]private string Info;
    public AudioClip[] Insults;
}
