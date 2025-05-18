using UnityEngine;

public class NotDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // делаем объект неуничтожаемым при загрузке новой сцены
    }
}
