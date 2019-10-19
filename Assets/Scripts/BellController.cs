using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    [SerializeField] private GameObject rightClickCanvas;


    void Start()
    {
        rightClickCanvas.transform.position = transform.position + Vector3.right * 0.25f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) rightClickCanvas.SetActive(true);
        Debug.Log("enter");
    }

    private void OnMouseExit()
    {
        rightClickCanvas.SetActive(false);
    }
}