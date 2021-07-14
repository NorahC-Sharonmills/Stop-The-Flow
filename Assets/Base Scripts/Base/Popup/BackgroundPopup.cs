using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public enum PopupType
{
    OOPS_POPUP,
    REMOVE_ADS_POPUP,
    OUT_OF_FUEL_POPUP,
    DEAD_POPUP,
    VICTORY_POPUP,
    OVER_LOAD_POPUP
}


public class BackgroundPopup : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PopupUIComponent[] popupRefs;
    private Dictionary<PopupType, PopupUIComponent> popupDics = new Dictionary<PopupType, PopupUIComponent>();
    private static Stack<PopupUIComponent> popups;
    private static Queue<UnityAction> actions;
    private static GameObject _me;
    private static int lastSiblingIndex;
    public static bool HideOnBackground;
    private static bool isProcessing;

    private void Awake()
    {
        foreach(var dic in popupRefs)
        {
            popupDics.Add(dic.popupType, dic);
        }

        popups = new Stack<PopupUIComponent>();
        actions = new Queue<UnityAction>();
        _me = gameObject;
        lastSiblingIndex = transform.parent.childCount - 2;

        var color = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.7f);
        gameObject.SetActive(false);
    }

    private static void Processing()
    {
        if (!isProcessing)
        {
            while (actions.Count > 0)
            {
                if (!isProcessing)
                {
                    isProcessing = true;
                    UnityAction act = actions.Dequeue();
                    act.Invoke();
                }
            }
        }
        if (popups.Count == 0)
            _me.SetActive(false);
        isProcessing = false;
    }

    public static void OverrideNewValueAlpha(float a)
    {
        _me.GetComponent<Image>().color = new Color(0, 0, 0, a);
    }

    public static void RestoreValueAlpha()
    {
        _me.GetComponent<Image>().color = new Color(0, 0, 0, 0.7f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HideOnBackground)
            return;

        if (popups.Count > 0)
        {
            var popup = popups.Peek();
            popup.TurnoffPopup();
        }

    }

    public static void AddPopupToStack(PopupUIComponent popup)
    {

        popups.Push(popup);
        actions.Enqueue(() =>
        {
            popup.transform.SetAsLastSibling();
            if (!_me.activeSelf)
            {
                _me.SetActive(true);
            }
            _me.transform.SetSiblingIndex(lastSiblingIndex);
            isProcessing = false;
        });
        Processing();
    }

    public static void RemovePopup()
    {
        actions.Enqueue(() =>
        {
            if (popups.Count > 0)
            {
                var popup = popups.Pop();
                popup.HidePopup();
                isProcessing = false;
            }
        });

        if (popups.Count > 0)
        {
            actions.Enqueue(() =>
            {
                CoroutineUtils.PlayCoroutine(() =>
                {
                    if (popups.Count > 0)
                    {
                        var secondPopup = popups.Peek();
                        secondPopup.transform.SetAsLastSibling();
                        _me.transform.SetSiblingIndex(lastSiblingIndex);
                    }
                    isProcessing = false;
                }, .5f);
            });
        }
        else
        {
            actions.Enqueue(() =>
            {
                _me.SetActive(false);
                isProcessing = false;
            });
        }



        Processing();
        //CoroutineHandler.instance.StartCoroutine(ChangePopup());
    }

    public static void PopAPopup()
    {
        if (popups.Count == 0)
            return;
        popups.Pop();
        actions.Enqueue(() =>
        {
            if (popups.Count == 0)
                _me.SetActive(false);
            isProcessing = false;
        });
        Processing();
    }


    public static IEnumerator HideAllPopup()
    {
        yield return null;
        var count = popups.Count;
        for (int i = 0; i < count; i++)
        {
            RemovePopup();
            yield return WaitForSecondCache.WAIT_TIME_ZERO_POINT_ONE;
        }
    }

    public void ShowPopup(PopupType popup)
    {
        actions.Enqueue(() =>
        {
            GameObject popupObj = GetPopup(popup);

            if (popupObj != null)
                popupObj.SetActive(true);
        });
        Processing();
    }

    public T ShowPopup<T>(PopupType popupType) where T : class
    {
        GameObject popupObj = GetPopup(popupType);

        if (popupObj != null)
            popupObj.SetActive(true);
        return popupDics[popupType] as T;
    }

    public GameObject GetPopup(PopupType popupType)
    {
        if (popupDics.ContainsKey(popupType))
            return popupDics[popupType].gameObject;

        return null;
    }

    public T GetPopup<T>(PopupType popupType) where T : class
    {
        return popupDics[popupType] as T;
    }

    public static int PopupCounter()
    {
        return popups.Count;
    }

    public static bool IsNullPopup()
    {
        return popups.Count == 0;
    }
}
