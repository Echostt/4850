using UnityEngine;

public class AssaultRifle : Gun {
    protected override void Update () {
        base.Update();
        //assaultrifle has auto fire
        if(Input.GetMouseButton(0) && (Time.time - lastFireTime) > fireRate) {
            lastFireTime = Time.time;
            Fire();
        }
    }
}
