using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {
    public GameObject canvas;
    public int nextScene;

    private GameObject dialogueBG = null;
    private GameObject advanceTextBtn = null;
    private List<Transform> children = new List<Transform>();
    private List<GameObject> UIMembers = new List<GameObject>();

    public void playScene(int scene) {
        //disable UI buttons
        Transform[] childrens = canvas.GetComponentsInChildren<Transform>();
        for (int i = 0; i < childrens.Length; ++i) {
            if (childrens[i].gameObject.CompareTag("UI")) {
                children.Add(childrens[i]);
                childrens[i].gameObject.SetActive(false);
            }
        }

        //red bg
        if (!GameObject.Find("dialogueBG(Clone)")) {
            dialogueBG = (GameObject)Instantiate(Resources.Load("dialogueBG"));
            dialogueBG.transform.SetParent(canvas.transform);
            dialogueBG.transform.Translate(Vector3.right * 500);
        }

        //advance text btn
        if (!GameObject.Find("btnTextAdvance(Clone)")) {
            advanceTextBtn = (GameObject)Instantiate(Resources.Load("btnTextAdvance"));
            advanceTextBtn.transform.SetParent(canvas.transform);
            advanceTextBtn.transform.localPosition = new Vector3(300, -150, 0);
            Button bt = advanceTextBtn.GetComponent<Button>();
            bt.onClick.AddListener(playNextScene);
        }

        switch (scene) {
            case -1: {
                hideCutscene(); break;
            }
            case 0: {
                GameObject left = (GameObject)Instantiate(Resources.Load("UIFriendly"));
                UIMembers.Add(left);
                left.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
                left.transform.localPosition = new Vector3(-5, 1, 3);

                GameObject text = (GameObject)Instantiate(Resources.Load("UIText/txt_MakeIt"));
                UIMembers.Add(text);
                text.transform.SetParent(canvas.transform);
                text.transform.position = new Vector3(350, 100, 0);
                //cutscenes are ordered by number, set the next before ending
                nextScene = 1;
                break;
            }
            case 1: {
                GameObject right = (GameObject)Instantiate(Resources.Load("UIFriendly"));
                UIMembers.Add(right);
                right.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
                right.transform.localPosition = new Vector3(5, 1, 3);

                GameObject text = (GameObject)Instantiate(Resources.Load("UIText/txt_Ellipse"));
                UIMembers.Add(text);
                text.transform.SetParent(canvas.transform);
                text.transform.position = new Vector3(350, 100, 0);
                //cutscenes are ordered by number, set the next before ending
                nextScene = 2;
                break;
            }
            case 2: {
                GameObject right = (GameObject)Instantiate(Resources.Load("UIFriendly"));
                UIMembers.Add(right);
                right.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
                right.transform.localPosition = new Vector3(5, 1, 3);

                GameObject text = (GameObject)Instantiate(Resources.Load("UIText/txt_NoChance"));
                UIMembers.Add(text);
                text.transform.SetParent(canvas.transform);
                text.transform.position = new Vector3(350, 100, 0);
                //cutscenes are ordered by number, set the next before ending
                nextScene = -1;
                break;
            }
        }
    }

    public void playNextScene () {
        for(int i = 0; i < UIMembers.Count; ++i) {
            Destroy(UIMembers[i]);
        }
        UIMembers.Clear();

        playScene(nextScene);
    }

    public void hideCutscene () {
        Destroy(dialogueBG);
        Destroy(advanceTextBtn);
        for (int i = 0; i < children.Count; ++i) {
            children[i].gameObject.SetActive(true);
        }
    }

}
