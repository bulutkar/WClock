using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    public static PrefabFactory Instance;
    public GameObject rightClickCanvas;
    public GameObject doneCanvas;

    private void Awake()
    {
        Instance = this;
    }
}
