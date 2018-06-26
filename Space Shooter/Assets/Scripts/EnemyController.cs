using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float fireRate;
    public GameObject shot;
    public Transform shotSpawn;

    private int randDir;
    private float moveSpeedVertical;
    private float moveSpeedHorizontal;

    private void Start () {
        //get movement speed from Mover
        this.moveSpeedVertical = this.GetComponent<Mover>().speed;
        //pick random direction to move first
        randDir = 1;
        if (Mathf.Round(Random.value) == 1)
            randDir *= -1;
        Debug.Log(randDir);
        StartCoroutine(fireShot());
    }

    private void Update () {
        //strafe
        if (this.GetComponent<Rigidbody>().transform.position.x <= -6) {
            //off the left
            this.transform.Translate(12, 0, 0);
            this.moveSpeedHorizontal *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.x >= 6) {
            //off the right
            this.transform.Translate(-12, 0, 0);
            this.moveSpeedHorizontal *= -1;
        } else {
            this.GetComponent<Rigidbody>().velocity.Set(moveSpeedHorizontal * randDir, 0, moveSpeedVertical);
        }        
    }

    IEnumerator fireShot () {
        while (true) {
            //fire
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(fireRate);
        }
    }
}
