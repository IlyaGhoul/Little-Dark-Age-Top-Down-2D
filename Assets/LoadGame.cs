using UnityEngine;

public class LoadGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveLoadManager.LoadGame();
        GameManager.Instance.StartCutsceneIfFirstScene();
        Debug.Log(GameManager.Instance.FirstSave);
    }

    
}
