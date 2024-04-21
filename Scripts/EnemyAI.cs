using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float chaseDistanceThreshold = 3; 
    [SerializeField]
    private float attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 3;
    [SerializeField]
    private float passedTime = 3;

    private EnemyController enemyController;
    

    private void Awake()
    {
        enemyController = GetComponentInChildren<EnemyController>();
    }

    private void Update()
    {

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            return;
        }

        Vector3 transformPosition = transform.position + new Vector3(0f, 0.3f, 0f); // counteracts the fact that the pivot is at the bottom
        float distance = Vector2.Distance(player.transform.position, transformPosition);
        if (distance < chaseDistanceThreshold)
        {
            Vector2 direction = player.transform.position - transformPosition;
            if (distance <= attackDistanceThreshold)
            {
                direction = direction * 0.001f; // make the value super small so it is interpreted as Idle, and at the same time allows you to follow player
                enemyController.Move(direction);
                if (passedTime >= attackDelay)
                {
                    passedTime = 0;
                    enemyController.Attack(direction);
                }
            }
            else
            {
                enemyController.Move(direction);
            }
        }
        else
        {
            enemyController.Move(new Vector2(0f, -0.0001f)); // face down
        }
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }

    private void Attack()
    {
        
    }

}
