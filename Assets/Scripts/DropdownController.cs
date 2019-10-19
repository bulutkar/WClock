using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    [SerializeField] private int startRange = 0;
    [SerializeField] private int count = 1;
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        var list = Enumerable.Range(startRange, count).ToList();
        List<string> list2 = new List<string>();
        foreach (var i in list)
        {
            list2.Add(i.ToString());
        }
        _dropdown.AddOptions(list2);
    }
}
