using Examples.AbstractFactoryExample.Unit.Warrior;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Examples.AbstractFactoryExample.Unit
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class KnightBossVisual : MonoBehaviour
    {
        [SerializeField] private WeakWarrior _weakWarrior;
        [SerializeField] private EnemyAI _enemyAI;
        [SerializeField] private Unit _enemyEntity;
        [SerializeField] private GameObject _enemyShadow;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private readonly int TakeHit = Animator.StringToHash("TakeHit");
        private readonly int IsDie = Animator.StringToHash("IsDie");

        private const string IS_RUNNING = "IsRunning";
        private const string ATTACK = "Attack";
        private bool _IsDeath = false;

        // Задержка после удара
        [SerializeField] private float _attackCooldownAfterHit = 0.7f;
        private bool _canAttack = true;
        private float _lastHitTime;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _enemyAI.OnEnemyAttack += _slaveSkeletonLogic_OnEnemyAttack;
            _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
            _enemyEntity.OnDeath += _enemyEntity_OnDeath;
        }

        private void Update()
        {
            _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);

            if (!_IsDeath)
            {
                AdjustPlayerFacingDirection();
            }

        }

        private void OnDestroy()
        {
            _enemyAI.OnEnemyAttack -= _slaveSkeletonLogic_OnEnemyAttack;
            _enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
            _enemyEntity.OnDeath -= _enemyEntity_OnDeath;

            CancelInvoke(nameof(EndAttackCooldown));
        }

        public void TriggerAttackAnimationTurnOff()
        {
            _weakWarrior.PolygonColliderTurnOff();
        }

        public void TriggerAttackAnimationTurnOn()
        {
            TriggerAttackAnimationTurnOff();
            _weakWarrior.PolygonColliderTurnOn();
        }

        private void _slaveSkeletonLogic_OnEnemyAttack(object sender, System.EventArgs e)
        {
            if (!_canAttack || _IsDeath) return;

            _animator.SetTrigger(ATTACK);
        }

        public void AdjustPlayerFacingDirection()
        {
            Vector3 knightBossPosition = EnemyAI.Instance.GetKnightBossScreenPosition();
            Vector3 playerPosition = EnemyAI.Instance.GetTargetPlayerScreenPosition();

            if (knightBossPosition.x < playerPosition.x)
            {
                _spriteRenderer.flipX = false;
                _enemyEntity.ApplyPaths(_enemyEntity._mirroredPaths);
            }
            else
            {
                _spriteRenderer.flipX = true;
                _enemyEntity.ApplyPaths(_enemyEntity._originalPaths);
            }
        }

        private void StartAttackCooldown()
        {
            _canAttack = false;
            _lastHitTime = Time.time;

            // Отменяем предыдущий вызов, чтобы избежать наложения
            CancelInvoke(nameof(EndAttackCooldown));
            Invoke(nameof(EndAttackCooldown), _attackCooldownAfterHit);
        }

        private void EndAttackCooldown()
        {
            _canAttack = true;
        }

        private void _enemyEntity_OnDeath(object sender, EventArgs e)
        {
            _animator.SetBool(IsDie, true);
            _spriteRenderer.sortingOrder = 1;
            _enemyShadow.SetActive(false);
            _IsDeath = true;
        }

        private void _enemyEntity_OnTakeHit(object sender, EventArgs e)
        {
            _animator.SetTrigger(TakeHit);
            StartAttackCooldown(); // Добавляем задержку после удара
        }
    }
}
