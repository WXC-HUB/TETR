using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponPrefab
{
    public int ID;
    public GameObject prefab;
}

[CreateAssetMenu(menuName = "MySubMenue/Create WeaponSetting")]
public class WeaponSetting : ScriptableObject
{
    public List<WeaponPrefab> weaponPrefabs;
}
