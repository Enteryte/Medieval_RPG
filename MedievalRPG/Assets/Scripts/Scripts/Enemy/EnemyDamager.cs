using UnityEngine;
using UnityEngine.AI;

public class EnemyDamager : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging;

    private void OnTriggerEnter(Collider _collision)
    {
        if (!IsDamaging || _collision.gameObject == GameManager.instance.playerGO)
            return;
        Attack(_collision.gameObject);
    }

    public void Init(float _damage)
    {
        Damage = _damage;
    }

    public void DamageOn()
    {
        IsDamaging = true;
    }

    public void DamageOff()
    {
        IsDamaging = false;
    }

    private void Attack(GameObject _playerGameObject)
    {
        _playerGameObject.GetComponent<GotDamage>().GotHit(true);
        PlayerValueManager.instance.CurrHP -= Damage;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;
        IsDamaging = false;
    }
}