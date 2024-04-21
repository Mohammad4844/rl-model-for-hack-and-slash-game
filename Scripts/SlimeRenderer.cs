using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRenderer : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    int lastDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int LastDirection()
    {
        return lastDirection; 
    }

    public void SetDirection(Vector2 direction)
    {

        string animation;
        if (direction.magnitude < 0.1f)
        {
            animation = "Idle";
        }
        else
        {
            animation = "Run";
        }

        lastDirection = DirectionToIndex(direction, 8) / 4; // 0 means top > bottom left, 1 means bottom > top right
        if (lastDirection == 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        animator.Play(animation);
    }

    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfstep = step / 2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfstep;
        if (angle < 0)
            angle += 360;
        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }     

    public void Spin(Vector2 direction)
    {
        SetDirection(direction);
        animator.Play("Spin");
    }

}
