using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class zombiehit : NetworkBehaviour {

    [SyncVar]
    private int health = 100;

    private Animator amtr;

    private int deathID = Animator.StringToHash("death");

	void Start () {
        amtr = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    [Command]
    void CmdDeath()
    {
        RpcDeath();
    }

    [ClientRpc]
    void RpcDeath()
    {
        amtr.SetTrigger(deathID);
        //gameObject.GetComponent<zombieattack>().enabled = false;
        gameObject.GetComponent<zombiehit>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        GameObject.Destroy(gameObject, 5.0f);
    }

    [Command]
    void CmdFrozen()
    {
        RpcFrozen();
    }

    [ClientRpc]
    void RpcFrozen()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        //gameObject.GetComponent<zombieattack>().enabled = false;
        gameObject.GetComponent<zombiehit>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        GameObject.Destroy(gameObject, 10.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name == "Bullet(Clone)") && (health > 0))
        {
            health -= 100;
            if (health <= 0)
            {
                //amtr.SetTrigger(deathID);
                //gameObject.GetComponent<zombieattack>().enabled = false;
                //gameObject.GetComponent<zombiehit>().enabled = false;
                CmdDeath();
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (((other.name == "Ice_Snow(Clone)") || (other.name == "Ice(Clone)") || (other.name == "Ice")) && (health > 0))
        {
            health -= 10;
            if (health <= 0)
            {
                Frozen();
            }
        }
    }

    private void Frozen()
    {
        //gameObject.GetComponent<Animator>().enabled = false;
        //gameObject.GetComponent<zombieattack>().enabled = false;
        //gameObject.GetComponent<zombiehit>().enabled = false;
        CmdFrozen();
    }
}
