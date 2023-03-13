using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;

    public Transform PlayerTransform;
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    public float movementSpeed;
    public float rotationSpeed;

    public bool sprint;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        //myTransform = transform;
        animatorHandler.Initialize();
    }

    Vector3 normalVector;
    Vector3 targetPosition;

    public void Update()
    {
        float delta = Time.deltaTime;
        sprint = inputHandler.longB_input;
        inputHandler.TickInput(delta);

        HandleMovement(delta);
        HandleRolling(delta);

    }

    public void HandleMovement(float delta)
    {

        if (animatorHandler.anim.GetBool("isInteracting"))
        {
            return;
        }
        /*moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;*/

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, sprint);

        if (animatorHandler.CanRotate)
        {
            HandleRotation(delta);
        }
    }

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = PlayerTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(PlayerTransform.rotation, tr, rs * delta);

        PlayerTransform.rotation = targetRotation;

    }

    public void HandleRolling(float delta)
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
        {
            return;
        }

        if (inputHandler.roll)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayAnimation("Roll", true, 0.1f);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                PlayerTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayAnimation("BackStep", true, 0.05f);
            }
        }
        
    }
}
