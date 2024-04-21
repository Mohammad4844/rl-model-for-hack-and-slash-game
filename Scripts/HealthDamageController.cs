using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageController : MonoBehaviour
{
    public float health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        AgentRewardForDamage(damage);

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        AgentRewardForDying();
        if (AgentManager.Instance != null && gameObject.CompareTag("Player"))
            AgentManager.Instance.EndEpisode();
        else
            Destroy(gameObject);
    }

    void AgentRewardForDamage(float damage)
    {
        string tag = gameObject.tag;
        switch (tag)
        {
            case "Player":
                AgentManager.Instance.AddReward(-0.01f * damage);
                break;
            case "Enemy":
            case "Slime":
            case "Elemental":
                AgentManager.Instance.AddReward(0.025f * damage);
                break;
            default:
                Debug.Log("Something went wrong in AgentRewardForDamage()");
                break;
        }
    }

    void AgentRewardForDying()
    {
        string tag = gameObject.tag;
        switch (tag)
        {
            case "Player":
                AgentManager.Instance.SetReward(-1.0f);
                break;
            case "Enemy":
                AgentManager.Instance.AddReward(0.2f);
                break;
            case "Slime":
                AgentManager.Instance.AddReward(0.2f);
                break;
            case "Elemental":
                AgentManager.Instance.AddReward(0.2f);
                break;
            default:
                Debug.Log("Something went wrong in AgentRewardForDying()");
                break;
        }
    }
}
