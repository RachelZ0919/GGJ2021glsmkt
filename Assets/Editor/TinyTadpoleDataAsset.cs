using UnityEditor;

public class TinyTadpoleDataAsset
{
    [MenuItem("Assets/Create/ScriptableObject/TinyTadpoleData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<TinyTadpoleData>();
    }
}
