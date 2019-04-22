using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class shoot : NetworkBehaviour {
    public GameObject Bullet;
    public GameObject Fire;
    public GameObject Ice;
    public GameObject i;
    public GameObject IceTime;
    public Transform Gun;
    public long bulletCount = 0;
    public float bulletspeed = 100;

    [SyncVar]
    public float temp = 0;

    private int fire = Animator.StringToHash("fire");
    private int aim = Animator.StringToHash("aim");
    private bool flag = true;

    public Animator amtr;
    void Start()
    {
        amtr = GetComponent<Animator>();
    }

    void Update()
    {
        //射击
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {   
        temp = amtr.GetCurrentAnimatorStateInfo(2).normalizedTime % 1;
        if (temp > 0.5f)
        {
            flag = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            //Vector3 dic = new Vector3(0, 2.0f, 0);
            //dic += transform.forward * 0.4f;
            //GameObject iceTime = GameObject.Instantiate(IceTime, transform.position + dic, transform.rotation);
            //Rigidbody rgd = iceTime.GetComponent<Rigidbody>();
            //rgd.AddForce(transform.forward * 300);
            //CmdIceTime();
        }
        else if ((amtr.GetCurrentAnimatorStateInfo(2).shortNameHash == fire) && (temp < 0.1f) && flag)
        {
            if (Input.GetButton("Fire1"))
            {
                ////创建子弹
                //GameObject bullet = GameObject.Instantiate(Bullet, Gun.GetChild(0).position, Gun.rotation);
                ////创建枪口火花
                //GameObject fire = GameObject.Instantiate(Fire, Gun.GetChild(0).position, Gun.rotation);
                //GameObject.Destroy(fire, 0.5f);
                ////子弹施加动力
                //Rigidbody rgd = bullet.GetComponent<Rigidbody>();
                //rgd.AddForce(transform.forward * bulletspeed * 100);
                ////播放音效
                //AudioSource audioSource = Gun.GetComponent<AudioSource>();
                //audioSource.Play();
                //GameObject.Destroy(bullet, 1.5f);
                flag = false;
                CmdFire();
            }
        }
        else if (amtr.GetCurrentAnimatorStateInfo(2).shortNameHash == aim)
        {
            if (Input.GetKey(KeyCode.F))
            {
                //if (i == null)
                //{
                //    i = GameObject.Instantiate(Ice, Gun.GetChild(0).position, Gun.rotation);
                //    i.transform.SetParent(Gun.GetChild(0));
                //}
                CmdIce();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                //GameObject.Destroy(i);
                CmdStopIce();
            }
        }
        yield return null;
    }

    [Command]
    void CmdFire()
    {
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {
        GameObject bullet = GameObject.Instantiate(Bullet, Gun.GetChild(0).position, Gun.rotation);
        GameObject.Destroy(bullet, 10f);

        GameObject fire = GameObject.Instantiate(Fire, Gun.GetChild(0).position, Gun.rotation);
        GameObject.Destroy(fire, 0.5f);

        Rigidbody rig = bullet.GetComponent<Rigidbody>();
        Vector3 br = transform.forward;
        Debug.Log(br);
        float temp = Mathf.Clamp(amtr.GetCurrentAnimatorStateInfo(1).normalizedTime, 0f, 1f);
        br.y += (-20 + temp * 60) / 60f;
        br.y = Mathf.Clamp(br.y, -0.5f, 0.7f);
        Debug.Log(br.y);
        rig.AddForce(br * bulletspeed);

        AudioSource audioSource = Gun.GetComponent<AudioSource>();
        audioSource.Play();

        NetworkServer.Spawn(bullet);
        NetworkServer.Spawn(fire);
    }

    [Command]
    void CmdIceTime()
    {
        RpcIceTime();
    }

    [ClientRpc]
    void RpcIceTime()
    {
        Vector3 dic = new Vector3(0, 2.0f, 0);
        dic += transform.forward * 0.4f;
        GameObject iceTime = GameObject.Instantiate(IceTime, transform.position + dic, transform.rotation);
        Rigidbody rgd = iceTime.GetComponent<Rigidbody>();
        rgd.AddForce(transform.forward * 300);
    }

    [Command]
    void CmdIce()
    {
        RpcIce();
    }

    [ClientRpc]
    void RpcIce()
    {
        if (i == null)
        {
            i = GameObject.Instantiate(Ice, Gun.GetChild(0).position, Gun.rotation);
            i.transform.SetParent(Gun.GetChild(0));
        }
    }

    [Command]
    void CmdStopIce()
    {
        RpcStopIce();
    }

    [ClientRpc]
    void RpcStopIce()
    {
        GameObject.Destroy(i);
    }
}
