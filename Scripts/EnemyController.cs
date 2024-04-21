using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 1;
    Rigidbody2D rb;
    private EnemyRenderer isoRenderer;
    private bool canMove = true;
    private AttackHitboxController attackController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<EnemyRenderer>();
        attackController = GetComponentInChildren<AttackHitboxController>();
    }

    void Start()
    {
        
    }

    public void Attack(Vector2 direction)
    {
        canMove = false;
        isoRenderer.Attack(direction);
        StartCoroutine(WaitForAttackEnd());
    }

    public void OnAttackAnimationEnd()
    {
        attackController.EndAttack();
        canMove = true;
    }
    private IEnumerator WaitForAttackEnd()
    {
        yield return new WaitWhile(() => isoRenderer.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3);
        int direction = isoRenderer.LastDirection();
        attackController.StartAttack(direction);

        yield return new WaitWhile(() => isoRenderer.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        
        yield return null; //wait for an extra frame to ensure the animation has truly finished

        OnAttackAnimationEnd();
    }

   public void Move(Vector2 direction)
    {
        if (canMove)
        {
            Vector2 currentPos = rb.position;
            Vector2 inputVector = Vector2.ClampMagnitude(direction, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            isoRenderer.SetDirection(movement);
            rb.MovePosition(newPos);
        }
    }
}
