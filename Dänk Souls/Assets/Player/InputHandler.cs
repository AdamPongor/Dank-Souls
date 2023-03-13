using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_input;
    public bool roll;
    public bool isInteracting;
    public bool longB_input;

    PlayerInput inputActions;
    CameraHandler cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    public void Awake()
    {
        cameraHandler = CameraHandler.instance;
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleRotation(delta, mouseX, mouseY);
        }
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInput();
            inputActions.Movement.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Movement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.Actions.Roll.performed += i => b_input = i.performed;
            inputActions.Actions.Sprint.started += i => longB_input = true;
            inputActions.Actions.Sprint.canceled += i => longB_input = false;
        }

        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        RollInput(delta);
        SprintInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void RollInput(float delta)
    {
        Debug.Log(b_input.ToString());
        if (b_input)
        {
            roll = true;
            b_input = false;
        }
    }

    private void SprintInput(float delta)
    {

        if (longB_input)
        {
            Debug.Log(longB_input.ToString());
            //longB_input = false;
        }
    }
}
