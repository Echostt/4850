using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEquipper : MonoBehaviour {
    public static string activeWeaponType;
    public GameObject pistol;
    public GameObject assaultRifle;
    public GameObject shotgun;
    public GameObject activeGun;
    [SerializeField]
    GameUI gameUI;

	// Use this for initialization
	void Start () {
        activeWeaponType = Constants.Pistol;
        loadWeapon(pistol);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
            loadWeapon(pistol);
            activeWeaponType = Constants.Pistol;
            gameUI.UpdateReticle();
        } else if (Input.GetKeyDown("2")) {
            loadWeapon(assaultRifle);
            activeWeaponType = Constants.AssaultRifle;
            gameUI.UpdateReticle();
        } else if (Input.GetKeyDown("3")) {
            loadWeapon(shotgun);
            activeWeaponType = Constants.Shotgun;
            gameUI.UpdateReticle();
        }
	}

    public GameObject GetActiveWeapon () {
        return activeGun;
    }

    private void loadWeapon (GameObject weapon) {
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
        weapon.SetActive(true);
        activeGun = weapon;
    }
}
