using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private EnemyArrowController ArrowPrefab;
    [SerializeField] private Transform ArrowParent;
    public Transform GetArrowParent => ArrowParent;
    [SerializeField] private int ArrowsToGenerate;
    [SerializeField] private float DespawnTime;
    [SerializeField] private Transform HardCodeTarget;

    private EnemyArrowController[] ArrowControllers;


    /// <summary>
    /// With this, one can set the gameplay affecting values according to the Difficulty of the game
    /// </summary>
    /// <param name="_perfectionDistance">How far the Arrow can come near the player before not perfectly tracking it anymore.</param>
    /// <param name="_damage">How much damage the Arrow is supposed to cause</param>
    /// <param name="_speed">How fast the Arrow should go.</param>
    public void InitializeArrows(float _perfectionDistance, float _damage, float _speed)
    {
        DestroyArrows();
        GenerateArrows(_perfectionDistance, _damage, _speed);
    }

    private void GenerateArrows(float _perfectionDistance, float _damage, float _speed)
    {
        ArrowControllers = new EnemyArrowController[ArrowsToGenerate];
        Transform target = HardCodeTarget ? HardCodeTarget : GameManager.instance.playerGO.transform;
        for (int i = 0; i < ArrowControllers.Length; i++)
        {
            ArrowControllers[i] = Instantiate(ArrowPrefab);
            ArrowControllers[i].gameObject.name = $"Arrow {i}";
            ArrowControllers[i].Initialize(this, _perfectionDistance, DespawnTime, _damage, _speed, target);
        }
    }

    /// <summary>
    /// This script destroys any arrows that existed if any exist (Useful if Game Difficulty can be switched during the game.)
    /// </summary>
    private void DestroyArrows()
    {
        if (ArrowControllers == null)
            return;
        for (int i = ArrowControllers.Length; i > 0; i--)
            Destroy(ArrowControllers[i].gameObject);
        ArrowControllers = null;
    }

    /// <summary>
    /// The function to Spawn arrows and fire them, best used as an animation part
    /// </summary>
    /// <param name="_place"></param>
    public void SpawnAndFireArrow(Transform _place)
    {
        if (ArrowControllers.Length == 0)
        {
            Debug.Log("Arrow Fired without any arrows existing.");
            return;
        }
        for (int i = 0; i < ArrowControllers.Length; i++)
        {
            if (ArrowControllers[i].gameObject.activeSelf)
                continue;
            ArrowControllers[i].gameObject.transform.position = _place.transform.position;
            ArrowControllers[i].gameObject.SetActive(true);
            ArrowControllers[i].Fire();
            break;
        }
    }

    public void DespawnArrow(EnemyArrowController _arrowToDespawn)
    {
        _arrowToDespawn.gameObject.SetActive(false);
        _arrowToDespawn.transform.position = Vector3.zero;
    }
}