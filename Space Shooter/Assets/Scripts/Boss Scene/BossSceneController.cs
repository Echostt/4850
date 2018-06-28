using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossSceneController : MonoBehaviour {
    public Text restartText;
    public Text gameOverText;
    public int chargedShotsRemaining;
    public PlayerController player;

    private bool gameOver;
    private float timeHeldCharging;
    private float timeToCharge;
    private bool isCharging;
    private bool isTransitioningScene;

    public void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        isCharging = false;
        restartText = GameObject.Find("RestartText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        restartText.text = "";
        gameOverText.text = "";
    }

    public void Update () {
        if (isTransitioningScene) {
            //move player ship to final fly point
            Vector3 target = Vector3.MoveTowards(player.transform.position, Vector3.zero, Time.deltaTime * 3);
            //Debug.Log("Moving toward: " + target);
            if (target != player.transform.position) {
                player.GetComponent<Rigidbody>().transform.position = target;
            } else {
                //fly off screen, load next scene
                isTransitioningScene = false;
                StartCoroutine(flyOff());
            }
        }

        if (Input.GetButton("Fire1")) {
            player.toFire = false;
            if (timeHeldCharging > timeToCharge && isCharging && chargedShotsRemaining > 0) {
                //fire charged shot player
                player.fireChargedShot();
                isCharging = false;
                chargedShotsRemaining -= 1;
            } else if (!isCharging) {
                player.fireShot();
                timeToCharge = Time.time + 1;
                isCharging = true;
            } else {
                timeHeldCharging += Time.deltaTime;
            }
        } else {
            isCharging = false;
            player.toFire = true;
        }

        if (gameOver) {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("Main");
        }
    }

    public void bossDeath () {
        StaticStats.Score += 100;
        //disable player and start fly away sequence
        player.GetComponent<MeshCollider>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        isTransitioningScene = true;
    }

    public void GameOver () {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }

    private IEnumerator flyOff () {
        while (true) {
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 150), ForceMode.Acceleration);
            yield return new WaitForSeconds(0.2f);
            //when player off screen transition
            if (player.transform.position.z > 15) {
                //bring in score
                bringInScore();
                break;
            }
        }
    }

    private void bringInScore () {
        gameOverText.text = "You win!\nScore: " + StaticStats.Score;
        StopAllCoroutines();
        StartCoroutine(backToMain());
    }

    private IEnumerator backToMain() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
}
