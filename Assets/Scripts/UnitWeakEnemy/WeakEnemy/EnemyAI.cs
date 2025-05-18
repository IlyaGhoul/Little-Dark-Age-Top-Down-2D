using UnityEngine;

public class WeakLogicRoamingILevelEasy : MonoBehaviour
{
    public static WeakLogicRoamingILevelEasy Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;

    public GameInput gameInput { get; private set; }

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        gameInput = gameObject.AddComponent<GameInput>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

    }
    public bool IsRunning()
    {
        return isRunning;
    }


}
