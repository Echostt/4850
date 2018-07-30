using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject[] enemies;
    public GameObject[] friendlies;
    public GameObject selected = null;
    public GameObject target = null;
    public float lastClicked = 0;
    public float reclickBuffer = 0.3f;
    public GameObject highlight;
    public GameObject cam;
    public bool isAttacking;

    private bool isTargetting = false;
    private bool isPlayerTurn = false;

    //intro cs
    //give player control
    //enemy turn handle
    //r

    public void Start () {
        isPlayerTurn = true;

        //load arrays with units
        friendlies = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
    }

    //current selected unit attacks target
    public void attack() {
        clsUnitBase attacker = selected.GetComponent<clsUnitBase>();
        clsUnitBase defender = target.GetComponent<clsUnitBase>();
        defender.hp -= attacker.atk_current;
        Debug.Log(attacker + " did " + attacker.atk_current + " damage to " + defender);
    }

    //create a highlight above the passed gameobject
    private void highlighter (GameObject gameObject) {
        Destroy(highlight);
        Debug.Log("Light setting to " + gameObject);
        highlight = Instantiate((GameObject)Resources.Load("Spot Light"));
        highlight.transform.SetPositionAndRotation(new Vector3(
            gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y + 4,
            gameObject.transform.localPosition.z), Quaternion.Euler(90, 0, 0));
    }

    //set of events that happens immediately after ending the player's turn
    public void playerTurnEnd () {
        //turn camera to face opposite direction
        cam.transform.SetPositionAndRotation(new Vector3(
            enemies[0].transform.position.x,
            enemies[0].transform.position.y + 4,
            enemies[0].transform.position.z), Quaternion.Euler(46.46f, 190, 0));
        //start enemy turn
    }

    //set game state to start of player's turn
    public void playerTurnStart () {
        isPlayerTurn = true;
        //move camera to first player unit on the list
        Vector3 unitPos = friendlies[0].transform.position;
        cam.transform.position = new Vector3(unitPos.x, unitPos.y + 4, unitPos.z - 2);

        for (int i = 0; i < enemies.Length; ++i) //reset every unit back to moveable
            friendlies[i].GetComponent<clsUnitBase>().canMove = true;
    }

    public void setAttackingTrue () { isAttacking = true; }

    public void setSelected(GameObject selection) {
        if (lastClicked < Time.time - reclickBuffer) { //prevent overclicking
            lastClicked = Time.time;
            if (!isTargetting) {
                this.selected = selection;
                highlighter(selected);
                Debug.Log("Selected: " + selection);
                isTargetting = true;
            } else {
                this.target = selection;
                Debug.Log("Targetting: " + selection);
                selectionHandler();
                Destroy(highlight);
            }
        }
    }

    private void selectionHandler () {
        if (selected != null && selected.GetComponent<clsUnitBase>() != null) { //selection is a unit, target is a 
            if (target != null && target.GetComponent<clsUnitBase>() != null) { //unit
                Debug.Log("Unit to Unit");
                //move unit to closest adjacent tile, and attack --------------------------------------------
                if (isAttacking) {
                    attack();
                }

            } else if (target != null && target.GetComponent<clsGroundTileBase>() != null) { //tile
                Debug.Log("Unit to Tile");
                //move unit to tile
                if (selected.GetComponent<clsUnitBase>().canMove) { //unit can still move
                    Vector3 dest = target.transform.position;
                    dest.y += 1;
                    selected.transform.position = dest;
                    selected.GetComponent<clsUnitBase>().canMove = false;
                }
                
            } else {                                           // ?? Unknown selection ??
                Debug.Log("Unit to ???");
                Debug.Log("Target: " + target);
            }
        } else { //selection is a tile
            Debug.Log("selectionHandler tile " + selected);
        }
        isTargetting = false;
    }

}
