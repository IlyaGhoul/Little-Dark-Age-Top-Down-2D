using UnityEngine;
using UnityEngine.Playables;

public class StartCutscne : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] director; // —сылка на Timeline
    [SerializeField] private GameObject[] _setNotActive;
    [SerializeField] private GameObject _backgroundActiveStartScene;
    [SerializeField] private GameObject _cutScene;
    public static StartCutscne Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void StartCutScene()
    {
        foreach (var obj in _setNotActive)
        {
            obj.SetActive(false);
        }

        director[0].Play();
        director[0].stopped += OnCutsceneEnd;

        _backgroundActiveStartScene.SetActive(true);
        _cutScene.SetActive(true);
    }

    private void OnCutsceneEnd(PlayableDirector director)
    {
        foreach (var obj in _setNotActive)
        {
            _backgroundActiveStartScene.SetActive(false);
            obj.SetActive(true);
        }

    }
}
