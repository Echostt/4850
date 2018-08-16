using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<GameObject> enemies;
    public List<GameObject> friendlies;
    public GameObject selected = null;
    public GameObject target = null;
    public float lastClicked = 0;
    public float reclickBuffer = 0.3f;
    public GameObject highlight;
    public GameObject highlightTarget;
    public GameObject cam;
    public bool isAttacking;
    public CutsceneManager csm;

    private bool isTargetting = false;
    private bool isPlayerTurn = false;

    //intro cs
    //give player control
    //enemy turn handle
    //r

    public void Start () {
        isPlayerTurn = true;

        //load arrays with units
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("FriendlyUnit")) {
            friendlies.Add(unit);
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("EnemyUnit")) {
            enemies.Add(unit);
        }
    }

    public void startGame () {
        csm.playScene(0);
        playerTurnStart();
    }

    //current selected unit attacks target
    public void attack () {
        clsUnitBase attacker = selected.GetComponent<clsUnitBase>();
        clsUnitBase defender = target.GetComponent<clsUnitBase>();
        defender.hp -= attacker.atk_current;
        Debug.Log(attacker + " did " + attacker.atk_current + " damage to " + defender);
        if (defender.hp <= 0) {
            //remove unit from enemy/friendly list
            removeGameObjectUnit(defender);
            Destroy(highlightTarget);
            target = null;
        }
    }

    /// <summary>
    /// First unit attacks second unit with standard damage calc.
    /// </summary>
    /// <param name="attacker">The unit attacking</param>
    /// <param name="defender">The unit defending</param>
    public void attackSelected (clsUnitBase attacker, clsUnitBase defender) {
        defender.hp -= attacker.atk_current;
        Debug.Log(attacker + " did " + attacker.atk_current + " damage to " + defender);
        if (defender.hp <= 0) {
            //remove unit from enemy/friendly list
            removeGameObjectUnit(defender);
            if (enemies.Count == 0) {
                Debug.Log("Quit");
                Application.Quit();
            }
        }
    }

    public void enemyTurnStart () {
        isPlayerTurn = false;
        cam.GetComponent<CameraController>().isMovingToTarget = true;
        cam.GetComponent<CameraController>().setAdjustedTargetLocation(enemies[0].transform.position);
        StartCoroutine(enemyTurnController());
    }

    /// <summary>
    /// Handles all enemy's interactions during their turn.
    /// </summary>
    /// <returns></returns>
    public IEnumerator enemyTurnController () {
        for (int i = 0; i < enemies.Count; ++i) {
            //each unit moves toward player, or attacks
            highlighter(enemies[i]);
            cam.GetComponent<CameraController>().setAdjustedTargetLocation(enemies[i].gameObject.transform.position);

            //find closest enemy
            float distToClosestUnit = 100;
            GameObject closestGO = null;
            for(int j = 0; j < friendlies.Count; ++j) {
                float distToClosestUnitX = enemies[i].gameObject.transform.position.x - friendlies[j].gameObject.transform.position.x;
                float distToClosestUnitZ = enemies[i].gameObject.transform.position.z - friendlies[j].gameObject.transform.position.z;
                float distTmp = Mathf.Sqrt(distToClosestUnitX * distToClosestUnitX + distToClosestUnitZ * distToClosestUnitZ);
                if (distTmp < distToClosestUnit) {
                    distToClosestUnit = distTmp;
                    closestGO = friendlies[j].gameObject;
                }
            }
            //select closest tile
            Vector3 direction = Vector3.zero;
            if (enemies[i].transform.position.x < closestGO.transform.position.x) { // LR movement < >
                direction = Vector3.right;
            } else if (enemies[i].transform.position.x > closestGO.transform.position.x) {
                direction = Vector3.left;
            }

            if (enemies[i].transform.position.z < closestGO.transform.position.z) { // UD movement ^ v
                direction = Vector3.forward;
            } else if (enemies[i].transform.position.z > closestGO.transform.position.z) {
                direction = Vector3.back;
            }

            RaycastHit[] hit = Physics.RaycastAll(enemies[i].transform.position, direction, 1.0f);
            if (hit.Length > 0) {
                //hit enemy or move up tile that is collided with
                if (hit[0].collider.gameObject.GetComponent<clsUnitBase>() != null) {
                    attackSelected(enemies[i].GetComponent<clsUnitBase>(), hit[0].collider.gameObject.GetComponent<clsUnitBase>());
                } else {
                    enemies[i].transform.Translate(Vector3.up);
                    enemies[i].transform.Translate(direction);
                }
            } else {
                enemies[i].transform.Translate(direction);
            }
            //wait short period between unit moving
            yield return new WaitForSecondsRealtime(0.5f);
        }

        selected = null;
        target = null;
        
        cam.GetComponent<CameraController>().isRotating180 = true;
        StartCoroutine(rotateCamera180Enemy());
        StartCoroutine(enemyTurnEnd());
    }

    public IEnumerator enemyTurnEnd () {
        if (cam.GetComponent<CameraController>().isRotating180 == false) {
            playerTurnStart();
            StopCoroutine(enemyTurnEnd());
        } else {
            yield return new WaitForSecondsRealtime(0.2f);
            StartCoroutine(enemyTurnEnd());
        }
    }

    /// <summary>
    /// Create a spot light above the passed gameObject.
    /// </summary>
    /// <param name="gameObject">Object to create light above.</param>
    private void highlighter (GameObject gameObject) {
        Destroy(highlight);
        //set light to new passed in color ------------------------------------------------------------------------------------
        //Debug.Log("Light setting to " + gameObject);
        highlight = Instantiate((GameObject)Resources.Load("Spot Light"));
        highlight.transform.SetPositionAndRotation(new Vector3(
            gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y + 4,
            gameObject.transform.localPosition.z), Quaternion.Euler(90, 0, 0));
    }

    private void highlighterTarget (GameObject gameObject) {
        Destroy(highlightTarget);
        //Debug.Log("Light setting to " + gameObject);
        highlightTarget = Instantiate((GameObject)Resources.Load("Spot Light Target"));
        highlightTarget.transform.SetPositionAndRotation(new Vector3(
            gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y + 4,
            gameObject.transform.localPosition.z), Quaternion.Euler(90, 0, 0));
    }

    /// <summary>
    /// Interface function for UI button to end player turn.
    /// </summary>
    public void playerTurnEnd () {
        //reset targets and spotlights for next round
        selected = null;
        target = null;
        Destroy(highlight);
        Destroy(highlightTarget);

        cam.GetComponent<CameraController>().isRotating180 = true;
        StartCoroutine(rotateCamera180Player());
    }


    //set game state to start of player's turn
    public void playerTurnStart () {
        //Debug.Log("Player turn start.");
        Destroy(highlight);
        Destroy(highlightTarget);
        isPlayerTurn = true;
        //move camera to first player unit on the list
        Vector3 unitPos = friendlies[0].transform.position;
        cam.transform.position = new Vector3(unitPos.x, unitPos.y + 6, unitPos.z - 4);
        cam.transform.rotation = Quaternion.Euler(46.46f, 10, 0);

        for (int i = 0; i < friendlies.Count; ++i) //reset every unit back to moveable
            friendlies[i].GetComponent<clsUnitBase>().canMove = true;
    }

    public void removeGameObjectUnit (clsUnitBase tbd) {
        if (tbd.gameObject.CompareTag("FriendlyUnit")) {
            friendlies.Remove(tbd.gameObject);
        } else if (tbd.gameObject.CompareTag("EnemyUnit")) {
            enemies.Remove(tbd.gameObject);
        } else {
            Debug.Log("removeGameObjectUnit could not destroy " + tbd.gameObject);
        }
        Destroy(tbd.gameObject);
    }

    /// <summary>
    /// Rotates the camera 180 degrees in the world Y+ direction at the end of Player turn.
    /// </summary>
    public IEnumerator rotateCamera180Player () {
        //turn camera to face opposite direction
        //Debug.Log("Is camera rotating? " + cam.GetComponent<CameraController>().isRotating180);
        if (cam.GetComponent<CameraController>().isRotating180 == false) {
            //start enemy turn
            StopCoroutine(rotateCamera180Player());
            enemyTurnStart();
        } else {
            yield return new WaitForSecondsRealtime(0.2f);
            StartCoroutine(rotateCamera180Player());
        }
    }

    /// <summary>
    /// Rotates the camera 180 degrees in the world Y+ direction at the end of Enemy turn.
    /// </summary>
    public IEnumerator rotateCamera180Enemy () {
        //turn camera to face opposite direction
        //Debug.Log("Is camera rotating? " + cam.GetComponent<CameraController>().isRotating180);
        if (cam.GetComponent<CameraController>().isRotating180 == false) {
            //start enemy turn
            StopCoroutine(rotateCamera180Enemy());
        } else {
            //Debug.Log("EnemyTurnEnd rotating");
            yield return new WaitForSecondsRealtime(0.2f);
            StartCoroutine(rotateCamera180Enemy());
        }
    }

    public void setSelected(GameObject selection) {
        if (lastClicked < Time.time - reclickBuffer) { //prevent overclicking
            lastClicked = Time.time;
            if (!isTargetting) {
                this.selected = selection;
                highlighter(selected);
                cam.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y + 6, selected.transform.position.z - 4);
                Debug.Log("Selected: " + selection);
                isTargetting = true;
            } else {
                this.target = selection;
                Debug.Log("Targetting: " + selection);
                selectionHandler();
            }
        }
    }

    private void selectionHandler () {
        if (selected != null && selected.GetComponent<clsUnitBase>() != null) { //selection is a unit, target is a 
            if (target != null && target.GetComponent<clsUnitBase>() != null) { //unit
                Debug.Log("Unit to Unit");
                //move unit to closest adjacent tile, and attack ---------------------------------------------------------
                highlighterTarget(target);

            } else if (target != null && target.GetComponent<clsGroundTileBase>() != null) { //tile
                Debug.Log("Unit to Tile");
                //move unit to tile
                if (selected.GetComponent<clsUnitBase>().canMove) { //unit can still move
                    Vector3 dest = target.transform.position;
                    dest.y += 1;
                    selected.transform.position = dest;
                    selected.GetComponent<clsUnitBase>().canMove = false;
                    Destroy(highlight);
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
