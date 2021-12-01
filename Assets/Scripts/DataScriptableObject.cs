using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "persistence", order = 100)]
public class DataScriptableObject : ScriptableObject
{
    public string playerName;
    public int playerScore;
}
