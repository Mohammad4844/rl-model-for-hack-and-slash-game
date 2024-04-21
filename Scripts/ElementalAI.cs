using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAI : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float chaseDistanceThreshold = 7; 
    [SerializeField]
    private float attackDistanceThreshold = 3f;
    [SerializeField]
    private float escapeDistanceThreshold = 1f;

    [SerializeField]
    private float attackDelay = 3;
    [SerializeField]
    private float passedTime = 3;

    private ElementalController elementalController;
    

    private void Awake()
    {
        elementalController = GetComponentInChildren<ElementalController>();
    }

    private void Update()
    {

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            return;
        }

        Vector3 transformPosition = transform.position;
        float distance = Vector2.Distance(player.transform.position, transformPosition);
        if (distance < chaseDistanceThreshold)
        {
            Vector2 direction = player.transform.position - transformPosition;
            if (distance <= escapeDistanceThreshold)
            {
                // TODO: Maybe like a blink ability???
            }
            else if (distance <= attackDistanceThreshold)
            {
                direction = direction * 0.001f; // make the value super small so it is interpreted as Idle, and at the same time allows you to follow player
                elementalController.Move(direction);
                if (passedTime >= attackDelay)
                {
                    passedTime = 0;
                    elementalController.Attack(direction);
                }
            }
            else
            {
                elementalController.Move(direction);
            }
        }
        else
        {
            elementalController.Move(new Vector2(0f, -0.0001f)); // face down
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
