using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandlerSpecial : MonoBehaviour {
    private void OnCollisionEnter (Collision collision) {
        //collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Impulse);
        Vector3 moveDir = collision.rigidbody.velocity;
        moveDir.x *= -0.5f;
        moveDir.y *= -0.5f;
        collision.gameObject.GetComponent<Rigidbody>().AddForce(moveDir, ForceMode.Impulse);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPoweredUp = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().playPowerUpAudio();
        Instantiate(Resources.Load("ParticleRed"), this.gameObject.transform);
    }
}
