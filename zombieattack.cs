using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class zombieattack : NetworkBehaviour{

    public Rigidbody aimRgd;
    public float moveSpeed = 2;

    private Animator amtr;
    private int instancdID = Animator.StringToHash("instance");
    private int attackID = Animator.StringToHash("attack");
    private int deathID = Animator.StringToHash("death");
    private int walkID = Animator.StringToHash("walk");

    void Start()
    {
        amtr = GetComponent<Animator>();
        Queue<GameObject> players = playermanager.PLAYERS;
        GetAim();
    }

    [Command]
    void CmdAttack(Quaternion rotation, Vector3 position)
    {
        RpcAttack(rotation, position);
    }

    [ClientRpc]
    void RpcAttack(Quaternion rotation, Vector3 position)
    {
        transform.GetComponent<Rigidbody>().MoveRotation(rotation);
        transform.GetComponent<Rigidbody>().MovePosition(position / 100);
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
            return;
        }
        float instance = (players[0].GetComponent<Rigidbody>().position - transform.position).sqrMagnitude;
        foreach (GameObject player in players)
        {
            float temp = (player.GetComponent<Rigidbody>().position - transform.position).sqrMagnitude;
            if (instance >= temp)
            {
                instance = temp;
                aimRgd = player.GetComponent<Rigidbody>();
            }
        }
    }

    void Update()
    {
        GetAim();

        if (!aimRgd)
        {
            return;
        }

        Vector3 dir = aimRgd.position - transform.position;
        float instance = dir.sqrMagnitude;
        if (amtr.GetCurrentAnimatorStateInfo(0).shortNameHash != deathID)
        {
            if (instance < 50000)
            {
                amtr.SetBool(attackID, false);
                if (instance < 0.5f)
                {
                    amtr.SetBool(attackID, true);
                }
                else
                {
                    amtr.SetInteger(instancdID, 1);
                    dir.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(dir);
                    //transform.GetComponent<Rigidbody>().MoveRotation(rotation);
                    //transform.GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed);
                    Vector3 position = transform.position + transform.forward * Time.deltaTime * moveSpeed;
                    position.x = Mathf.Floor(position.x * 100);
                    position.y = Mathf.Floor(position.y * 100);
                    position.z = Mathf.Floor(position.z * 100);
                    CmdAttack(rotation, position);
                }
            }
            else
            {
                amtr.SetInteger(instancdID, 0);
            }
        }

        if (amtr.GetCurrentAnimatorStateInfo(0).shortNameHash == walkID)
        {
            amtr.speed = moveSpeed / 2;
        }
        else
        {
            amtr.speed = 1;
        }
    }
}
