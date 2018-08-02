using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public bool isRotating180;

    private int rotateDegrees = 0;
	
	// Update is called once per frame
	void Update () {
        if (isRotating180) {
            this.gameObject.transform.Rotate(Vector3.up * 3, Space.World);
            rotateDegrees += 3;
            if (rotateDegrees >= 180) {
                //Debug.Log("Falsifying the camera");
                isRotating180 = false;
                rotateDegrees = 0;
            }
        }
	}
}
