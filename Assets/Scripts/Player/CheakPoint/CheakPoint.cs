using UnityEngine;
using UnityEngine.UI;

public class CheakPoint : MonoBehaviour
{
    private bool _isCheckPoint;
    [SerializeField] private GameManager _gameManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isCheckPoint == true)
        {
            PlayerSounds.Instance.PlayUseCheckPoint();
            SaveLoadManager.SaveGame();

            _gameManager.FirstSave = false;

            Debug.Log("Сохранение выполнено");
            SaveLoadManager.LoadGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            ShowActionTakeItem.Instance.ShowActionForTakeItem(1);

            _isCheckPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            ShowActionTakeItem.Instance.CloseActionForTakeItem();

            _isCheckPoint = false;

        }
    }
}
