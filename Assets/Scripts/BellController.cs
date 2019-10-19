using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellController : MonoBehaviour
{
    [SerializeField] private RemainderContainer _remainder;
    [SerializeField] private DayController dayController;
    [SerializeField] private GameObject rightClickCanvas;

    private Button _deleteButton;
    private Button _editButton;

    private void Awake()
    {
        _remainder = new RemainderContainer();
        _editButton = rightClickCanvas.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        _deleteButton = rightClickCanvas.transform.GetChild(0).GetChild(1).GetComponent<Button>();

        _editButton.onClick.AddListener(OnEdit);
        _deleteButton.onClick.AddListener(OnDelete);

    }

    void Start()
    {
        rightClickCanvas.transform.position = transform.position + Vector3.right * 0.5f;
    }

    void OnMouseOver()
    {
        if (CanvasController.IsMainCanvasOpen) return;
        if (Input.GetMouseButtonDown(0))
        {
            OnEdit();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CanvasController.Instance.CloseActiveCanvas();
            CanvasController.Instance.AddActiveCanvas(rightClickCanvas);
            rightClickCanvas?.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _editButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.RemoveAllListeners();
    }

    public void AddRemainder(RemainderContainer remainder)
    {
        _remainder = remainder;
    }

    public void OnDelete()
    {
        CanvasController.Instance.CloseActiveCanvas();
        dayController.RemoveRemainder(_remainder);
    }

    public void DeleteOnEdit()
    {
        OnDelete();
        EditController.Instance.SaveOnClick();
        EditController.Instance.saveButton.onClick.RemoveAllListeners();
    }
    public void OnEdit()
    {
        CanvasController.Instance.CloseActiveCanvas();
        EditController.Instance.saveButton.onClick.AddListener(DeleteOnEdit);
        EditController.Instance.LoadCanvas(_remainder);
    }
}