﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private List<GameObject> activeGroup;
    private List<GameObject> inactiveGroup;
    public GameObject projectable;
    public int size;
    void Start()
    {
        activeGroup = new List<GameObject>();
        inactiveGroup = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            inactiveGroup.Add(projectable.GetComponent< ProjectableFactory>().BulletFactory(this));
        }
    }

    private bool HalfEmpty()
    {
        if (inactiveGroup.Count > 1)
        {
            return true;
        }
        return false;
    }

    internal void SetEllement(GameObject go)
    {
        inactiveGroup.Add(go);
        activeGroup.Remove(go);
    }

    public GameObject GetEllement()
    {
        GameObject go;
        if (!HalfEmpty())
        {
            inactiveGroup.Add(projectable.GetComponent<ProjectableFactory>().BulletFactory(this));
        }
        go = inactiveGroup[0];
        inactiveGroup.Remove(go);
        activeGroup.Add(go);
        return go;
    }
}