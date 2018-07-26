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
    private bool isTargetting = false;
    public GameObject highlight;

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
            }
        }
    }

    public void Update () {
        
    }

    private void selectionHandler () {
        if (selected != null && selected.GetComponent<clsUnitBase>() != null) { //selection is a unit, target is a 
            if (target != null && target.GetComponent<clsUnitBase>() != null) { //unit
                Debug.Log("Unit to Unit");
            } else if (target != null && target.GetComponent<clsGroundTileBase>() != null) { //tile
                Debug.Log("Unit to Tile");
            } else {                                           // ?? Unknown selection ??
                Debug.Log("Unit to ???");
                Debug.Log("Target: " + target);
            }
        } else { //selection is a tile
            Debug.Log("selectionHandler tile " + selected);
        }
        isTargetting = false;
    }

    private void highlighter (GameObject gameObject) {
        Destroy(highlight);
        Debug.Log("Light setting to " + gameObject);
        highlight = Instantiate((GameObject)Resources.Load("Spot Light"));
        highlight.transform.SetPositionAndRotation( new Vector3 (
            gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y + 4,
            gameObject.transform.localPosition.z), Quaternion.Euler(90, 0, 0));
    }
}
