using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {
    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Enemy"))
            Destroy(other.gameObject);
        else
            other.transform.SetPositionAndRotation(new Vector3(
                other.gameObject.transform.position.x,
                other.gameObject.transform.position.y,
                15
                ),
                Quaternion.Euler(0, 180, 0)
            );
    }
}
