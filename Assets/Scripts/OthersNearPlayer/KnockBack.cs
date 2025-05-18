using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.3f;

    private Rigidbody2D _rb;

    private float _knockBackMovingTimer;

    public bool IsGettingKnockedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;

        if(_knockBackMovingTimer < 0)
        {
            StopKnockBackMovement();
        }
    }

    public void GetKnockedBack(Transform damageSource)
    {
        IsGettingKnockedBack = true;

        _knockBackMovingTimer = _knockBackMovingTimerMax;

        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }
                                                        
    public void StopKnockBackMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        IsGettingKnockedBack = false;
    }

}
