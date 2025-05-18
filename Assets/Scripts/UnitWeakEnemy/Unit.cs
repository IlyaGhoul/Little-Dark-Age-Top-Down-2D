using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Examples.AbstractFactoryExample.Unit
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(EnemyAI))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private EnemySO _enemySO;
        [SerializeField] private Unit _gameObject;
        // ��������� �������� ��� ������� � EnemySO
        public EnemySO EnemySO => _enemySO;
        // ��������� �������� ��� ������� � ��������� ���������
        public bool IsDefeated => _isDefeated;

        private int _currentHealth;
        private int _damageAmount;
        private bool _isDefeated;

        [SerializeField]private PolygonCollider2D _polygonCollider2D;
        private BoxCollider2D _boxCollider2D;
        private CapsuleCollider2D _capsuleCollider2D;
        private EnemyAI _enemyAI;

        public Vector2[][] _originalPaths;
        public Vector2[][] _mirroredPaths;

        public event EventHandler OnTakeHit;
        public event EventHandler OnDeath;

        [SerializeField] private Animator _animatorPlayerVisual;

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }

        private void Awake()
        {
            _gameObject.enabled = true;   // ��������� ������

            _boxCollider2D = GetComponent<BoxCollider2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _enemyAI = GetComponent<EnemyAI>();



            // ��������� ��� ����������� ����������
            if (_polygonCollider2D == null || _enemyAI == null)
            {
                Debug.LogError($"����������� ���������� �� ������� �� {name}! " +
                              $"PolygonCollider: {_polygonCollider2D != null}, " +
                              $"EnemyAI: {_enemyAI != null}", this);

                // ��������� ������ ������ ���� �� ������������� ��������� � GameObject
                if (this != null)
                {
                    enabled = false;
                }
                return;
            }

            // ������������� �����
            try
            {
                CachePaths();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"������ ��� ����������� �����: {e.Message}", this);
                enabled = false;
                return;
            }

            // ����������� � SaveLoadManager
            if (_enemySO != null)
            {
                SaveLoadManager.Instance?.RegisterEnemyUnit(this);
            }
            else
            {
                Debug.LogError($"EnemySO �� �������� �� {name}!", this);
            }
        }
        private void OnDestroy()
        {
            if (_enemySO != null && SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.UnregisterEnemyUnit(_enemySO.enemyID);
            }
        }

        private void Start()
        {
            _currentHealth = _enemySO.enemyHealth;
            _damageAmount = _enemySO.enemyDamageAmount;
        }

        // ��� ���������� ���������� ������ ��������
        //public EnemySaveData GetSaveData()
        //{
        //    return new EnemySaveData
        //    {
        //        enemyId = _enemySO.enemyID,
        //        position = transform.position,
        //        currentHelth = _currentHealth,
        //        isDefeated = _isDefeated
        //    };
        //}

        //public void LoadSaveData(EnemySaveData data)
        //{
        //    transform.position = data.position;
        //    _currentHealth = data.currentHelth;
        //    _isDefeated = data.isDefeated;
        //}

        public void PolygonColliderTurnOff()
        {
            _polygonCollider2D.enabled = false;
        }

        public void PolygonColliderTurnOn()
        {
            _polygonCollider2D.enabled = true;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            DetectDeath();
        }

        // ��������������� Poligon Collider 2D
        // ��������������� Poligon Collider 2D
        public void ApplyPaths(Vector2[][] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                _polygonCollider2D.SetPath(i, paths[i]);
            }
        }

        public void Defeat()
        {
            _isDefeated = true;
        }

        private void CachePaths()
        {
            Debug.Log(_polygonCollider2D.pathCount);
            _originalPaths = new Vector2[_polygonCollider2D.pathCount][];
            _mirroredPaths = new Vector2[_polygonCollider2D.pathCount][];

            for (int i = 0; i < _polygonCollider2D.pathCount; i++)
            {
                _originalPaths[i] = _polygonCollider2D.GetPath(i);
                _mirroredPaths[i] = new Vector2[_originalPaths[i].Length];

                // ������� ��������� ��������� ����
                for (int j = 0; j < _originalPaths[i].Length; j++)
                {
                    _mirroredPaths[i][j] = new Vector2(-_originalPaths[i][j].x, _originalPaths[i][j].y);
                }
            }
        }

        private void DetectDeath()
        {
            if (_currentHealth <= 0)
            {
                _boxCollider2D.enabled = false;
                _polygonCollider2D.enabled = false;
                _capsuleCollider2D.enabled = false;
                Defeat();

                _enemyAI.SetDeathState();

                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DetectLive()
        {
            _animatorPlayerVisual.Rebind(); // ������� ��� �������� � ���������
            _animatorPlayerVisual.Update(0f); // �������������� ����������

            _boxCollider2D.enabled = true;
            _polygonCollider2D.enabled = true;
            _capsuleCollider2D.enabled = true;

            _isDefeated = false;
               

            EnemyAI.Instance.SetNewLive();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out Player player))
            {
                player.TakeDamage(transform, _damageAmount);
            }
        }

    }
}
