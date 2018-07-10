using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public int type;

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.GetComponent<Player>() != null && other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().PickUpItem(type);
            GetComponentInParent<PickupSpawn>().PickupWasPickedUp();
            Destroy(gameObject);
        }
    }

}
