using UnityEngine;

namespace Examples.AbstractFactoryExample.Unit.Warrior
{
    public class WeakWarrior : Warrior
    {
        [SerializeField] private float _weakWarriorKoef = 0f;

        public void Init(float weakWarriorKoef)
        {
            _weakWarriorKoef = weakWarriorKoef;
        }

        public override void MeleeCombat()
        {
            // do somthing
        }
    }
}
