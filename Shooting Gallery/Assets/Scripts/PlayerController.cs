using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject bulletDefault;
    public GameObject bulletPowerUp;
    public AudioClip sound;

    public bool isPoweredUp;
    public float fireRate;
    public int range;
    public int score;

    private float lastFireTime;

    private void Fire () {
        if (!isPoweredUp) {
            GameObject bullet = Instantiate(bulletDefault, this.gameObject.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 30, ForceMode.Impulse);
        } else {
            isPoweredUp = false;
            for (int i = 0; i < 15; ++i) {
                GameObject bullet = Instantiate(bulletPowerUp, this.gameObject.transform.position, Quaternion.identity);
                float randDir = Random.Range(0, 10);
                Vector3 straight = Camera.main.transform.forward;
                int randScale = i % 2 == 0 ? -1 : 1;
                straight.x += randScale * randDir/40;
                straight.y += randDir/30;
                bullet.GetComponent<Rigidbody>().AddForce(straight * 130, ForceMode.Impulse);
            }
            stopPowerUpAudio();
        }
    }

    private void Update () {
        if (Input.GetButton("Fire1") && Time.time - lastFireTime > fireRate) {
            lastFireTime = Time.time;
            Fire();
        } 
    }

    public void playPowerUpAudio () {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        for (int i = 0; i < lights.Length; ++i) {
            lights[i].GetComponent<Light>().intensity = 8;
            //Debug.Log("Turning up light: " + lights[i].gameObject);
        }
        GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().intensity = 0;
        this.GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void stopPowerUpAudio () {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        for (int i = 0; i < lights.Length; ++i) {
            lights[i].GetComponent<Light>().intensity = 0;
            //Debug.Log("Turning down light: " + lights[i].gameObject);
        }
        GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().intensity = 1;
        this.GetComponent<AudioSource>().Stop();
    }
}
