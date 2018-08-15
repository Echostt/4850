using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {
    public GameObject canvas;

    public void playScene(int scene) {
        GameObject advanceText = (GameObject)Instantiate(Resources.Load("btnTextAdvance"));
        advanceText.transform.SetParent(canvas.transform);
        switch (scene) {
            case 0: {
                //disable UI buttons
                Transform[] children = canvas.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; ++i) {
                    if (children[i].gameObject.CompareTag("UI"))
                        children[i].gameObject.SetActive(false);
                }
                GameObject dialogue = (GameObject)Instantiate(Resources.Load("dialogueBG"));
                dialogue.transform.SetParent(canvas.transform);
                dialogue.transform.Translate(Vector3.right * 500);

                GameObject left = (GameObject)Instantiate(Resources.Load("UIFriendly"));
                left.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
                left.transform.localPosition = new Vector3(-5, 1, 3);

                GameObject text = (GameObject)Instantiate(Resources.Load("UIText/txt_MakeIt"));
                text.transform.SetParent(canvas.transform);
                text.transform.position = new Vector3(350, 100, 0);
                break;
            }
        }
    }
}
