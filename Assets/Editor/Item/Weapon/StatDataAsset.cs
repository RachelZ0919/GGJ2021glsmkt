using UnityEngine;
using UnityEditor;

public class StatDataAsset
{
    [MenuItem("Assets/Create/StatData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<StatData>();
    }
}
