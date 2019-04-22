using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour {
    private const float RANGE = 200f;
    private float DISTANCE = 0;
    private const float SPEED = 50f;
    public bool isShooted = false;

    private Vector3 direction;
    public Vector3 DIRECTION
    {
        get { return direction; }
        set { direction = value; }
    }

    private bool shooted = false;

    public bool SHOOTED
    {
        get { return shooted; }
        set { shooted = value; }
    }

    void Start()
    {
        SHOOTED = true;
    }

    void Update()
    {
        if (SHOOTED && DISTANCE < RANGE)
        {

            Quaternion rotation = new Quaternion(0,0,0,0);
            float temp = SPEED * Time.deltaTime;
            DISTANCE += temp;
            if (DISTANCE > RANGE)
            {
                temp -= DISTANCE - RANGE;
            }
            Vector3 position = transform.position + temp * DIRECTION;
            position.x = Mathf.Floor(position.x * 100);
            position.y = Mathf.Floor(position.y * 100);
            position.z = Mathf.Floor(position.z * 100);
            //CmdAttack(rotation, position, temp);
            transform.Translate(Vector3.forward * temp);
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    [Command]
    void CmdAttack(Quaternion rotation, Vector3 position, float step)
    {
        RpcAttack(rotation, position, step);
    }

    [ClientRpc]
    void RpcAttack(Quaternion rotation, Vector3 position, float step)
    {
        
    }
}
