using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Game close");
        Application.Quit();
    }

    public void SceneBase()
    {
        AsyncOperation locationAfterSceneLoad = SceneManager.LoadSceneAsync(0);
    }
}
