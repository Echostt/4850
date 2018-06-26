using System.Collections;
using UnityEngine;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    void Update() {
        if (Input.GetButton("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("PowerUp")) {
            StartCoroutine(givePowerUpShot(3, 0));
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
            } break;
        } //end switch(powerUpSelection)

    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        GetComponent<Rigidbody>().velocity = new Vector3(moveHorizontal, 0, moveVertical) * speed;

        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
