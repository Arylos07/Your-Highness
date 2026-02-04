using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using QFSW.QC;

public class CameraMovement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private CameraNode startingNode;
    [SerializeField] private float moveSpeed = 5f;

    private CameraNode currentNode;
    [ReadOnly(true)] public string nodeName;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private InputSystem_Actions inputActions;
    public bool IsMoving
    {
        get
        {
            const float positionThreshold = 0.01f;
            const float rotationThreshold = 0.01f;

            bool positionMoving = Vector3.Distance(transform.position, targetPosition) > positionThreshold;
            bool rotationMoving = Quaternion.Angle(transform.rotation, targetRotation) > rotationThreshold;

            return positionMoving || rotationMoving;
        }
    }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        currentNode = startingNode;
        targetPosition = currentNode.cameraPosition;
        targetRotation = currentNode.cameraRotation;
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        // Smoothly move camera to target position and rotation
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (QuantumConsole.Instance.IsActive) return;

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x < -0.5f && currentNode.Left != null)
            MoveToNode(currentNode.Left);
        else if (input.x > 0.5f && currentNode.Right != null)
            MoveToNode(currentNode.Right);
        else if (input.y > 0.5f && currentNode.Up != null)
            MoveToNode(currentNode.Up);
        else if (input.y < -0.5f && currentNode.Down != null)
            MoveToNode(currentNode.Down);
    }

    private void MoveToNode(CameraNode node)
    {
        currentNode = node;
        targetPosition = node.cameraPosition;
        targetRotation = node.cameraRotation;
        nodeName = node.name; // Update the node name for display or debugging
        UIManager.Instance.SetUIState(node.uiState);

#if UNITY_EDITOR
        //Selection.objects = new Object[] { node.gameObject };
#endif
    }

    // Unused interface methods
    public void OnLook(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
}
