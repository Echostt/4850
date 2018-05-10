using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform launchPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
            if (!IsInvoking("fireBullet"))
                InvokeRepeating("fireBullet", 0, 0.1f);

        if (Input.GetMouseButton(1))
            CancelInvoke("fireBullet");
	}

    void fireBullet() {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = launchPosition.position;
        bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
    }

}
