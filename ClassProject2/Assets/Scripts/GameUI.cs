using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField]
    Sprite redReticle;
    [SerializeField]
    Sprite yellowReticle;
    [SerializeField]
    Sprite blueReticle;
    [SerializeField]
    Image reticle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateReticle () {
        switch (GunEquipper.activeWeaponType) {
            case Constants.Pistol: reticle.sprite = redReticle; break;
            case Constants.AssaultRifle: reticle.sprite = blueReticle; break;
            case Constants.Shotgun: reticle.sprite = yellowReticle; break;
            default: return;
        }
    }
}
