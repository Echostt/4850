using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clsGroundTileBase : MonoBehaviour {
    public int moveCost;
    public int defBonus;

    private UIHoverController uiListener;

    void Start () {
        uiListener = GameObject.Find("Canvas").GetComponent<UIHoverController>();
    }

    void OnMouseDown () {
        if (uiListener.isUIOverride) {
            Debug.Log("Cancelled OnMouseDown! A UI element has override this object!");
        } else {
            Debug.Log("Object OnMouseDown");
            if (Input.GetMouseButton(0)) {
                Debug.Log("TileClick: " + this.gameObject);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().setSelected(this.gameObject);
            }
        }
    }
}
