using UnityEngine;

namespace Examples.AbstractFactoryExample.Unit.Archer
{
    public class WeakArcher : Archer
    {
        [SerializeField] private float _fireArrowDamage;
        public void Init(float fireArrowDamage, float rangeDistance)
        {
            _fireArrowDamage = fireArrowDamage;
            base.Init(rangeDistance);
        }
        public override void Shoot()
        {
            // shoot fire arrow
        }
    }
}
