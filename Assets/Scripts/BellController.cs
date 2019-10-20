using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BellController : MonoBehaviour
{
    [SerializeField] private RemainderContainer _remainder;
    [SerializeField] private DayController dayController;
    private GameObject rightClickCanvas;
    private GameObject doneCanvas;

    private Button _deleteButton;
    private Button _editButton;

    private TextMeshProUGUI _text;
    private Button _doneButton;
    private AudioSource _audioSource;

    [SerializeField] private bool _isDone;

    private void Awake()
    {
        rightClickCanvas = Instantiate(PrefabFactory.Instance.rightClickCanvas, PrefabFactory.Instance.rightClickCanvas.transform.position, Quaternion.identity, transform);
        _editButton = rightClickCanvas.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        _deleteButton = rightClickCanvas.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        _editButton.onClick.AddListener(OnEdit);
        _deleteButton.onClick.AddListener(OnDelete);

        doneCanvas = Instantiate(PrefabFactory.Instance.doneCanvas, PrefabFactory.Instance.doneCanvas.transform.position, Quaternion.identity, transform);
        _text = doneCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _doneButton = doneCanvas.transform.GetChild(2).GetComponent<Button>();

        gameObject.AddComponent<AudioSource>();
        _audioSource = GetComponent<AudioSource>();

        _remainder = new RemainderContainer();
    }

    void Start()
    {
        rightClickCanvas.transform.position = transform.position + Vector3.right * 0.5f;
    }

    private void Update()
    {
        if (!_isDone && _remainder != null)
        {
            var timeSpan = _remainder.DateTime - DateTime.Now;
            if (timeSpan.TotalSeconds <= 0)
            {
                _isDone = true;
                _audioSource.clip = SoundManager.Instance.GetChosenClip();
                if (_audioSource.clip != null) _audioSource.Play();
                _text.text = _remainder.Text;
                _doneButton.onClick.AddListener((() =>
                {
                    dayController.RemoveRemainder(_remainder);
                    doneCanvas.SetActive(false);
                    _doneButton.onClick.RemoveAllListeners();
                }));
                doneCanvas.SetActive(true);
            }
        }
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
        _isDone = false;
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