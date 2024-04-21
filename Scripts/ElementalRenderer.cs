using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalRenderer : MonoBehaviour
{
    public static readonly string[] moveDirections = { "Move T", "Move TL", "Move L", "Move BL", "Move B", "Move BR", "Move R", "Move TR"};
    public static readonly string[] attackDirections = { "Attack T", "Attack TL", "Attack L", "Attack BL", "Attack B", "Attack BR", "Attack R", "Attack TR" };

    public Animator animator;
    int lastDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public int LastDirection()
    {
        return lastDirection; 
    }

    public void SetDirection(Vector2 direction)
    {
        lastDirection = DirectionToIndex(direction, 8);
        animator.Play(moveDirections[lastDirection]);
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

    public void Attack()
    {
        animator.Play(attackDirections[lastDirection]);
    }
}
