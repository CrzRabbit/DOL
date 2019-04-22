using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class move : NetworkBehaviour {

    private Animator amtr;
    private Transform spine;

    private float moveSpeed = 2f;
    private float coefficient = 1;
    private float mouseSpeed = 120f;
    private bool isWary = false;
    private bool isHandUP = false;

    private int speedID = Animator.StringToHash("Speed");
    private int jumpID = Animator.StringToHash("Jump");
    private int waryID = Animator.StringToHash("Wary");
    private int handupID = Animator.StringToHash("HandUp");
    private int aimID = Animator.StringToHash("Aim");
    private int fireID = Animator.StringToHash("Fire");
    private int shootID = Animator.StringToHash("Shoot");

    private int idle = Animator.StringToHash("idle");
    private int walk = Animator.StringToHash("walk");
    private int run = Animator.StringToHash("run");

    void Start()
    {
        amtr = GetComponent<Animator>();
        spine = transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2);

        if (isLocalPlayer)
        {
            //设置相机起始位置
            Camera.main.transform.position = transform.position + (new Vector3(0f, 2.0f, -1.4f));
            Camera.main.transform.rotation = transform.rotation;
            Camera.main.transform.Rotate(new Vector3(0, 0, 0));

            Camera.main.transform.parent = transform;
            Cursor.lockState = CursorLockMode.Locked;
        }
        playermanager.putPlayers(gameObject);
    }

	void Update ()
    {
        //如果不是本地玩家，返回
        if (!isLocalPlayer)
        {
            return;
        }

        //Camera.main.transform.Rotate(spine.transform.rotation.eulerAngles);

        int state = amtr.GetCurrentAnimatorStateInfo(0).shortNameHash;

        //转身
        float mx = Input.GetAxisRaw("Mouse X");
        if (0 != mx)
        {
            //Vector3 rotation = Vector3.zero;
            //rotation.y += Mathf.Ceil(mx);
            ////transform.Rotate(rotation * Time.deltaTime * mounseSpeed);
            //CmdRotate(rotation * Time.deltaTime * mouseSpeed);

            Vector3 rotation = new Vector3(0, mx, 0);
            rotation = rotation * Time.deltaTime * mouseSpeed;
            rotation.y = Mathf.Floor(rotation.y * 100);
            CmdRotate(rotation);
            //transform.Rotate(rotation / 100);
        }

        float my = Input.GetAxisRaw("Mouse Y");
        //if (0 != my)
        {
            CmdLook(Mathf.Floor(my * 100));
        }

        //锁定鼠标
        if (Input.GetKey(KeyCode.V))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //解锁鼠标
        if (Input.GetKey(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //开火
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //amtr.SetBool(fireID, true);
            CmdFire();
        }

        //停止开火
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //amtr.SetBool(fireID, false);
            CmdStopFire();
        }

        //垂直移动
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.Translate(Vector3.up);
            CmdUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.Translate(Vector3.down);
            CmdDown();
        }

        //向前移动
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //amtr.SetFloat(speedID, 1f);
                coefficient = 3;
                if ((state == walk) || (state == run))
                {
                    //transform.Translate(Vector3.forward * moveSpeed * coefficient * Time.deltaTime);
                    Vector3 step = Vector3.forward * moveSpeed * coefficient * Time.deltaTime;
                    step.x = Mathf.Floor(step.x * 100);
                    step.y = Mathf.Floor(step.y * 100);
                    step.z = Mathf.Floor(step.z * 100);
                    CmdRun(step);
                    //amtr.SetFloat(speedID, 1f);
                    //step = step / 100;
                    //transform.Translate(step);
                }
                if (Input.GetKey(KeyCode.F))
                {
                    //transform.Translate(Vector3.forward * 10 * moveSpeed * coefficient * Time.deltaTime);
                    Vector3 step = Vector3.forward * 10 * moveSpeed * coefficient * Time.deltaTime;
                    step.x = Mathf.Floor(step.x * 100);
                    step.y = Mathf.Floor(step.y * 100);
                    step.z = Mathf.Floor(step.z * 100);
                    CmdRush(step);
                    //amtr.SetFloat(speedID, 1f);
                    //step = step / 100;
                    //transform.Translate(step);
                }
            }
            else
            {
                //amtr.SetFloat(speedID, 0.5f);
                coefficient = 1;
                if ((state == idle) || (state == walk))
                {
                    //transform.Translate(Vector3.forward * moveSpeed * coefficient * Time.deltaTime);
                    Vector3 step = Vector3.forward * moveSpeed * coefficient * Time.deltaTime;
                    step.x = Mathf.Floor(step.x * 100);
                    step.y = Mathf.Floor(step.y * 100);
                    step.z = Mathf.Floor(step.z * 100);
                    CmdWalk(step);
                    //amtr.SetFloat(speedID, 0.5f);
                    //step = step / 100;
                    //transform.Translate(step);
                }
            }
        }

        //停止行走
        else if (Input.GetKeyUp(KeyCode.W))
        {
            //amtr.SetFloat(speedID, 0f);
            CmdStopWalk();
        }

        //停止奔跑
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (amtr.GetFloat(speedID) == 1)
            {
                //amtr.SetFloat(speedID, 0.5f);
                CmdStopRun();
            }
        }

        //跳跃
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //amtr.SetTrigger(jumpID);
            CmdJump();
        }

        //警惕
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //amtr.SetBool(waryID, isWary);
            CmdWary(isWary);
            isWary = !isWary;
        }

        //举手
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //amtr.SetBool(handupID, !isHandUP);
            CmdHandup(!isHandUP);
            isHandUP = !isHandUP;
        }

        //瞄准开火
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            //amtr.SetBool(aimID, true);
            CmdAim();
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //amtr.SetBool(fireID, true);
                CmdFire();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //amtr.SetBool(fireID, false);
                CmdStopFire();
            }
        }

        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //amtr.SetBool(aimID, false);
            CmdStopAim();
        }
    }

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Animator animator;

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (arsenal.Length > 1)
            SetArsenal(arsenal[1].name);
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    transform.GetComponent<shoot>().Gun = newRightGun.transform;
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [Command]
    void CmdFire()
    {
        //amtr.SetBool(fireaID, true);
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {
        amtr.SetBool(fireID, true);
    }

    [Command]
    void CmdStopFire()
    {
        //amtr.SetBool(fireID, false);
        RpcStopFire();
    }

    [ClientRpc]
    void RpcStopFire()
    {
        amtr.SetBool(fireID, false);
    }

    [Command]
    void CmdUp()
    {
        //transform.Translate(Vector3.up);
        RpcUp();
    }

    [ClientRpc]
    void RpcUp()
    {
        transform.Translate(Vector3.up);
    }

    [Command]
    void CmdDown()
    {
        //transform.Translate(Vector3.down);
        RpcDown();
    }

    [ClientRpc]
    void RpcDown()
    {
        transform.Translate(Vector3.down);
    }

    [Command]
    void CmdWalk(Vector3 step)
    {
        //amtr.SetFloat(speedID, 0.5f);
        //transform.Translate(step);
        RpcWalk(step);
    }

    [ClientRpc]
    void RpcWalk(Vector3 step)
    {
        amtr.SetFloat(speedID, 0.5f);
        step = step / 100;
        transform.Translate(step);
    }

    [Command]
    void CmdRun(Vector3 step)
    {
        //amtr.SetFloat(speedID, 1f);
        //transform.Translate(step);
        RpcRun(step);
    }

    [ClientRpc]
    void RpcRun(Vector3 step)
    {
        amtr.SetFloat(speedID, 1f);
        step = step / 100;
        transform.Translate(step);
    }

    [Command]
    void CmdRush(Vector3 step)
    {
        //amtr.SetFloat(speedID, 1f);
        //transform.Translate(step);
        RpcRush(step);
    }

    [ClientRpc]
    void RpcRush(Vector3 step)
    {
        amtr.SetFloat(speedID, 1f);
        step = step / 100;
        transform.Translate(step);
    }

    [Command]
    void CmdStopWalk()
    {
        //amtr.SetFloat(speedID, 0f);
        RpcStopWalk();
    }

    [ClientRpc]
    void RpcStopWalk()
    {
        amtr.SetFloat(speedID, 0f);
    }

    [Command]
    void CmdStopRun()
    {
        //amtr.SetFloat(speedID, 0.5f);
        RpcStopRun();
    }

    [ClientRpc]
    void RpcStopRun()
    {
        amtr.SetFloat(speedID, 0.5f);
    }

    [Command]
    void CmdJump()
    {
        //amtr.SetTrigger(jumpID);
        RpcJump();
    }

    [ClientRpc]
    void RpcJump()
    {
        amtr.SetTrigger(jumpID);
    }

    [Command]
    void CmdWary(bool isWary)
    {
        //amtr.SetBool(waryID, isWary);
        RpcWary(isWary);
    }

    [ClientRpc]
    void RpcWary(bool isWary)
    {
        amtr.SetBool(waryID, isWary);
    }

    [Command]
    void CmdHandup(bool isHandup)
    {
        //amtr.SetBool(handupID, !isHandUP);
        RpcHandup(isHandup);
    }

    [ClientRpc]
    void RpcHandup(bool isHandup)
    {
        amtr.SetBool(handupID, !isHandup);
    }

    [Command]
    void CmdAim()
    {
        //amtr.SetBool(aimID, true);
        RpcAim();
    }

    [ClientRpc]
    void RpcAim()
    {
        amtr.SetBool(aimID, true);
    }

    [Command]
    void CmdStopAim()
    {
        //amtr.SetBool(aimID, false);
        RpcStopAim();
    }

    [ClientRpc]
    void RpcStopAim()
    {
        amtr.SetBool(aimID, false);
    }

    [Command]
    void CmdRotate(Vector3 rotation)
    {
        //transform.Rotate(rotation);
        RpcRotate(rotation);
    }

    [ClientRpc]
    void RpcRotate(Vector3 rotation)
    {
        rotation.y = rotation.y / 100;
        transform.Rotate(rotation);
    }

    [Command]
    void CmdLook(float my)
    {
        RpcLook(my);
    }

    [ClientRpc]
    void RpcLook(float my)
    {
        float temp = amtr.GetCurrentAnimatorStateInfo(1).normalizedTime;
        if (((temp < 1) && (temp > 0)) || ((0 >= temp) && (my >= 0)) || ((1 <= temp) && (my <= 0)))
        {
            amtr.SetFloat(shootID, my / 100);
        }
        else
        {
            amtr.SetFloat(shootID, 0);
        }
    }
}
