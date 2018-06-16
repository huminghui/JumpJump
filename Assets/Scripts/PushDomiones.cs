using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDomiones : MonoBehaviour {

    public GameObject dominoeStart;
    public float force = 4.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            dominoeStart.GetComponent<Rigidbody>().AddForce(force,0,0,ForceMode.Impulse);
        }
	}
}
