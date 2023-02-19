using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyUIDamageFloatControl : MonoBehaviour
{
   private TMP_Text DamageText;
   private Animator Anim;
   [SerializeField] private float DamageMultiplier = 1656;
   [SerializeField] private Color LightDamageColor;
   [SerializeField] private Color HeavyDamageColor;
   [SerializeField] private float LightDamageTextSize = 0.18f;
   [SerializeField] private float HeavyDamageTextSize = 0.32f;

   private const float INITIAL_Y_POS = -1.0f;
   private const float X_RANDOM_POS_RANGE = 0.5f;

   public bool IsInProcess { get; private set; }


   private void Start()
   {
      DamageText = GetComponent<TMP_Text>();
      Anim = GetComponent<Animator>();
   }

   public void DamageFloat(float _damage, bool _isHeavyDamage)
   {
      IsInProcess = true;
      float depictedDamage = _damage * DamageMultiplier;
      string damageString = depictedDamage.ToString();
      int damageStringLength = damageString.Length;
      while (damageStringLength > 3)
      {
         damageStringLength -= 3;
         damageString = damageString.Insert(damageStringLength, ".");
      }
      DamageText.text = damageString;
      DamageText.color = _isHeavyDamage ? HeavyDamageColor : LightDamageColor;
      DamageText.fontSize = _isHeavyDamage ? HeavyDamageTextSize : LightDamageTextSize;
      transform.position = new Vector3(Random.Range(-X_RANDOM_POS_RANGE, X_RANDOM_POS_RANGE),INITIAL_Y_POS);
      Anim.SetTrigger(Animator.StringToHash("DamageTrigger"));
   }

   private void EndingEvent()
   {
      IsInProcess = false;
   }
}
