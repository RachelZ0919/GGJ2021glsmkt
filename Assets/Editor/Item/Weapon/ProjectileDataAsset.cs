using UnityEngine;
using UnityEditor;

public class ProjectileDataAsset
{
    [MenuItem("Assets/Create/ProjectileData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ProjectileData>();
    }
}
