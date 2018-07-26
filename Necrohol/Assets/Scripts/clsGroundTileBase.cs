using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clsGroundTileBase : MonoBehaviour {
    public int moveCost;
    public int defBonus;

    public void OnMouseOver () {
        if (Input.GetMouseButton(0)) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().setSelected(this.gameObject);
        }
    }
}
