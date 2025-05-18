using Examples.AbstractFactoryExample.Unit;
using Examples.AbstractFactoryExample.Unit.Archer;
using Examples.AbstractFactoryExample.Unit.Warrior;
using UnityEngine;

namespace Examples.AbstractFactoryExample
{
    public abstract class UnitsFactory
    {
        public abstract Warrior CreateWarrior(GameObject gameObject, Vector2 position);

        public abstract Archer CreateArcher(GameObject gameObject, Vector2 position);
    }
}