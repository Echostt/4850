using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 20.0f);
	}
	
}
