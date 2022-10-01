using System.Collections;
using UnityEngine;

public class HandGunTest : WeaponBase
{
    bool _isFiring = false;
    float lastShotTime = -1;
    float ShotCD = 0.5f;

    public Vector3 weaponDir;

    void Update()
    {
        if(_isFiring && Time.time - lastShotTime >= ShotCD)
        {
            Debug.Log("shot!");
            WeaponUtils.spawnBulletPrefab(bulletPrefab, bulletRoot.position, weaponDir);
            lastShotTime = Time.time;
        }
    }
    public override void OnStartFire()
    {
        base.OnStartFire();

        _isFiring = true;
    }

    public override void OnEndFire()
    {
        base.OnEndFire();

        _isFiring = false;
    }

    public override void OnSetDir(Vector3 dir)
    {
        base.OnSetDir(dir);
        dir = (new Vector3(dir.x, dir.y, 0)).normalized;
        weaponDir = dir;
        var z = (dir.y > 0 ? 1:-1) * Vector3.Angle(new Vector3(1 , 0 , 0), new Vector3(dir.x , dir.y , 0));
        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
