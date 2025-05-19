using Examples.AbstractFactoryExample.Unit.Warrior;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _canvasDeadPlayer;
    [SerializeField] private GameObject _localAtive;
    [SerializeField] private GameObject _fadeIn;
    [SerializeField] private Animator _animatorPlayerVisual;

    [SerializeField] private WeakWarrior _newLiveEnemy;
    private bool _firstSave = true;

    public bool FirstSave
    {
        get { return _firstSave; }
        set { _firstSave = value; }
    }

    private void Awake()
    {
        Instance = this;

    }


    public void GameOver()
    {
        Debug.Log("Game Over!");

        LoadGameLastSave();

    }

    public void StartCutsceneIfFirstScene()
    {
        if (FirstSave)
        {
            StartCutscne.Instance.StartCutScene();
        }
        _localAtive.SetActive(true);

    }

    private void LoadGameLastSave()
    {
        PreparingToLoadLastSave();

        _animatorPlayerVisual.Rebind(); // ������� ��� �������� � ���������
        _animatorPlayerVisual.Update(0f); // �������������� ����������

        SaveLoadManager.LoadGame();

        StartCutsceneIfFirstScene();
    }

    private void FadeInAfterRespawn()
    {
        _fadeIn.SetActive(true);
        _fadeIn.SetActive(false);
    }

    private void PreparingToLoadLastSave()
    {
        Player.Instance.Initialize();

        _canvasDeadPlayer.SetActive(false);

        FrameSwitch.UpdateFrameCache();

        FadeInAfterRespawn();
    }


}
