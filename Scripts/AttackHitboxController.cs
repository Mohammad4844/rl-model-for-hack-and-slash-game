using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    public float attackDamage = 1f;
    // supposed to be a capsule, NOTE: is a TRIGGER
    // NOTE: assigne manually
    public CapsuleCollider2D attackCollider; 

    // refernce for each direction's hitbox trigger sizes
    // number represent: [position x, position y, rotation on z axis, scale x, scale y]
    private float[][] directionalColliderMap;
    private float[][] playerDirectionalColliderMap = new float[][]
    {
        new float[] { 0.06f, 0.226f, 0f, 0.7f, 0.4f }, // T
        new float[] { -0.27f, 0.17f, 60f, 0.7f, 0.45f }, // TL
        new float[] { -0.39f, -0.05f, 90f, 0.7f, 0.6f }, // L
        new float[] { -0.29f, -0.19f, -60f, 0.7f, 0.45f }, // BL
        new float[] { -0.05f, -0.24f, 0f, 0.7f, 0.4f }, // B
        new float[] { 0.25f, -0.213f, 60f, 0.7f, 0.5f }, // BR
        new float[] { 0.39f, -0.05f, 90f, 0.7f, 0.6f }, // R
        new float[] { 0.32f, 0.125f, -60f, 0.7f, 0.5f } // TR 
    };

    private float[][] enemyDirectionalColliderMap = new float[][]
    {
        new float[] { 0f, 0.4f, 0f, 0.45f, 0.4f }, // T
        new float[] { -0.4f, 0.27f, 60f, 0.45f, 0.45f }, // TL
        new float[] { -0.5f, 0.1f, 90f, 0.45f, 0.5f }, // L
        new float[] { -0.29f, -0.01f, -60f, 0.45f, 0.45f }, // BL
        new float[] { 0.1f, -0.1f, 0f, 0.45f, 0.4f }, // B
        new float[] { 0.43f, -0.07f, 60f, 0.45f, 0.45f }, // BR
        new float[] { 0.5f, 0.1f, 90f, 0.45f, 0.5f }, // R
        new float[] { 0.37f, 0.26f, -60f, 0.45f, 0.45f } // TR 
    };

    private float[][] slimeDirectionalColliderMap = new float[][]
    {
        new float[] { 0f, -0.1f, 0f, 0.35f, 0.3f }, // all
        new float[] { 0f, -0.1f, 0f, 0.35f, 0.3f } // all
    };

    void Start()
    {
        if (gameObject.CompareTag("Player"))
            directionalColliderMap = playerDirectionalColliderMap;
        else if (gameObject.CompareTag("Slime"))
            directionalColliderMap = slimeDirectionalColliderMap;
        else if (gameObject.CompareTag("Enemy"))
            directionalColliderMap = enemyDirectionalColliderMap;
        attackCollider.enabled = false;
    }

    public void StartAttack(int direction)
    {
        float[] mapping = directionalColliderMap[direction];
        attackCollider.transform.localPosition = new Vector3(mapping[0], mapping[1], attackCollider.transform.localPosition.z);
        attackCollider.transform.localRotation = Quaternion.Euler(0, 0, mapping[2]);
        attackCollider.transform.localScale = new Vector3(mapping[3], mapping[4]);
        attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        attackCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHitbox healthHitbox = collision.gameObject.GetComponent<HealthHitbox>();
        if (healthHitbox != null && healthHitbox.owner != this.gameObject) // stops self-damage
        {
            healthHitbox.TakeDamage(attackDamage);
        }
    }
}
