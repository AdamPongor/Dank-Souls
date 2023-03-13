using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    public InputHandler inputHandler;
    public PlayerMovement playerMovement;
    int vertical;
    int horizontal;
    private bool canRotate = true;

    public bool CanRotate { get => canRotate; set => canRotate = value; }

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

    }

    public void PlayAnimation(string Animation, bool isInteracting, float transitiontime)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(Animation, transitiontime);
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool sprint)
    {
        float v = verticalMovement switch
        {
            float i when i < -0.55 => -1,
            float i when i >= -0.55 && i < -0.1 => -0.5f,
            float i when i >= -0.1 && i <= 0.1 => 0,
            float i when i > 0.1 && i <= 0.55 => 0.5f,
            float i when i > 0.55 => 1,
            _ => 0
        };

        float h = horizontalMovement switch
        {
            float i when i < -0.55 => -1,
            float i when i >= -0.55 && i < -0.1 => -0.5f,
            float i when i >= -0.1 && i <= 0.1 => 0,
            float i when i > 0.1 && i <= 0.55 => 0.5f,
            float i when i > 0.55 => 1,
            _ => 0
        };

        if (sprint && (h > 0 || v > 0))
        {
            v = 2;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        playerMovement.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerMovement.rigidbody.velocity = velocity;
    }

    public void ResetInteracting()
    {
        anim.SetBool("isInteracting", false);
    }
}
