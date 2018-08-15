using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public bool isRotating180;
    public bool isMovingToTarget;
    public Vector3 targetLocation;

    private int rotateDegrees = 0;
	
	// Update is called once per frame
	void Update () {
        if (isRotating180) {
            this.gameObject.transform.Rotate(Vector3.up * 3, Space.World);
            rotateDegrees += 3;
            if (rotateDegrees >= 180) {
                isRotating180 = false;
                rotateDegrees = 0;
            }
        }

        if (isMovingToTarget) {
            //this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetLocation, Time.deltaTime);
            //Debug.Log("Moving towards: " + Vector3.Lerp(this.gameObject.transform.position, targetLocation, Time.deltaTime));
            if (this.gameObject.transform.position == targetLocation)
                isMovingToTarget = false;
        }

	}

    public void setAdjustedTargetLocation(Vector3 inTargetLocation) {
        Vector3 t = inTargetLocation;
        t.y += 4;
        t.z += 2;
        targetLocation = t;
        this.gameObject.transform.position = t; //temp
        isMovingToTarget = true;
    }
}
