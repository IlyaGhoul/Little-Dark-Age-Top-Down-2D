using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _loadingScreen;

    public void PlayGame()
    {
        StartCoroutine(LoadGameAfterSceneLoad());
        
    }

    public void ExitGame()
    {
        Debug.Log("Game close");
        Application.Quit();
    }

    private IEnumerator LoadGameAfterSceneLoad()
    {
        _loadingScreen.SetActive(true);

        AsyncOperation locationAfterSceneLoad = SceneManager.LoadSceneAsync(1);
        locationAfterSceneLoad.allowSceneActivation = false;

        // ∆дЄм, пока сцена загрузитс€ на 90% 
        while (locationAfterSceneLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(locationAfterSceneLoad.progress / .9f);
            _slider.value = progress;
            yield return null;
        }

        locationAfterSceneLoad.allowSceneActivation = true;

        // ∆дЄм завершени€ загрузки сцены
        while (!locationAfterSceneLoad.isDone)
        {
            yield return null;
        }

        

        _loadingScreen.SetActive(false);
    }
}
