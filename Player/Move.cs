using UnityEngine;
using UnityEngine.Networking;

public class Move : NetworkBehaviour
{
    private float moveSpeed = 20f;
    private float coefficient = 1;
    private float mouseSpeed = 120f;
    private bool isWary = false;
    private bool isHandUP = false;
    private Transform camera = null;
    public float smoothTime = 3F;
    private float yVelocity = 0.0F;
    private Transform muzzle;

    private void Start()
    {
        muzzle = transform.Find("Muzzle");
        if (isLocalPlayer)
        {
            //设置相机起始位置
            camera = Camera.main.transform;
            Camera.main.transform.parent = transform;
            Camera.main.transform.position = transform.position + transform.forward * -1 + new Vector3(0, 2, 0);
            Camera.main.transform.rotation = transform.rotation;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        //如果不是本地玩家，返回
        if (!isLocalPlayer)
        {
            return;
        }

        //转身
        float mx = Input.GetAxisRaw("Mouse X");
        if (0 != mx)
        {
            Vector3 rotation = new Vector3(0, mx, 0);
            rotation = rotation * Time.deltaTime * mouseSpeed;
            rotation.y = Mathf.Floor(rotation.y * 100);
            transform.Rotate(rotation / 100);
            CmdRotate(rotation);
        }

        float my = Input.GetAxisRaw("Mouse Y");
        {
            Vector3 rotation = -1 * my * mouseSpeed * new Vector3(1, 0, 0) * Time.deltaTime;
            rotation = rotation * 100;
            camera.Rotate(rotation / 100);
            CmdGunRotate(rotation);
        }


        //向前移动
        if (Input.GetKey(KeyCode.W))
        {
            coefficient = 1;
            Vector3 step = Vector3.forward * moveSpeed * coefficient * Time.deltaTime;
            step.x = Mathf.Floor(step.x * 100);
            step.y = Mathf.Floor(step.y * 100);
            step.z = Mathf.Floor(step.z * 100);
            transform.Translate(step / 100);
            CmdWalk(step);
        }

        if (Input.GetKey(KeyCode.S))
        {
            coefficient = 1;
            Vector3 step = Vector3.back * moveSpeed * coefficient * Time.deltaTime;
            step.x = Mathf.Floor(step.x * 100);
            step.y = Mathf.Floor(step.y * 100);
            step.z = Mathf.Floor(step.z * 100);
            transform.Translate(step / 100);
            CmdWalk(step);
        }

        if (Input.GetKey(KeyCode.A))
        {
            coefficient = 1;
            Vector3 step = Vector3.left * moveSpeed * coefficient * Time.deltaTime;
            step.x = Mathf.Floor(step.x * 100);
            step.y = Mathf.Floor(step.y * 100);
            step.z = Mathf.Floor(step.z * 100);
            transform.Translate(step / 100);
            CmdWalk(step);
        }

        if (Input.GetKey(KeyCode.D))
        {
            coefficient = 1;
            Vector3 step = Vector3.right * moveSpeed * coefficient * Time.deltaTime;
            step.x = Mathf.Floor(step.x * 100);
            step.y = Mathf.Floor(step.y * 100);
            step.z = Mathf.Floor(step.z * 100);
            transform.Translate(step / 100);
            CmdWalk(step);
        }

        //停止行走
        else if (Input.GetKeyUp(KeyCode.W))
        {

        }

        //跳跃
        else if (Input.GetKeyDown(KeyCode.Space))
        {

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
            CmdUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            CmdDown();
        }
    }

    [Command]
    void CmdFire()
    {

    }

    [Command]
    void CmdUp()
    {
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
        RpcWalk(step);
    }

    [ClientRpc]
    void RpcWalk(Vector3 step)
    {
        if (!isLocalPlayer)
        {
            step = step / 100;
            transform.Translate(step);
        }
    }

    [Command]
    void CmdRotate(Vector3 rotation)
    {
        RpcRotate(rotation);
    }

    [ClientRpc]
    void RpcRotate(Vector3 rotation)
    {
        if (!isLocalPlayer)
        {
            rotation.y = rotation.y / 100;
            transform.Rotate(rotation);
        }
    }

    [Command]
    void CmdGunRotate(Vector3 rotation)
    {
        RpcGunRotate(rotation);
    }

    [ClientRpc]
    void RpcGunRotate(Vector3 rotation)
    {
        if(muzzle != null)
        {
            muzzle.Rotate(rotation / 100);
        }
    }
}
