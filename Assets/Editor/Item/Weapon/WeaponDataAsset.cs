using UnityEngine;
using UnityEditor;

public class WeaponDataAsset
{
    [MenuItem("Assets/Create/WeaponData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WeaponData>();
    }
}
