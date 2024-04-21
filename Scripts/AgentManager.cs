using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance { get; private set; }
    public PlayerCharacterAgent PlayerAgent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            AssignPlayer();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void AssignPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerAgent = player.GetComponent<PlayerCharacterAgent>();
        }
    }

    public void AddReward(float reward)
    {
        if (PlayerAgent != null)
        {
            PlayerAgent.AddReward(reward);
        }
    }

    public void SetReward(float reward)
    {
        if (Instance != null && Instance.PlayerAgent != null)
        {
            PlayerAgent.SetReward(reward);
        }
    }

    public void EndEpisode()
    {
        if (Instance != null && Instance.PlayerAgent != null)
        {
            PlayerAgent.EndEpisode();
        }
    }
}
