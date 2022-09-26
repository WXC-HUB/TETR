using System.Collections;
using UnityEngine;

public static class WeaponUtils
{
    public static GameObject spawnBulletPrefab(GameObject bulletPrefab , Vector3 pos , Vector3 dir)
    {
        GameObject newObj = GameObject.Instantiate(bulletPrefab);
        newObj.transform.position = pos;
        BulletBase bullet = newObj.GetComponent<BulletBase>();
        if(bullet == null)
        {
            GameObject.Destroy(newObj);
            return null;
        }

        bullet.InitBullet(dir);
        return newObj;
    }
}