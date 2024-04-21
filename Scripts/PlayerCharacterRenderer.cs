using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterRenderer : MonoBehaviour
{
    public static readonly string[] staticDirections = { "Idle T", "Idle TL", "Idle L", "Idle BL", "Idle B", "Idle BR", "Idle R", "Idle TR"};
    public static readonly string[] runDirections = { "Run T", "Run TL", "Run L", "Run BL", "Run B", "Run BR", "Run R", "Run TR" };
    public static readonly string[] attackDirections = { "Attack T", "Attack TL", "Attack L", "Attack BL", "Attack B", "Attack BR", "Attack R", "Attack TR" };
    public static readonly string[] dashDirections = { "Dash T", "Dash TL", "Dash L", "Dash BL", "Dash B", "Dash BR", "Dash R", "Dash TR" };

    Animator animator;
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
        string[] directionArray = null;

        if (direction.magnitude < 0.1f)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(direction, 8);
        }

        animator.Play(directionArray[lastDirection]);
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

    public void Dash()
    {
        animator.Play(dashDirections[lastDirection]);
    }

    public void Cast()
    {
        animator.Play(attackDirections[lastDirection], 0, 0.31f); // the percentage point for the clip to play
        animator.speed = 0; // stop the animation from playing further
    }

    public void EndCast()
    {
        animator.speed = 1; // need to resume the animation
    }
}
