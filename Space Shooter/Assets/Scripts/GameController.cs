using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject hazard;
    public GameObject enemy;
    [SerializeField]
    public GameObject[] powerUps;
    public GameObject player;

    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int enemiesRemaining;
    public int enemyMax;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private int score;
    private bool gameOver;
    private bool restart;
    private bool isTransitioningScene;
    private int currentEnemies;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        isTransitioningScene = false;
        enemiesRemaining = enemyMax;
        StaticStats.Score = 0;
        gameOver = false;
        restart = false;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        restartText = GameObject.Find("RestartText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        restartText.text = "";
        gameOverText.text = "";
        updateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update() {
        if (restart) {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("Main");
        }
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
                enemiesRemaining = 1;
            }
        }
        if (enemiesRemaining <= 0 && !isTransitioningScene) {
            //move to boss scene
            StopAllCoroutines();
            startBossScene();
        }
    }

    private IEnumerator flyOff () {
        while (true) {
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 150), ForceMode.Acceleration);
            yield return new WaitForSeconds(0.2f);
            //when player off screen transition
            if (player.transform.position.z > 15) {
                SceneManager.LoadScene("BossFight");
            }
        }
    }

    public void startBossScene () {
        //disable player controls and fly to center
        isTransitioningScene = true;
        player.GetComponent<MeshCollider>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
    }

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (enemiesRemaining > 0) {
            if (gameOver) {
                restartText.text = "Press 'R' to Restart";
                restart = true;
                break;
            }

            for (int i = 0; i < hazardCount; ++i) {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                int randVal = Mathf.FloorToInt(Random.value * 10);
                if (randVal < 3 && currentEnemies < enemyMax) { 
                    // ~1/4 of the time spawn an enemy ship
                    Vector3 spawnPositionEnemy = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Instantiate(enemy, spawnPositionEnemy, Quaternion.Euler(0, 180, 0));
                    currentEnemies++;
                } else if (randVal == 5) {
                    // ~ 1/5th of the time spawn a powerup
                    // should look like you blow up an asteroid and receive a powerup
                    int powerUpChoice = Mathf.RoundToInt(Random.Range(0, 10));
                    if (powerUpChoice % 2 == 0)
                        Instantiate(powerUps[0], spawnPosition, Quaternion.Euler(180, 0, 90));
                    else
                        Instantiate(powerUps[1], spawnPosition, Quaternion.Euler(180, 0, 90));
                }
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void AddScore(int newScoreValue) {
        StaticStats.Score += newScoreValue;
        updateScore();
    }

    void updateScore() {
        scoreText.text = "Score: " + StaticStats.Score;
    }

    public void GameOver() {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
