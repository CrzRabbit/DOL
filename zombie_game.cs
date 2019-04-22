using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class zombie_game : NetworkBehaviour {

    [SyncVar]
    private int mHealth = 100;
    private int deathID = Animator.StringToHash("death");
    private Animator cAmtr;

    void Start () {
        cAmtr = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollision hit by " + collision.gameObject.name);
        if ((collision.gameObject.name == "Bullet(Clone)") && (mHealth > 0))
        {
            mHealth -= 20;
            if (mHealth <= 0)
            {
                CmdDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger hit by " + other.gameObject.name);
        if ((other.gameObject.name == "Bullet(Clone)") && (mHealth > 0))
        {
            mHealth -= 20;
            if (mHealth <= 0)
            {
                CmdDeath();
            }
        }
    }

    [Command]
    void CmdDeath()
    {
        gameObject.GetComponent<zombie_nav>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        RpcDeath();
    }

    [ClientRpc]
    void RpcDeath()
    {
        cAmtr.SetTrigger(deathID);
        GameObject.Destroy(gameObject, 5.0f);
    }
}
