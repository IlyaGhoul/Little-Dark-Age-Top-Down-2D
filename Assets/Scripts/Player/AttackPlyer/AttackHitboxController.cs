using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Animator))]
public class AttackHitboxController : MonoBehaviour
{
    [Header("Привязка пресетов")]
    [SerializeField] private AttackColliderPreset _attackLeft;
    [SerializeField] private AttackColliderPreset _attackRight;
    [SerializeField] private AttackColliderPreset _attackForward;
    [SerializeField] private AttackColliderPreset _attackBack;

    [Header("Компоненты")]
    [SerializeField] private PolygonCollider2D _hitboxCollider;
    [SerializeField] private Animator _animator;

    private Dictionary<string, Vector2[]> _colliderCache;

    private void Awake()
    {
        if (_hitboxCollider == null)
            _hitboxCollider = GetComponent<PolygonCollider2D>();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        ColliderCachingForFastAccess();
    }

    // Вызывается через Animation Event при атаке
    public void EnableHitbox(string animationName)
    {
        if (_colliderCache.TryGetValue(animationName, out Vector2[] points))
        {
            _hitboxCollider.points = points;
            _hitboxCollider.enabled = true;
        }
    }

    // Вызывается через Animation Event в конце атаки
    public void DisableHitBox()
    {
        _hitboxCollider.enabled = false;
    }

    private void RegisterAttack(AttackColliderPreset preset)
    {
        if (preset != null)
        {
            _colliderCache.Add(preset.AnimationName, preset.ColliderPoints);
        }
    }

    // Кэшируем коллайдеры для быстрого доступа
    private void ColliderCachingForFastAccess()
    {
        _colliderCache = new Dictionary<string, Vector2[]>(4);

        RegisterAttack(_attackLeft);
        RegisterAttack(_attackRight);
        RegisterAttack(_attackForward);
        RegisterAttack(_attackBack);
    }
}




