using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Transform HolderBody;
    public Transform EnemyParentObjTrans;
    private Camera Camera;
    private float RelativeHealthValue;
    [SerializeField] private Slider HealthDepict;
    [SerializeField] private TMP_Text EnemyName;

    [SerializeField] private EnemyUIDamageFloatControl[] DamageFloaters;

    private void Awake()
    {
        Debug.Log("A");
    }

    void Start()
    {
        Camera = Camera.main;
        Debug.Log("B");
    }

    public void Init(float _maxHealth, string _enemyName)
    {
        Debug.Log("C");
        RelativeHealthValue = 1.0f / _maxHealth;
        EnemyName.text = _enemyName;
    }

    /// <summary>
    /// Updated die Lebensanzeige und lässt einen Schadenstext hochfliegen
    /// </summary>
    /// <param name="_remainingHP"></param>
    /// <param name="_isHeavyDamage"></param>
    /// <param name="_damageCaused"></param>
    public void HealthUpdate(float _remainingHP, bool _isHeavyDamage, float _damageCaused)
    {
        HealthDepict.value = _remainingHP * RelativeHealthValue;
        for (int i = 0; i < DamageFloaters.Length; i++)
        {
            if(DamageFloaters[i].IsInProcess)continue;
            DamageFloaters[i].DamageFloat(_damageCaused, _isHeavyDamage);
            break;
        }
    }
    /// <summary>
    /// Eine Überladung sodass Heilung möglich ist
    /// </summary>
    /// <param name="_newHp"></param>
    public void HealthUpdate(float _newHp)
    {
        HealthDepict.value = _newHp;
    }

    void Update()
    {
        if (Camera)
            HolderBody.transform.LookAt(Camera.transform);
    }
}