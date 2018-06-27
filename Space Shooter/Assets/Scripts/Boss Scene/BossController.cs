using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    public float fireRate;
    public GameObject shot;
    [SerializeField]
    public Transform[] shotSpawn;
    public float moveSpeedVertical;
    public float moveSpeedHorizontal;
    public GameObject explosion;
    public float shotRotateSpeed;
    public int HP;

    private float shotAngleLeft;
    private float shotAngleRight;

    private int randDir;

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("PlayerShot")) {
            Destroy(other.gameObject);
            Instantiate(explosion, other.transform.position, transform.rotation);
            this.HP -= 1;
            if (this.HP <= 0) {
                //boss beat condition
                Debug.Log("Boss beated condition");
            }
        }
    }

    private void Start () {
        shotAngleLeft = 0;
        shotAngleRight = 0;
        //pick random direction to move first
        randDir = 1;
        if (Mathf.Round(Random.value) == 1)
            randDir *= -1;
        StartCoroutine(fireShot());
    }

    private void Update () {
        //strafe
        if (this.GetComponent<Rigidbody>().transform.position.x <= -6 && this.moveSpeedHorizontal * randDir < 0) {
            //off the left
            this.moveSpeedHorizontal *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.x >= 6 && this.moveSpeedHorizontal * randDir > 0) {
            //off the right
            this.moveSpeedHorizontal *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.z <= 9 && this.moveSpeedVertical < 0) {
            this.moveSpeedVertical *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.z >= 15 && this.moveSpeedVertical > 0) {
            this.moveSpeedVertical *= -1;
        } else {
            this.GetComponent<Rigidbody>().velocity = new Vector3(moveSpeedHorizontal * randDir, 0, moveSpeedVertical);
        }
    }

    IEnumerator fireShot () {
        while (true) {
            //slightly rotate shotSpawns
            if (shotSpawn[0].rotation.y < 0.95) {
                shotRotateSpeed *= -1;
            } 
            shotSpawn[0].Rotate(Vector3.down, shotRotateSpeed);
            shotSpawn[1].Rotate(Vector3.down, shotRotateSpeed);
            //fire
            Instantiate(shot, shotSpawn[0].position, shotSpawn[0].rotation);
            Instantiate(shot, shotSpawn[1].position, shotSpawn[1].rotation);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(fireRate);
        }
    }

}
