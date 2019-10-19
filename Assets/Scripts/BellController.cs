using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    private RemainderContainer _remainder;
    [SerializeField] private GameObject rightClickCanvas;
    [SerializeField] private GameObject leftClickCanvas;

    private void Awake()
    {
        _remainder = new RemainderContainer();
    }

    void Start()
    {
        rightClickCanvas.transform.position = transform.position + Vector3.right * 0.5f;
    }

    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (CanvasController.IsMainCanvasOpen) return;
        if (Input.GetMouseButtonDown(0))
        {
            CanvasController.Instance.CloseActiveCanvas();
            CanvasController.Instance.AddActiveCanvas(leftClickCanvas);
            leftClickCanvas?.SetActive(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CanvasController.Instance.CloseActiveCanvas();
            CanvasController.Instance.AddActiveCanvas(rightClickCanvas);
            rightClickCanvas?.SetActive(true);
        }
    }

    public void AddRemainder(RemainderContainer remainder)
    {
        _remainder = remainder;
    }
}