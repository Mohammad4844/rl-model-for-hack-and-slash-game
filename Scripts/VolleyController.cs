using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyController : MonoBehaviour
{
    public float attackDamage = 1f;
    public float speed = 5f;
    public float turnSpeed = 20f;
    public float volleyTotalLife = 20f;
    private float volleyLife;

    private Rigidbody2D rb;
    
    GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        volleyLife = 0f;
    }

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            Vector2 currentDirection = rb.velocity.normalized;
            Vector2 newDirection = Vector2.Lerp(currentDirection, directionToPlayer, turnSpeed * Time.fixedDeltaTime).normalized;

            rb.velocity = newDirection * speed;
        }

        volleyLife += Time.deltaTime;
        if (volleyLife >= volleyTotalLife)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHitbox healthHitbox = collision.gameObject.GetComponent<HealthHitbox>();
        if (healthHitbox != null && healthHitbox.healthDamageController.gameObject.CompareTag("Player")) // stops self-damage
        {
            healthHitbox.TakeDamage(attackDamage);
        }
    }
}
