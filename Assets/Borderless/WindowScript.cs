using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowScript : MonoBehaviour, IDragHandler, IPointerClickHandler
{

    private Vector2 _deltaValue = Vector2.zero;
    private bool _maximized;
    private bool _bordered;

    public void OnCloseBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Application.Quit();
    }

    public void OnMinimizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        BorderlessWindow.MinimizeWindow();
    }

    public void OnMaximizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (_maximized)
            BorderlessWindow.RestoreWindow();
        else
            BorderlessWindow.MaximizeWindow();

        _maximized = !_maximized;
        AspectRatioController.SetReso();
    }

    public void OnBorderButton()
    {
        if (_bordered)
        {
            BorderlessWindow.SetFramelessWindow();
            _bordered = false;
        }
        else
        {
            BorderlessWindow.SetFramedWindow();
            _bordered = true;
        }
    }
    public void OnDrag(PointerEventData data)
    {
        if (BorderlessWindow.framed)
            return;
        _deltaValue += data.delta;
        if (data.dragging)
        {
            BorderlessWindow.MoveWindowPos(_deltaValue, Screen.width, Screen.height);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) CanvasController.Instance.CloseActiveCanvas();
    }
}
