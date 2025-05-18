using UnityEngine;
using UnityEngine.UI;


public class ShowActionTakeItem : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private Color _color;
    [SerializeField] private string[] _phraseForAction;
    [SerializeField] private GameObject _gameObject;

    public static ShowActionTakeItem Instance { get; private set; }

    private void Awake()                                                           
    {
        if (Instance == null) Instance = this;       
    }                                                                                                                                           
                                                                                                                                   
    public void ShowActionForTakeItem(int idPhrase)
    {
        _gameObject.SetActive(true);
        
        _text.text = _phraseForAction[idPhrase];
    }
    
    public void CloseActionForTakeItem()
    {
        _gameObject.SetActive(false);                                   
    }
    
    public string ChoosePhraseForAction(int id)
    {
        return _phraseForAction[id];
    }
}
