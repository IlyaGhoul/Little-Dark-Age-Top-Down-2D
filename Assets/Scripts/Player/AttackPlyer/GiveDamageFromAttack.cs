using UnityEngine;

namespace Examples.AbstractFactoryExample.Unit
{
    public class GiveDamageFromAttack : MonoBehaviour
    {
        [SerializeField]private int DamageMultiplier;

        private float _lastDamageTime;
        private float _damageCooldown = 0.5f; // Задержка между ударами
        [SerializeField] PlayerSounds _soundAttack;

        private void OnTriggerEnter2D(Collider2D collision)
        {           
            if (Time.time - _lastDamageTime < _damageCooldown) return;

            _soundAttack.PlayHit();

            if (collision.transform.TryGetComponent(out Unit enemyEntity))
            {
                enemyEntity.TakeDamage(DamageMultiplier);

                _lastDamageTime = Time.time;
            }
        }
    }
}
