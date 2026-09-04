using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class Controller : MonoBehaviour
{
    public RectTransform cursor;
    public float cursorSpeed = 800f;
    public Canvas canvas;

    public MainMenu mainMenu;

    private Vector2 cursorPosition;
    void OnEnable()
    {
        SnapToCenter();
    }
    void Start()
    {
        cursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        UpdateCursorPosition();
    }

    void Update()
    {
        if(Gamepad.current == null)
        {
            cursor.gameObject.SetActive(false);
            return;
        }
        else
        {
            cursor.gameObject.SetActive(true);
        }
        Vector2 stick = Gamepad.current.leftStick.ReadValue();

        if (stick.magnitude < 0.15f)
            stick = Vector2.zero;

        cursorPosition += stick * cursorSpeed * Time.unscaledDeltaTime;

        cursorPosition.x = Mathf.Clamp(
            cursorPosition.x,
            0,
            Screen.width
        );

        cursorPosition.y = Mathf.Clamp(
            cursorPosition.y,
            0,
            Screen.height
        );

        UpdateCursorPosition();

        if (Mouse.current != null)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            // If the mouse moves, switch back to mouse
            if (Mouse.current.delta.ReadValue().magnitude > 0.1f)
            {
                cursorPosition = mousePosition;

                UpdateCursorPosition();

                MouseHover.usingController = false;
            }
        }

        // Tell MouseHover where the controller cursor is
        MouseHover.controllerMousePosition = cursorPosition;
        MouseHover.usingController = true;

        // A / X
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Click();
        }
    }

    void UpdateCursorPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            cursorPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera,
            out Vector2 localPosition
        );

        cursor.localPosition = localPosition;
    }

    void Click()
{
    // Check UI buttons first
    PointerEventData pointerData =
        new PointerEventData(EventSystem.current);

    pointerData.position = cursorPosition;

    List<RaycastResult> results =
        new List<RaycastResult>();

    EventSystem.current.RaycastAll(pointerData, results);

    foreach (RaycastResult result in results)
    {
        Button button = result.gameObject.GetComponent<Button>();

        if (button != null && button.interactable)
        {
            button.onClick.Invoke();
            return;
        }
    }

    // If it wasn't a UI button, use your 3D menu system
    if(mainMenu != null)
        {
             mainMenu.HandleInput(cursorPosition);
        }
}
public void SnapToCenter()
{
    cursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    UpdateCursorPosition();

    MouseHover.controllerMousePosition = cursorPosition;
}
}