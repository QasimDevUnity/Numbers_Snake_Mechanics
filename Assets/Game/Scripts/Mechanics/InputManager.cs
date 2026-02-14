using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastPointerX;
    private bool isDragging;

    public float GetHorizontal()
    {
        float keyboardInput = Input.GetAxis("Horizontal");
        float swipeInput = 0f;

        // Mouse works for both Editor + Mobile
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastPointerX = Input.mousePosition.x;
        } 

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float delta = Input.mousePosition.x - lastPointerX;
            lastPointerX = Input.mousePosition.x;

            swipeInput = delta / Screen.width; // resolution independent
        }

        return keyboardInput + swipeInput;
    }
}