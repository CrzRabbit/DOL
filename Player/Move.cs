using UnityEngine;
using UnityEngine.Networking;

public class Move : NetworkBehaviour
{
    private float moveSpeed = 20f;
    private float coefficient = 1;
    private float mouseSpeed = 120f;
    private bool isWary = false;
    private bool isHandUP = false;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //设置相机起始位置
            Camera.main.transform.position = transform.position + (new Vector3(0f, 1.0f, -1.4f));
            Camera.main.transform.rotation = transform.rotation;
            Camera.main.transform.Rotate(new Vector3(0, 0, 0));

            Camera.main.transform.parent = transform;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        //如果不是本地玩家，返回
        if (!isLocalPlayer)
        {
            return;
        }

        //Camera.main.transform.Rotate(spine.transform.rotation.eulerAngles);

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
           // CmdLook(Mathf.Floor(my * 100));
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
            coefficient = 1;
            Vector3 step = Vector3.forward * moveSpeed * coefficient * Time.deltaTime;
            step.x = Mathf.Floor(step.x * 100);
            step.y = Mathf.Floor(step.y * 100);
            step.z = Mathf.Floor(step.z * 100);
            CmdWalk(step);
        }

        //停止行走
        else if (Input.GetKeyUp(KeyCode.W))
        {
            //amtr.SetFloat(speedID, 0f);
            //CmdStopWalk();
        }

        //跳跃
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //amtr.SetTrigger(jumpID);
            //CmdJump();
        }
    }


    [Command]
    void CmdFire()
    {
        //amtr.SetBool(fireaID, true);
        //RpcFire();
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
        step = step / 100;
        transform.Translate(step);
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
}
