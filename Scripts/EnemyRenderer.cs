using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderer : MonoBehaviour
{
    private int[][] directionMap = new int[][]
    {
        new int[] { 1, 0 }, // T
        new int[] { 1, -1 }, // TL
        new int[] { 0, -1 }, // L
        new int[] { -1, -1 }, // BL
        new int[] { -1, 0 }, // B
        new int[] { -1, 1 }, // BR
        new int[] { 0, 1 }, // R
        new int[] { 1, 1 } // TR
    };

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
        int[] mapping = directionMap[lastDirection];
        animator.SetFloat("Vertical", mapping[0]);
        animator.SetFloat("Horizontal", mapping[1]);

        string animation;

        if (direction.magnitude < 0.1f)
        {
            animation = "Idle";
        }
        else
        {
            animation = "Run";
        }

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

    public void Attack(Vector2 direction)
    {
        SetDirection(direction);
        animator.Play("Attack");
    }

}
