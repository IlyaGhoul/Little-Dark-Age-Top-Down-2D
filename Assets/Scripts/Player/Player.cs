using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Item;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(KnockBack))]
[SelectionBase]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private SpriteRenderer _playerSprite;

    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 20;
    [SerializeField] private float _damageRecoveryTime = 0.1f;

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Animator _animatorToRebind;

    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private Vector2 _inputVector = Vector2.zero;

    private static bool _isMoving = true;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    // Стамина
    private float _maxStamina = 10;
    private float _currentStamina;
    public float RegenDelay = 5f; // Задержка перед восстановлением
    public float StaminaAttackCost = 5f; // Сколько стамины отнимает удар
    public float StaminaRegenRate = 2f; // Скорость восстановления


    private Coroutine _regenDelayCorutine;

    private bool _isInvincible = false;
    private bool _isDeath;


    public int CurrentHealth    
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public float CurrentStamina
    {
        get { return _currentStamina; }
        set { _currentStamina = value; }
    }

    public float MaxStamina
    {
        get { return _maxStamina; }
        set { _maxStamina = value; }
    }

    public Vector2 InputVector
    {
        get { return _inputVector; }
        set { _inputVector = value; }
    }

    public enum PlayerState { Idle, Walk, Attack, Death }

    private PlayerState CurrentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Удаляем дубликат
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Переносим между сценами

        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        CurrentState = PlayerState.Idle;
    }

    private void Start()
    {       
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;

        _canTakeDamage = true;
        _isAlive = true;
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack) { return; }

        HandleMovement();
    }

    private void Update()
    {
        if (_isDeath == true)
        {
            CurrentState = PlayerState.Death;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            _inputVector = GameInput.Instance.GetMovementVector();
            UpdateState();
            UpdateMovement();
        }
        
    }

    public void Initialize()
    {
        _isDeath = false;
        _isInvincible = false;
        _canTakeDamage = true;
        _isAlive = true;
        CurrentState = PlayerState.Idle;
    }

    public static bool IsMoving()
    {
        return _isMoving;
    }

    public void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (_movingSpeed * Time.fixedDeltaTime));
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {           
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockedBack(damageSource);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
        

    }
    public bool IsTryUseStamina(float amount)
    {
        if (_currentStamina >= amount)
        {
            _currentStamina -= amount;

            if (_regenDelayCorutine != null)
            {
                StopCoroutine( _regenDelayCorutine );
            }

            _regenDelayCorutine = StartCoroutine(RegenDelayCoroutine());

            return true;
        }
        else
        {
            return false;
        }
    }


    private IEnumerator RegenDelayCoroutine()
    {
        yield return new WaitForSeconds(RegenDelay);

        // Восстановление после задержки
        while (CurrentStamina < MaxStamina)
        {
            CurrentStamina += StaminaRegenRate * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
            yield return null;
        }
    }

    private void UpdateMovement()
    {
        if (CurrentState == PlayerState.Death) return;
        // Оптимизация: обновляем параметры только при изменении ввода
        Vector2 input = _inputVector;
        _isMoving = input != Vector2.zero;

        if (_isMoving)
        {
            CurrentState = PlayerState.Walk;
        }
        else
        {
            CurrentState = PlayerState.Idle;
        }
    }

    private void UpdateState()
    {
        // Проверка завершения анимаций
        if (CurrentState == PlayerState.Attack)
        {
            CurrentState = PlayerState.Idle;
        }
    } 

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _isAlive)
        {
            _isAlive = false;


            _isDeath = true;


          
            _playerSprite.color = Color.white;
            _gameOverUI.SetActive(true);

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);

        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        PlayerSounds.Instance.PlayTakeHit();

        _isInvincible = true;

        Color originalColor = _playerSprite.color;

        _playerSprite.color = Color.red;

        yield return new WaitForSeconds(_damageRecoveryTime);

        _playerSprite.color = originalColor;

        _isInvincible = false;     
        _canTakeDamage = true;
    }

  
}
