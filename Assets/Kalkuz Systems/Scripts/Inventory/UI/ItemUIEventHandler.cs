using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIEventHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) InventoryUIManager.Instance.ChildClicked(transform.GetSiblingIndex());
        if (eventData.button == PointerEventData.InputButton.Right) InventoryUIManager.Instance.UseItem(transform.GetSiblingIndex());

        print(eventData.clickCount);
    }
}
