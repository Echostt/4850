using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitHandler : MonoBehaviour {
    private Vector3 startPos;
    private bool isEnding;

    private void Start () {
        startPos = this.gameObject.transform.position;
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Bullet") && !isEnding) {
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score += 5;
            GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = "Score: " +
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score;

            isEnding = true;
            StartCoroutine(createNewTarget());
        }
    }

    private IEnumerator createNewTarget () {
        yield return new WaitForSeconds(3);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        GameObject nTarget = Instantiate(this.gameObject, startPos, Quaternion.identity);

        nTarget.GetComponent<BoxCollider>().enabled = true;
        nTarget.GetComponent<Rigidbody>().useGravity = false;
        nTarget.GetComponent<Rigidbody>().isKinematic = true;

        Destroy(this.gameObject);
        nTarget.GetComponent<HitHandler>().setPos(startPos);

    }

    public void setPos(Vector3 pos) { this.startPos = pos; }
}
