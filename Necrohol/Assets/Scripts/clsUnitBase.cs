using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clsUnitBase : MonoBehaviour {
    //number of tiles the unit can move
    public int movesRemaining;
    //health points
    public int hp;
    //base attack
    public int atk_base;
    //current attack
    public int atk_current;
    //is movement still available
    public bool canMove;

    public void MoveUnit (Vector3 destination) {
        this.gameObject.transform.Translate(destination);
    }

    public void OnMouseOver () {
        if (Input.GetMouseButton(0)) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().setSelected(this.gameObject);
        }
    }
}
