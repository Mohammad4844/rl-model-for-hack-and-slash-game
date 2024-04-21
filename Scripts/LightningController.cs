using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightningController : MonoBehaviour
{
    public float attackDamage = 5f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
        GameObject[] elementals = GameObject.FindGameObjectsWithTag("Elemental");

        GameObject[] targets = enemies.Concat(slimes).Concat(elementals).ToArray();

        if (targets.Length > 0)
        {
            GameObject randomTarget = targets[Random.Range(0, targets.Length)];
            transform.position = randomTarget.transform.position + new Vector3(0f, 0.3f, 0f);
            ExecuteCast();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void ExecuteCast()
    {
        animator.Play("Strike");
        StartCoroutine(WaitForCastEnd());
    }

    public void OnCastAnimationEnd()
    {
        Destroy(gameObject);
    }
    private IEnumerator WaitForCastEnd()
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        
        yield return null; //wait for an extra frame to ensure the animation has truly finished

        OnCastAnimationEnd();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHitbox healthHitbox = collision.gameObject.GetComponent<HealthHitbox>();
        if (healthHitbox != null) // stops self-damage
        {
            healthHitbox.TakeDamage(attackDamage);
        }
    }
}
