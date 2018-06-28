using System.Collections;
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
        this.moveSpeedHorizontal = this.moveSpeedVertical;
        //pick random direction to move first
        randDir = 1;
        if (Mathf.Round(Random.value) == 1)
            randDir *= -1;
        StartCoroutine(fireShot());
    }

    private void OnDestroy () {
        GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (gc != null) {
            gc.enemiesRemaining -= 1;
        }
    }

    private void Update () {
        //strafe
        if (this.GetComponent<Rigidbody>().transform.position.x <= -6 && this.moveSpeedHorizontal * randDir < 0) {
            //off the left
            this.moveSpeedHorizontal *= -1;
        } else if (this.GetComponent<Rigidbody>().transform.position.x >= 6 && this.moveSpeedHorizontal * randDir > 0) {
            //off the right
            this.moveSpeedHorizontal *= -1;
        } else {
            this.GetComponent<Rigidbody>().velocity = new Vector3(moveSpeedHorizontal * randDir, 0, moveSpeedVertical);
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
