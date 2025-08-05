using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraNode : MonoBehaviour
{
    public UIState uiState;
    public CameraNode Up;
    public CameraNode Down;
    public CameraNode Left;
    public CameraNode Right;

    [Header("Camera View Data")]
    public float cameraDistance = 10f;
    [Tooltip("Angle in degrees around the Y axis (0 = forward, 90 = right, 180 = back, 270 = left)")]
    public float desiredAngle = 45f;
    [Tooltip("Height offset above the target position.")]
    public float cameraHeight = 5f;

    public Vector3 cameraPosition;
    public Quaternion cameraRotation;

    [Header("Target (must be a separate object)")]
    public Transform targetTransform;

    /// <summary>
    /// (Commented out) Calculates and stores the camera view data using the specified angle, distance, and height in world space.
    /// </summary>
    public void SetCameraView(Transform target, float distance = 10f, float angle = 45f, float height = 5f)
    {
        if (target == null) return;

        // Commented out: Calculation logic
        /*
        Vector3 offsetDir = target.rotation * Quaternion.Euler(0, angle, 0) * Vector3.forward;
        cameraPosition = target.position + offsetDir * distance + Vector3.up * height;
        cameraRotation = Quaternion.LookRotation((target.position + Vector3.up * height) - cameraPosition, Vector3.up);
        */
    }

    private void OnValidate()
    {
        if (targetTransform != null)
        {
            cameraPosition = targetTransform.transform.position;
            cameraRotation = targetTransform.transform.rotation;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (targetTransform == null) return;

        // Draw a line from this node to the target
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetTransform.position);

        // Draw a sphere at the node and at the target
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetTransform.position, 0.2f);

        // Draw the forward direction of the node
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);

        // Draw the angle arc between node forward and direction to target
        Vector3 toTarget = (targetTransform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(transform.forward, toTarget, Vector3.up);

        Handles.color = new Color(1f, 0.5f, 0f, 0.5f);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleToTarget, 1.0f);

        // Draw a label showing the angle
        Handles.color = Color.white;
        Handles.Label(transform.position + Vector3.up * 0.5f, $"Angle: {angleToTarget:F1}°");

        // Draw connection arrows for each direction
        DrawConnectionGizmo(Up, Color.red, "Up");
        DrawConnectionGizmo(Down, Color.blue, "Down");
        DrawConnectionGizmo(Left, Color.yellow, "Left");
        DrawConnectionGizmo(Right, Color.magenta, "Right");
    }

    private void DrawConnectionGizmo(CameraNode node, Color color, string label)
    {
        if (node == null) return;
        Vector3 start = transform.position + Vector3.up * 0.25f;
        Vector3 end = node.transform.position + Vector3.up * 0.25f;
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);

        // Draw arrowhead
        Vector3 direction = (end - start).normalized;
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 150, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -150, 0) * Vector3.forward;
        float arrowHeadLength = 0.3f;
        Gizmos.DrawRay(end, right * arrowHeadLength);
        Gizmos.DrawRay(end, left * arrowHeadLength);

        // Draw label at midpoint
        #if UNITY_EDITOR
        Handles.color = color;
        Handles.Label((start + end) * 0.5f + Vector3.up * 0.1f, label);
        #endif
    }
#endif
}