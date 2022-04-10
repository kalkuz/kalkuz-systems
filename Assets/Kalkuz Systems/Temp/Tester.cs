using System;
using System.Collections.Generic;
using KalkuzSystems.Analysis.Debugging;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [ContextMenu("Log")]
    public void Log()
    {
        KalkuzLogger.Info("Info message");
    }
}