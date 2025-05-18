using Examples.AbstractFactoryExample.Unit.Warrior;
using UnityEngine;

namespace Examples.AbstractFactoryExample
{
    public class Necromancer : MonoBehaviour
    {
        [SerializeField] private GameObject _skeletonWarriorPrefab;

        private UnitsFactory _currentFactory;

        void Awake()
        {
            _currentFactory = new WeakUnitsFactory();
            var warrior = _currentFactory.CreateWarrior(_skeletonWarriorPrefab, new Vector2(-10, 0));

            warrior.MeleeCombat();
        }
    }
}