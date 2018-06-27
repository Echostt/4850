using UnityEngine;
using UnityEngine.UI;

public class BossSceneController : MonoBehaviour {
    public Text restartText;
    public Text gameOverText;

    private bool gameOver;
    private bool isFightStarted;

    public void Start () {
        isFightStarted = false;
        restartText = GameObject.Find("RestartText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        restartText.text = "";
        gameOverText.text = "";
    }

    public void GameOver () {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
