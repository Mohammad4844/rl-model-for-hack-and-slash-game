using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float chaseDistanceThreshold = 3; 
    [SerializeField]
    private float attackDistanceThreshold = 0.1f;

    [SerializeField]
    private float attackDelay = 10;
    [SerializeField]
    private float passedTime = 10;

    private SlimeController slimeController;
    

    private void Awake()
    {
        slimeController = GetComponentInChildren<SlimeController>();
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
            if (distance <= attackDistanceThreshold)
            {
                slimeController.Move(direction);
                if (passedTime >= attackDelay)
                {
                    passedTime = 0;
                    slimeController.Spin(direction);
                }
            }
            else
            {
                slimeController.Move(direction);
            }
        }
        else
        {
            slimeController.Move(new Vector2(0f, -0.0001f)); // face down
        }
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }
}
