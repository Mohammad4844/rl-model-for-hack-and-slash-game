using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHitbox : MonoBehaviour
{
    public HealthDamageController healthDamageController;
    public GameObject owner;

    void Start()
    {
    }

    void Awake()
    {
        healthDamageController = GetComponentInParent<HealthDamageController>();
        owner = healthDamageController.gameObject;
    }

    public void TakeDamage(float damage)
    {
        healthDamageController.TakeDamage(damage);
    }
}
