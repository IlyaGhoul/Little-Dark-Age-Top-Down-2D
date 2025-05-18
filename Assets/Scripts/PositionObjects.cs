using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Transform))]
public class PositionObjects : MonoBehaviour
{
    private Rigidbody2D _rb;

    private Transform _transformPosition;

    private float _minMovingSpeed = 0.1f;

    private static bool _isRunning = false;

    public Transform TransformPosition
    {
        get { return _transformPosition; }
        set
        {
            _transformPosition = value;
        }
    }

    private void Awake()
    {
        _transformPosition = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        GetPositionCoordinate();
    }

    private void GetPositionCoordinate()
    {
        if (_rb.linearVelocity.sqrMagnitude > _minMovingSpeed * _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    public static bool IsRunningBool()
    {
        return _isRunning;
    }
}
