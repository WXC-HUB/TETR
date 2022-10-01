using System.Collections;
using UnityEngine;

public class GameCharacterBase : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    WeaponBase nowWeapon;

    public Transform weaponRoot;

    public int HP = 10;


    float _moveSpeed = 15f;
    // Use this for initialization
    public virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public bool HaveWeapon()
    {
        return nowWeapon != null;
    }

    public void EquipWeaponByID(int ID)
    {
        var z = LevelManager.Instance.weaponSetting.weaponPrefabs.Find((WeaponPrefab w) => w.ID == ID);
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

    public virtual void Move( Vector3 direction , float speedscale = 1.0f)
    {
        if (_rigidbody2D == null) return;
        //_rigidbody2D.velocity = direction.normalized * _moveSpeed * speedscale;

        _rigidbody2D.MovePosition(transform.position + direction.normalized * _moveSpeed * speedscale * Time.deltaTime);
    }

    public virtual void OnTakeDamage(int dmg)
    {
        HP -= dmg;
    }

}
