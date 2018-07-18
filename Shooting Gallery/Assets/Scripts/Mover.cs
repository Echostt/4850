using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    Vector3 direction;
    private void Start () {
        direction = Vector3.up;
    }

    void Update () {
        this.gameObject.transform.Translate(direction * 0.1f);
        if (this.gameObject.transform.position.y >= 8)
            direction = Vector3.down;
        else if (this.gameObject.transform.position.y <= 2) {
            direction = Vector3.up;
        }
	}
}
