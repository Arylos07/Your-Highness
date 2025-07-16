using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycaster : MonoBehaviour
{
    public GameObject DetectedObject;
    public float RayDistance = 100f;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, RayDistance))
            {
                DetectedObject = hit.collider.gameObject;
            }
            else
            {
                DetectedObject = null;
            }
        }
    }
}