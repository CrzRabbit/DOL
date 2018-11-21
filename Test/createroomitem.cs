using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createroomitem : MonoBehaviour {

    public RectTransform item;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 5; i++)
        {
            GameObject.Instantiate(item).SetParent(transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
