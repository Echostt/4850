using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject hazard;
    public GameObject enemy;
    [SerializeField]
    public GameObject[] powerUps;

    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int enemyCount;
    public int enemyMax;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private int score;
    private bool gameOver;
    private bool restart;

    void Start() {
        score = 0;
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
    }

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (true) {
            if (gameOver) {
                restartText.text = "Press 'R' to Restart";
                restart = true;
                break;
            }
            for (int i = 0; i < hazardCount; ++i) {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                int randVal = Mathf.FloorToInt(Random.value * 10);
                if ( randVal < 3 && enemyCount < enemyMax) { 
                    // ~1/4 of the time spawn an enemy ship
                    Vector3 spawnPositionEnemy = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Instantiate(enemy, spawnPositionEnemy, Quaternion.Euler(0, 180, 0));
                    enemyCount++;
                } else if (randVal == 5) {
                    // ~ 1/5th of the time spawn a powerup
                    // should look like you blow up an asteroid and receive a powerup
                    if (enemyCount > 2)
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
        score += newScoreValue;
        updateScore();
    }

    void updateScore() {
        scoreText.text = "Score: " + score;
    }

    public void GameOver() {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
