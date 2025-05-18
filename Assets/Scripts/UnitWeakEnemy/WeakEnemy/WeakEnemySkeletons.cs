using UnityEngine;
using UnityEngine.AI;
using ThisGame.Utils;

[RequireComponent(typeof(NavMeshAgent))]
public class WeakEnemySkeletons : MonoBehaviour
{
    //настройки брожения
    [SerializeField] private LogicStateEnemy _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    private NavMeshAgent _navMeshAgent;
    private LogicStateEnemy stateEnemy;

    private Vector3 _roamingPosition;
    private Vector3 _startingPosition;

    private float _roamingTime;

    private enum LogicStateEnemy
    {
        Idle,
        Roaming,
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false; // не менялась ориентация
        stateEnemy = _startingState;
    }

    public static bool isAgentMoving = false;

    private void GetPositionCoordinate()
    {
        isAgentMoving = _navMeshAgent.velocity.magnitude > 0.01f || _navMeshAgent.pathPending;
    }

    private void Update()
    {
        GetPositionCoordinate();

        switch (stateEnemy)
        {
            default:
            case LogicStateEnemy.Idle:
                break;
            case LogicStateEnemy.Roaming:
                    LogicEnemyRoaming();           
                break;
        }
    }
                            
    public void LogicEnemyRoaming()
    {
        _roamingTime -= Time.deltaTime;

        if (_roamingTime < 0)
        {
            Roaming(); // Теперь вызываем Roaming после паузы
            _roamingTime = _roamingTimerMax;
        }
    }
    
    // поиск новой точки
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(_startingPosition, _roamingPosition);
        _navMeshAgent.SetDestination(_roamingPosition);
    }       

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + GameUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}




