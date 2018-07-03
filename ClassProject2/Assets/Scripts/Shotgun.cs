using UnityEngine;

public class Shotgun : Gun {
    protected override void Update () {
        base.Update();
        if(Input.GetMouseButtonDown(0) && (Time.time - lastFireTime) > fireRate) {
            lastFireTime = Time.time;
            Fire();
        }
    }
}
