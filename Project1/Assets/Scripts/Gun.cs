using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform launchPosition;

    private AudioSource audioSource;
    public bool isUpgraded;
    public float upgradeTime = 10.0f;
    private float currentTime;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
            if (!IsInvoking("fireBullet"))
                InvokeRepeating("fireBullet", 0, 0.1f);

        if (Input.GetMouseButton(1))
            CancelInvoke("fireBullet");

        currentTime += Time.deltaTime;
        if (currentTime > upgradeTime && isUpgraded == true) 
            isUpgraded = false;
        
	}

    void fireBullet() {
        Rigidbody bullet = createBullet();
        bullet.velocity = transform.parent.forward * 100;
        if (isUpgraded) {
            Rigidbody bullet2 = createBullet();
            bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;
            Rigidbody bullet3 = createBullet();
            bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
        } else {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire);
        }
    }

    private Rigidbody createBullet () {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = launchPosition.position;
        return bullet.GetComponent<Rigidbody>();
    }

    public void UpgradedGun () {
        isUpgraded = true;
        currentTime = 0;
    }

}
