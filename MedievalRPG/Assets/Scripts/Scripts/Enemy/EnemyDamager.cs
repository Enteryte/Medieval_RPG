using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging;
    private void OnTriggerEnter(Collider _collision)
    {
        if(!IsDamaging)
            return;
        if (_collision.gameObject == GameManager.instance.playerGO)
        {
            Debug.Log("Player Hit");
            _collision.gameObject.GetComponent<GotDamage>().GotHit(true);
            Attack();
        }
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
    private void Attack()
    {
        //Do Damage to the Player Health here. The Debug is to be replaced with that.
        PlayerValueManager.instance.currHP -= Damage;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.currHP;


        Debug.Log($"{Damage} launched");
        IsDamaging = false;
    }
}