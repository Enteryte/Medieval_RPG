using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Transform HolderBody;
    public Transform EnemyParentObjTrans;
    private Camera Camera;
    private float RelativeHealthValue;
    [SerializeField] private Slider HealthDepict;
    [SerializeField] private TMP_Text EnemyName;


    // Start is called before the first frame update
    void Start()
    {
        Camera = Camera.main;
    }

    public void Init(float _maxHealth, string _enemyName)
    {
        RelativeHealthValue = 1.0f / _maxHealth;
        EnemyName.text = _enemyName;
    }

    public void HealthUpdate(float _remainingHP, bool _isHeavyDamage = false)
    {
        HealthDepict.value -= _remainingHP * RelativeHealthValue;
    }

    void Update()
    {
        if (Camera)
            HolderBody.transform.LookAt(Camera.transform);
    }
}