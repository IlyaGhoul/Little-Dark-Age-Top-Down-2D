using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUpUIController : MonoBehaviour
{
    public static ItemPickUpUIController Instance { get; private set; }

    public GameObject PopupPrefab;
    public int MaxPopups = 5;
    public float PopupDuration = 3f;


    private readonly Queue<GameObject> _activePopups = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(PopupPrefab, transform);

        newPopup.GetComponentInChildren<Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();

        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }

        _activePopups.Enqueue(newPopup);

        if (_activePopups.Count > MaxPopups)
        {
            Destroy(_activePopups.Dequeue());
        }

        StartCoroutine(FadeOutDestroy(newPopup));
    }

    private IEnumerator FadeOutDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(PopupDuration);
        if (popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();

        for (float timePassed = 0f;  timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);

    }
}
