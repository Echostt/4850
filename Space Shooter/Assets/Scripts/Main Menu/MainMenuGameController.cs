using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGameController : MonoBehaviour {
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
    public Text SpaceShooterText;

    private bool gameOver;
    private float textScaleFactor;
    private int currentEnemies;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        enemiesRemaining = enemyMax;
        gameOver = false;
        StartCoroutine(SpawnWaves());
        textScaleFactor = 0.1f;
        StartCoroutine(fluxMenuText());
    }

    public void startMainScene () {
        SceneManager.LoadScene("Main");
    }

    public void closeGame () {
        Application.Quit();
    }

    private IEnumerator fluxMenuText () {
        while (true) {
            if (SpaceShooterText.transform.localScale.x <= 0.9)
                textScaleFactor = 0.03f;
            else if (SpaceShooterText.transform.localScale.x >= 1.1)
                textScaleFactor = -0.03f;

            //adjust scale factors
            SpaceShooterText.transform.localScale = new Vector3(
                    SpaceShooterText.transform.localScale.x + textScaleFactor,
                    SpaceShooterText.transform.localScale.y + textScaleFactor,
                    SpaceShooterText.transform.localScale.z
            );

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator flyOff () {
        while (true) {
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 350), ForceMode.Acceleration);
            yield return new WaitForSeconds(0.2f);
            //when player off screen transition
            if (player.transform.position.z > 15) {
                Debug.Log("Start next scene");
            }
        }
    }

    IEnumerator SpawnWaves () {
        yield return new WaitForSeconds(startWait);
        while (enemiesRemaining > 0) {
            if (gameOver) {
                GameObject playerCopy = player;
                Destroy(player);
                Instantiate(playerCopy);
                playerCopy.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                player = playerCopy;
                Destroy(playerCopy);
                gameOver = false;
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


    public void GameOver () {
        gameOver = true;
    }
}
