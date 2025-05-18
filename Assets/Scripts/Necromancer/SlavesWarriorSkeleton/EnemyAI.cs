using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;

    [SerializeField] private bool _isAttackEnemy = false;
    private float _attackRate = 2f;
    private float _nextAttackTime = 0;
    private float _attackDistance = 2.5f;

    [SerializeField] private float _moveingSpeed = 5f;
    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _stoppingDistance = 2f;

    [SerializeField] private PolygonCollider2D _polygonCollider;
    [SerializeField] private GameObject _setActiveLocal;


    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Transform _transformPositionSkeleton;
    [SerializeField] private Transform _transformPositionKnightBoss;

    private NavMeshAgent _navMeshAgent;

    public static EnemyAI Instance { get; private set; }
    public event EventHandler OnEnemyAttack;

    public bool shouldBeSaved = false; // Отметьте этого врага в инспекторе

    private State _currentState;

    private enum State
    {
        Idle,
        Roaming,
        Attacking,
        Death
    }

    public bool IsRunning
    {
        get
        {
            if (_navMeshAgent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;


        _currentState = _startingState;

        

    }

    private void Update()
    {
        if (_currentState != State.Death)
        {
            CheckCurrentState();
            HandleStateBehavior();
        }
    }

    // разворот в сторону главного героя
    public Vector3 GetTargetPlayerScreenPosition()
    {
        Vector3 targetPlayePosition = _targetPlayer.position;

        return targetPlayePosition;
    }

    public Vector3 GetKnightBossScreenPosition()
    {
        Vector3 knightBossPosition = _transformPositionKnightBoss.position;
        return knightBossPosition;
    }

    public Vector3 GetSkeletonScreenPosition()
    {
        Vector3 skeletonPosition = _transformPositionSkeleton.position;
        return skeletonPosition;
    }

    public void SetDeathState()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    public void SetNewLive()
    {
        _navMeshAgent.isStopped = false;      
        _currentState = State.Roaming;
    }

    private void HandleStateBehavior()
    {
        if (_currentState != State.Attacking && _polygonCollider.enabled == true) _polygonCollider.enabled = false;

            switch (_currentState)
            {
                case State.Roaming:
                    HandleMovement();
                    break;
                case State.Attacking:
                    AttackingTarget();
                    break;
                case State.Death:
                    break;
                default:
                case State.Idle:
                    break;
            }
    }

    private void HandleMovement()
    {
        if (!_setActiveLocal.activeSelf)
        {
            _currentState = State.Idle;
            _navMeshAgent.ResetPath(); 
        }
        else
        {
            if (_targetPlayer != null)
            {
                _navMeshAgent.speed = _moveingSpeed;

                Vector3 direction = (_targetPlayer.position - transform.position).normalized;

                _navMeshAgent.SetDestination(transform.position + direction * _moveingSpeed);
            }
        }
        
    }

    // Аттака
    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void CheckCurrentState()
    {
        if (_currentState == State.Death) { return; }

        float distanceToPlayer = Vector3.Distance(transform.position, _targetPlayer.position);

        State newState = _currentState;

        if (_isAttackEnemy && distanceToPlayer <= _attackDistance)
        {
            newState = State.Attacking;
        }
        else if (_isChasingEnemy)
        {
            newState = State.Roaming;
        }
        else
        {
            newState = State.Idle;
        }

        // Обработка изменения состояния
        if (newState != _currentState)
        {
            switch (newState)
            {
                case State.Attacking:
                    _navMeshAgent.isStopped = true;
                    break;
                case State.Roaming:
                    _navMeshAgent.isStopped = false;
                    break;
            }

            _currentState = newState;
        }
    }

}
