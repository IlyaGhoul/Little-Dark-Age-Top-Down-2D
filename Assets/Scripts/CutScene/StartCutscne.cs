using UnityEngine;
using UnityEngine.Playables;

public class StartCutscne : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] director; // ������ �� Timeline
    [SerializeField] private GameObject[] _setNotActive;
    [SerializeField] private GameObject _backgroundActiveStartScene;

    public static StartCutscne Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCutScene();
        _backgroundActiveStartScene.SetActive(true);
    }

    public void StartCutScene()
    {
        foreach (var obj in _setNotActive)
        {
            obj.SetActive(false);
        }

        director[0].Play();
        director[0].stopped += OnCutsceneEnd;
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
