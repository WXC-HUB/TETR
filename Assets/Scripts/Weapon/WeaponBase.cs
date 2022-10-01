using System.Collections;
using UnityEngine;


public class WeaponBase : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletRoot;
    public virtual void OnStartFire()
    {

    }

    public virtual void OnEndFire()
    {

    }

    public virtual void OnUnEquip()
    {

    }

    public virtual void OnSetDir(Vector3 dir) 
    { 
    
    }
}
