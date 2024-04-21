using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerCharacterAgent : Agent
{
    PlayerCharacterController controller;
    HealthDamageController healthController;

    public SurvivalSpawner spawner;

    public float detectionRadius = 10.0f;
    public LayerMask healthHitboxesLayer;
    public LayerMask attackHitboxesLayer;
    public int numberOfEnemiesToDetect = 5;


    Rigidbody2D rb;

    public void Awake()
    {
        controller = GetComponent<PlayerCharacterController>();
        healthController = GetComponent<HealthDamageController>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    public override void OnEpisodeBegin()
    {
        spawner.Reset();
        rb.position = new Vector3(5, 2, 0);
        healthController.health = 20f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Normalize(rb.position.x, -20f, 20f));
        sensor.AddObservation(Normalize(rb.position.y, -20f, 20f));
        sensor.AddObservation(controller.lastDirection);
        sensor.AddObservation(Normalize(healthController.health, 0f, 20f));

        int playerState;
        if (controller.canMove) playerState = 0;
        else if (controller.isDashing) playerState = 1;
        else if (controller.isCasting) playerState = 2;
        else playerState = 3; // if they are attacking

        sensor.AddOneHotObservation(playerState, 4);

        // attack hit boxes
        List<GameObject> attackHitboxes = FindNearestObservations(attackHitboxesLayer);
        for (int i = 0; i < numberOfEnemiesToDetect; i++)
        {
            if (i < attackHitboxes.Count)
            {
                GameObject attackHitbox = attackHitboxes[i];
                sensor.AddObservation(Normalize(attackHitbox.transform.position.x, -20f, 20f));
                sensor.AddObservation(Normalize(attackHitbox.transform.position.y, -20f, 20f));
                
                int hitboxType = -1;
                if (attackHitbox.CompareTag("Volley")) hitboxType = 3;
                else if (attackHitbox.transform.parent.gameObject.CompareTag("Enemy")) hitboxType = 1;
                else if (attackHitbox.transform.parent.gameObject.CompareTag("Slime")) hitboxType = 2;
                sensor.AddOneHotObservation(hitboxType, 4);
            }
            else
            {
                sensor.AddObservation(0.0f);
                sensor.AddObservation(0.0f);
                sensor.AddOneHotObservation(0, 4);
            }
        }

        // health hit boxes
        List<GameObject> healthHitboxes = FindNearestObservations(healthHitboxesLayer);
        for (int i = 0; i < numberOfEnemiesToDetect; i++)
        {
            if (i < healthHitboxes.Count)
            {
                GameObject healthHitbox = healthHitboxes[i];
                sensor.AddObservation(Normalize(healthHitbox.transform.position.x, -20f, 20f));
                sensor.AddObservation(Normalize(healthHitbox.transform.position.y, -20f, 20f)); 
                sensor.AddObservation(Normalize(healthHitbox.GetComponent<HealthHitbox>().healthDamageController.health, 0f, 20f));
                
                int hitboxType = -1;
                if (healthHitbox.transform.parent.gameObject.CompareTag("Enemy")) hitboxType = 1;
                else if (healthHitbox.transform.parent.gameObject.CompareTag("Slime")) hitboxType = 2;
                else if (healthHitbox.transform.parent.gameObject.CompareTag("Elemental")) hitboxType = 3;
                sensor.AddOneHotObservation(hitboxType, 4);
            }
            else
            {
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                sensor.AddObservation(-0.1f);
                sensor.AddOneHotObservation(0, 4);
            }
        }

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector2 moveDirection = new Vector2(actionBuffers.ContinuousActions[0], actionBuffers.ContinuousActions[1]);
        controller.MovePlayer(moveDirection.normalized);

        int action = actionBuffers.DiscreteActions[0];
        if (action == 1) controller.TriggerAttack();
        if (action == 2) controller.TriggerDash();
        if (action == 3) controller.TriggerCast();

        AddReward(0.0001f); // reward for just surviving
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");  

        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKeyDown(KeyCode.Z)) discreteActionsOut[0] = 1; // Attack
        if (Input.GetKeyDown(KeyCode.X)) discreteActionsOut[0] = 2; // Dash
        if (Input.GetKeyDown(KeyCode.C)) discreteActionsOut[0] = 3; // Cast
    }

    float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    List<GameObject> FindNearestObservations(LayerMask layer)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layer);
        List<GameObject> observations = new List<GameObject>();

        foreach (Collider2D hit in hits)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, (hit.transform.position - transform.position).normalized, detectionRadius, layer);
            if (raycastHit.collider != null && raycastHit.collider.gameObject == hit.gameObject)
            {
                if (hit.gameObject.CompareTag("Lightning") || (hit.gameObject.transform.parent != null && hit.gameObject.transform.parent.gameObject.CompareTag("Player")))
                    continue;  
                observations.Add(hit.gameObject);
            }
        }

        // Sort by distance and return only the closest ones
        return observations.OrderBy(h => Vector2.Distance(transform.position, h.transform.position))
                           .Take(numberOfEnemiesToDetect)
                           .ToList();
    }


}
