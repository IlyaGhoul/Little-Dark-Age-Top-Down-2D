using UnityEngine;
using UnityEngine.UI;

public class MenuPlaySaveLoad : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;

    [SerializeField] private Player _player;

    private void Awake()
    {
        _saveButton.onClick.AddListener(SaveGame);
        _loadButton.onClick.AddListener(LoadGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameObject.SetActive(!_gameObject.activeSelf);
        }
    }

    private void SaveGame()
    {
        SaveLoadManager.SaveGame();
    }

    private void LoadGame()
    {
        SaveLoadManager.LoadGame();
    }
}
