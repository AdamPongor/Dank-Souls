using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    int vertical;
    int horizontal;
    private bool canRotate = true;

    public bool CanRotate { get => canRotate; set => canRotate = value; }

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");


    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        float v = verticalMovement switch
        {
            float i when i < -0.55 => -1,
            float i when i >= -0.55 && i < 0 => -0.5f,
            float i when i == 0 => 0,
            float i when i > 0 && i <= 0.55 => 0.5f,
            float i when i > 0.55 => 1,
            _ => 0
        };

        float h = horizontalMovement switch
        {
            float i when i < -0.55 => -1,
            float i when i >= -0.55 && i < 0 => -0.5f,
            float i when i == 0 => 0,
            float i when i > 0 && i <= 0.55 => 0.5f,
            float i when i > 0.55 => 1,
            _ => 0
        };

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

}
