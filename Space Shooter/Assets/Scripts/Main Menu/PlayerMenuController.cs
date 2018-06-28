using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour {
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;


    void Update () {
        if (Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("PowerUp")) {
            StartCoroutine(givePowerUpShot(3, 0));
            Destroy(other.gameObject);
        }
    }

    IEnumerator givePowerUpShot (int powerUpDuration, int powerUpSelection) {
        switch (powerUpSelection) {
            case 0: {
                float powerUpStart = Time.time;
                this.fireRate /= 50;
                while (powerUpDuration > Time.time - powerUpStart) {
                    yield return null;
                }
                this.fireRate *= 50;
            }
            break;
        } //end switch(powerUpSelection)

    }

    void FixedUpdate () {
        //strafe
        if (this.GetComponent<Rigidbody>().transform.position.x <= -4 && this.speed > 0) {
            //off the left
            this.speed *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.x >= 4 && this.speed < 0) {
            //off the right
            this.speed *= -1;
        } else {
            this.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(this.GetComponent<Rigidbody>().position, Vector3.left * speed, Time.deltaTime * Mathf.Abs(speed)));
        }

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

}
