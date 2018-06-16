using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {
    //public Gun gun;
    public Gun gun;

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.GetComponentsInChildren<Gun>() != null) {
            gun = other.gameObject.GetComponentsInChildren<Gun>()[0];
            gun.UpgradedGun();
            Destroy(gameObject);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpPickup);
        }
    }
}
