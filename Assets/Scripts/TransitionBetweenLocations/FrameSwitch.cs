using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView; // Только для редактора
#endif


public class FrameSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _activeFrame;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _changementPosition;

    // все FrameSwitch
    private static List<FrameSwitch> _cachedFrames = new List<FrameSwitch>();
    private static bool _cacheIsDirty = true;
    [SerializeField] private bool _isBossFight;
    [SerializeField] private BoxCollider2D _boxCollider;

    private void Awake()
    {
        if (!_cachedFrames.Contains(this))
        {
            _cachedFrames.Add(this);
        }
        _cacheIsDirty = true;
    }

    private void OnDestroy()
    {
        _cachedFrames.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Отключаем все фреймы
        foreach (var frame in _cachedFrames)
        {
            if (frame != null && frame._activeFrame != null)
                frame._activeFrame.SetActive(false);
        }

        _activeFrame.SetActive(true);
        _player.transform.position += _changementPosition;

        if (_isBossFight)
        {
            _boxCollider.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _activeFrame.SetActive(false);
        }
    }

    // Метод для обновления кеша
    public static void UpdateFrameCache()
    {
        if (!_cacheIsDirty) return;

        // Очищаем кеш от null-объектов
        _cachedFrames.RemoveAll(frame => frame == null);
        _cacheIsDirty = false;
    }
}
