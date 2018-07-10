using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int health;
    public int armor;
    public GameUI gameUI;
    private GunEquipper gunEquipper;
    private Ammo ammo;

	// Use this for initialization
	void Start () {
        ammo = this.GetComponent<Ammo>();
        gunEquipper = this.GetComponent<GunEquipper>();
	}
	
	public void TakeDamage(int amount) {
        int healthDamage = amount;

        if (armor > 0) {
            int effectiveArmor = armor * 2;
            effectiveArmor -= healthDamage;
            if (effectiveArmor > 0) {
                armor = effectiveArmor / 2;
                return;
            }
            armor = 0;
        }

        health -= healthDamage;
        Debug.Log("Health is: " + health);

        if (health <= 0) {
            Debug.Log("GAMEOVER");
        }

    }
}
