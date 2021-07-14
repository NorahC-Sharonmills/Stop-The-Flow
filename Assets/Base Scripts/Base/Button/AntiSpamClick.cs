using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AntiSpamClick : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            button.interactable = false;
            StartCoroutine(ReActiveButton());
        });
    }

    private IEnumerator ReActiveButton()
    {
        yield return WaitForSecondCache.GetWFSCache(.5f);
        button.interactable = true;
    }
}
