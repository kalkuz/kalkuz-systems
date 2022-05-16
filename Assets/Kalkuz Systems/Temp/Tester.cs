using System;
using KalkuzSystems.Battle;
using KalkuzSystems.Battle.SkillSystem;
using KalkuzSystems.DataStructures.Generics;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public ProjectileSkillData projectileSkillData;
    public Transform source;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            projectileSkillData.Cast(Vector3.zero, Vector3.forward, null, source);
        }
    }

    private void Start()
    {
        projectileSkillData.Cast(Vector3.zero, Vector3.forward, null, source);
    }
}