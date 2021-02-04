using UnityEditor;

public class DialogueSetDataAsset
{
    [MenuItem("Assets/Create/ScriptableObject/DialogueSet")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<DialogueSetData>();
    }
}
