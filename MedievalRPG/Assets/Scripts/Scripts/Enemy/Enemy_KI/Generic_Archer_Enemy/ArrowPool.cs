using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private EnemyArrowController ArrowPrefab;
    [SerializeField] private Transform ArrowParent;
    public Transform GetArrowParent => ArrowParent;
    [SerializeField] private int ArrowsToGenerate;
    [SerializeField] private float DespawnTime;
    [SerializeField] private Transform HardCodeTarget;
    
    [SerializeField]public float ArrowDamage = 20f;
    [SerializeField]public float ArrowSpeed = 6f;

    private EnemyArrowController[] ArrowControllers;
//Temporary Function, replace it with something external.
    private void Start()
    {
        InitializeArrows(ArrowDamage, ArrowSpeed);
    }

    /// <summary>
    /// With this, one can set the gameplay affecting values according to the Difficulty of the game
    /// </summary>
    /// <param name="_damage">How much damage the Arrow is supposed to cause</param>
    /// <param name="_speed">How fast the Arrow should go.</param>
    public void InitializeArrows(float _damage, float _speed)
    {
        DestroyArrows();
        GenerateArrows( _damage, _speed);
    }

    private void GenerateArrows(float _damage, float _speed)
    {
        ArrowControllers = new EnemyArrowController[ArrowsToGenerate];
        Transform target = HardCodeTarget ? HardCodeTarget : GameManager.instance.playerGO.transform;
        for (int i = 0; i < ArrowControllers.Length; i++)
        {
            ArrowControllers[i] = Instantiate(ArrowPrefab, ArrowParent);
            ArrowControllers[i].gameObject.name = $"Arrow Nr. {i}";
            ArrowControllers[i].Initialize(this, DespawnTime, _damage, _speed, target);
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

        foreach (EnemyArrowController arrow in ArrowControllers)
        {
            if (arrow.gameObject.activeSelf)
                continue;
            Transform arrowTransform = arrow.transform;
            arrowTransform.position = _place.transform.position;
            arrowTransform.parent = null;
            arrow.gameObject.SetActive(true);
            arrow.Fire();
            break;
        }
    }

    public void DespawnArrow(EnemyArrowController _arrowToDespawn)
    {
        _arrowToDespawn.gameObject.SetActive(false);
        Transform despawnArrowTransform = _arrowToDespawn.transform;
        despawnArrowTransform.parent = ArrowParent;
        despawnArrowTransform.position = Vector3.zero;
    }
}