using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameObject.transform.position = new Vector3();
    }


}
