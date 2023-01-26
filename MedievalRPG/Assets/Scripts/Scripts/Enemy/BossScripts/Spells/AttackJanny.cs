using UnityEngine;

public class AttackJanny : MonoBehaviour
{
    //In order to clean up the boss attacks, pooling them doesn't sound like a worthwhile effort, which is why I create destroy them instead.
    public void CleanUp()
    {
        Destroy(gameObject);
    }
}
