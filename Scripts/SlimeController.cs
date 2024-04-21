using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float movementSpeed = 1;
    Rigidbody2D rb;
    private SlimeRenderer isoRenderer;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float waitDuration = 4f;
    private bool isSpinning = false;
    public float spinSpeedMultiplier = 1.5f;
    private AttackHitboxController attackController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<SlimeRenderer>();
        attackController = GetComponentInChildren<AttackHitboxController>();
    }

    void Start()
    {
        
    }

    public void Spin(Vector2 direction)
    {
        isSpinning = true;
        isoRenderer.Spin(direction);
        StartCoroutine(WaitForAttackEnd());
    }

    public void OnAttackAnimationEnd()
    {
        attackController.EndAttack();
        isSpinning = false;
        isWaiting = true;
    }
    private IEnumerator WaitForAttackEnd()
    {
        yield return new WaitWhile(() => isoRenderer.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.05);
        int direction = isoRenderer.LastDirection();
        attackController.StartAttack(direction);

        yield return new WaitWhile(() => isoRenderer.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        
        yield return null; // wait for an extra frame to ensure the animation has truly finished

        OnAttackAnimationEnd();
    }

   public void Move(Vector2 direction)
    {
        if (isWaiting)
        {
            if (waitTimer >= waitDuration)
            {
                waitTimer = 0f;
                isWaiting = false;
            }
            else
            {
                waitTimer += Time.deltaTime;
                isoRenderer.SetDirection(direction * 0.0001f);
            }
        }
        else if (isSpinning)
        {
            Vector2 currentPos = rb.position;
            Vector2 inputVector = direction.normalized;
            Vector2 movement = inputVector * movementSpeed * spinSpeedMultiplier;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        else
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
