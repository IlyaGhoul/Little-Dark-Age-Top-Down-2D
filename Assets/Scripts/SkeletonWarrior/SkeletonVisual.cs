using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    private Animator _animator;

    private static SpriteRenderer _spriteRenderer;

    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, WeakEnemySkeletons.isAgentMoving);
    }
}
