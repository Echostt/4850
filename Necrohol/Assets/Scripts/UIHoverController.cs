using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIHoverController : MonoBehaviour {

    public bool isUIOverride { get; private set; }

    void Update () {
        // True if hovering any UI elements
        isUIOverride = EventSystem.current.IsPointerOverGameObject();
    }


}
