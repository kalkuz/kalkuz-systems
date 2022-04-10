using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Inventory;
using KalkuzSystems.Inventory.Item;
using UnityEngine;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; set; }
    public Canvas canvas;
    public Inventory bond;

    public Item pointerOccupance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            canvas.enabled = !canvas.enabled;
        }
    }

    public void SetInventory()
    {
        for (int x = 0; x < bond.sizeX; x++)
        {
            for (int y = 0; y < bond.sizeY; y++)
            {
                Transform child = transform.GetChild(x + y * bond.sizeX);
                child.GetChild(0).GetComponent<TextMeshProUGUI>().text = bond.GetItem(x, y) == null ? "" : bond.GetItem(x, y).itemName;
                child.GetChild(1).GetComponent<TextMeshProUGUI>().text = bond.GetItem(x, y) == null ? "" : bond.GetItem(x, y).currentStacks.ToString();
            }
        }
    }

    public void ChildClicked(int childIndex)
    {
        int x = childIndex % bond.sizeX;
        int y = childIndex / bond.sizeX;

        pointerOccupance = bond.ReplaceItem(x, y, pointerOccupance);
        GUIRepaint();
    }

    public void UseItem(int childIndex)
    {
        int x = childIndex % bond.sizeX;
        int y = childIndex / bond.sizeX;

        if (bond.GetItem(x, y) is UsableItem)
        {
            var item = (bond.GetItem(x, y) as UsableItem);
            item.Consume(bond.GetComponent<KalkuzSystems.Battle.CharacterData>());
            if (item.currentStacks == 0) bond.SetItem(x, y, null);
        }

        GUIRepaint();
    }

    public void GUIRepaint()
    {
        SetInventory();
    }
}
