using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class shoot1 : NetworkBehaviour {

    public GameObject bullet;
    private bool ready = true;
    private Transform muzzle;
	void Start () {
        muzzle = transform.Find("Muzzle").transform;
    }
	
	
	void Update () {
        if(!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButton("Fire1") && ready)
        {

            ready = false;
            CmdFire();
            ready = false;
            StartCoroutine(Ready());
        }
	}

    IEnumerator Ready()
    {
        yield return new WaitForSeconds(0.1f);
        ready = true;
    }

    [Command]
    void CmdFire()
    {
        GameObject obj = GameObject.Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
        obj.GetComponent<bullet>().DIRECTION = transform.forward;
        if (NetworkServer.active)
        {
            NetworkServer.Spawn(obj);
        }
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {

    }
}
