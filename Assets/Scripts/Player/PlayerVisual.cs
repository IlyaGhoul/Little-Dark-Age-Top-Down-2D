using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private LayerMask _blockingUILayer;
    [SerializeField] private StaminaStatusBar StaminaStatusBar;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerSounds _soundPlayer;

    private readonly int XMove = Animator.StringToHash("xMove");
    private readonly int YMove = Animator.StringToHash("yMove");
    private readonly int AttackTrigger = Animator.StringToHash("Attack");
    private readonly int IsWalking = Animator.StringToHash("IsWalking");
    private readonly int IsDie = Animator.StringToHash("IsDie");

    private Animator _animator;

    private bool _isAttack = true;

    public bool IsAttack()
    {
        return _isAttack;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateMovementAnimation();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;

    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IsDie, true);
        PlayerSounds.Instance.PlayDeath();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (CanAttack() && _player.IsTryUseStamina(_player.StaminaAttackCost))
        {
            _animator.SetTrigger(AttackTrigger);
            PauseProcessingAttack();
            StartCoroutine(AttackCooldown());
            GameInput.Instance._playerInputActions.Player.Move.Disable(); 
        }
    }

    private bool CanAttack()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    private IEnumerator AttackCooldown()
    {
        _isAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _isAttack = true;
    }

    private void PauseProcessingAttack()
    {    
        GameInput.Instance._playerInputActions.Player.Move.Disable(); // обработка паузы
        GameInput.Instance._playerInputActions.Player.Attack.Disable();

        Invoke(nameof(EndMove), 1f);
        Invoke(nameof(EndAttack), 1f);
    }

    private void EndMove()
    {
        GameInput.Instance._playerInputActions.Player.Move.Enable(); // Разблокируем WASD
    }

    private void EndAttack()
    {
        GameInput.Instance._playerInputActions.Player.Attack.Enable();
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = Player.IsMoving();

        _animator.SetBool(IsWalking, isMoving);

        if (isMoving)
        {
            _soundPlayer.PlayFootstep();
            _animator.SetFloat(XMove, Player.Instance.InputVector.x);
            _animator.SetFloat(YMove, Player.Instance.InputVector.y);
        }
    }

    

}




