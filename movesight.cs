using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movesight : MonoBehaviour {

    private float mounseSpeed = 60f;

    void Start () {
 
	}
	
	void Update () {
        float my = Input.GetAxisRaw("Mouse Y");
        if (0 != my)
        {
            Vector3 rotation = Vector3.zero;
            rotation.x -= my;
            transform.Rotate(rotation * Time.deltaTime * mounseSpeed);
        }
    }
}
