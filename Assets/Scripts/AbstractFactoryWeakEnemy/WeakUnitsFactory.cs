using Examples.AbstractFactoryExample.Unit;
using Examples.AbstractFactoryExample.Unit.Archer;
using Examples.AbstractFactoryExample.Unit.Warrior;
using UnityEngine;

namespace Examples.AbstractFactoryExample
{
    [RequireComponent(typeof(WeakWarrior))]
    public class WeakUnitsFactory : UnitsFactory
    {
        public override Warrior CreateWarrior(GameObject prefab, Vector2 position)
        {
            if (prefab == null)
                throw new System.Exception("Prefab not found");

            GameObject warriorInstance = GameObject.Instantiate(prefab, position, Quaternion.identity);

            if (!warriorInstance.TryGetComponent<WeakWarrior>(out var warrior))
            {
                GameObject.Destroy(warriorInstance);
                throw new System.Exception("No WeakWarrior component");
            }

            warrior.Init(3.3f);
            return warrior;
        }   

        public override Archer CreateArcher(GameObject gameObject, Vector2 position)
        {
            // need create ArcherSkeleton
            return null;
        }
    }
}