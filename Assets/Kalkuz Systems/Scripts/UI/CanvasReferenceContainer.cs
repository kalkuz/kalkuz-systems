using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasReferenceContainer : MonoBehaviour
{
    public static CanvasReferenceContainer Instance { get; set; }

    [Header("References")]
    public GameObject inventory;
    public GameObject playerUI, worldSpace;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
