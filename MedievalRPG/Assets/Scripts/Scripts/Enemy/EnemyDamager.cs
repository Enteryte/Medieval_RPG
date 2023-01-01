using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging;
    private void OnCollisionEnter(Collision _collision)
    {
        if(!IsDamaging)
            return;
        if (_collision.gameObject == GameManager.instance.playerGO)
            Attack();
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
        IsDamaging = true;
    }
    private void Attack()
    {
        //Do Damage to the Player Health here. The Debug is to be replaced with that.
        Debug.Log($"{Damage} launched");
        IsDamaging = false;
    }
}