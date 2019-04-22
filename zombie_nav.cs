using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class zombie_nav : NetworkBehaviour {

    private Transform aimTran;
    private NavMeshAgent agent;
    private Animator amtr;
    private int attackID = Animator.StringToHash("attack");
    private int deathID = Animator.StringToHash("death");
    private int walkID = Animator.StringToHash("walk");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        amtr = GetComponent<Animator>();
        if(!isServer)
        {
            agent.gameObject.SetActive(false);
            return;
        }
        GetAim();
        agent.destination = aimTran.position;
    }

    void Update () {
        if(!isServer)
        {
            return;
        }
        GetAim();
        if (agent.enabled)
        {
            agent.destination = aimTran.position;
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                CmdAttack(false);
            }
            else
            {
                CmdAttack(true);
            }
            CmdWalk(transform.rotation, transform.position);
        }
	}

    void GetAim()
    {
        GameObject[] players;
        if (playermanager.PLAYERS.Count > 0)
        {
            players = playermanager.PLAYERS.ToArray();
        }
        else
        {
            aimTran = transform;
            return;
        }
        float instance = (players[0].GetComponent<Rigidbody>().position - transform.position).sqrMagnitude;
        foreach (GameObject player in players)
        {
            float temp = (player.GetComponent<Rigidbody>().position - transform.position).sqrMagnitude;
            if (instance >= temp)
            {
                instance = temp;
                aimTran = player.GetComponent<Transform>();
            }
        }
        if (null == aimTran)
        {
            aimTran = transform;
        }
    }

    [Command]
    void CmdWalk(Quaternion rotation, Vector3 position)
    {
        RpcWalk(rotation, position);
    }

    [ClientRpc]
    void RpcWalk(Quaternion rotation, Vector3 position)
    {
        if (!isServer)
        {
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    [Command]
    void CmdAttack(bool attack)
    {
        RpcAttack(attack);
    }

    [ClientRpc]
    void RpcAttack(bool attack)
    {
        if (amtr != null)
        {
            amtr.SetBool(attackID, attack);
        }
    }

    [Command]
    void CmdDeath()
    {

    }

    [ClientRpc]
    void RpcDeath()
    {

    }
}
