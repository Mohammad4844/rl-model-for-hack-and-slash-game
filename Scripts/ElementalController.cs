using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalController : MonoBehaviour
{
    public float movementSpeed = 1;
    Rigidbody2D rb;
    private ElementalRenderer isoRenderer;
    private bool canMove = true;

    public GameObject volleyPrefab;
    // represents transform [x, y]
    private float[][] volleyDirectionalOffsetMap = new float[][]
    {
        new float[] { 0f, 0.3f}, // T
        new float[] { -0.2f, 0.3f }, // TL
        new float[] { -0.2f, 0.1f }, // L
        new float[] { -0.2f, -0.2f }, // BL
        new float[] { 0f, 0.2f }, // B
        new float[] { 0.2f, -0.2f }, // BR
        new float[] { 0.2f, 0.2f }, // R
        new float[] { 0.2f, 0.3f } // TR 
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<ElementalRenderer>();
    }

    void Start()
    {
        
    }

    public void Attack(Vector2 direction)
    {
        canMove = false;
        isoRenderer.Attack();
        StartCoroutine(WaitForAttackEnd());
    }

    public void OnAttackAnimationEnd()
    {
        canMove = true;
    }

    private IEnumerator WaitForAttackEnd()
    {
        yield return new WaitWhile(() => isoRenderer.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7);

        int direction = isoRenderer.LastDirection();
        float[] offsetMapping = volleyDirectionalOffsetMap[direction];
        Vector3 offset = new Vector3(offsetMapping[0], offsetMapping[1], 0f);
        Instantiate(volleyPrefab, transform.position + offset, transform.rotation);

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
