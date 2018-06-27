using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;
    private BossSceneController bossSceneController;

    void Start() {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject.GetComponent<GameController>() != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else { //find boss scene controller
            bossSceneController = gameControllerObject.GetComponent<BossSceneController>();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (gameController != null) { // game controller found
            if (other.CompareTag("Player")) {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                Destroy(other.gameObject);
                Destroy(gameObject);
            } else if (other.CompareTag("PlayerShot") || other.CompareTag("Asteroid")) {
                Instantiate(explosion, transform.position, transform.rotation);
                gameController.AddScore(scoreValue);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        } else { //otherwise it's a boss fight!
            if (other.CompareTag("Player")) {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                bossSceneController.GameOver();
                Destroy(other.gameObject);
                Destroy(gameObject);
            } else if (other.CompareTag("PlayerShot")) {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
