using System.Collections;
using UnityEngine;

public class GameCharacterBase : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    WeaponBase nowWeapon;

    public Transform weaponRoot;

    public int HP = 10;


    float _moveSpeed = 3f;
    // Use this for initialization
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Debug.Log(_rigidbody2D);
    }

    public void EquipWeaponByID(int ID)
    {
        var z = LevelManager.Instance.weaponSetting.weaponPrefabs.Find((WeaponPrefab w) => w.ID == ID);
        Debug.Log(z);
        GameObject wPrefab = LevelManager.Instance.weaponSetting.weaponPrefabs.Find((WeaponPrefab w) => w.ID == ID).prefab;
        GameObject newObj = Instantiate(wPrefab, weaponRoot);

        if (nowWeapon != null)
        {
            nowWeapon.OnUnEquip();
        }

        nowWeapon = newObj.GetComponent<WeaponBase>();

    }

    public virtual void UpdateWeaponDir(Vector3 dir)
    {
        if (nowWeapon == null) return;
        nowWeapon.OnSetDir(dir);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnStartFire()
    {
        nowWeapon.OnStartFire();
    }

    public virtual void OnEndFire()
    {
        nowWeapon.OnEndFire();
    }

    public virtual void Move( Vector3 direction)
    {
        if (_rigidbody2D == null) return;
        _rigidbody2D.velocity = direction.normalized * _moveSpeed;
    }

    public virtual void OnTakeDamage(int dmg)
    {
        HP -= dmg;
    }

}
