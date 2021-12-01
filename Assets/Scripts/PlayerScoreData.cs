using System;

[Serializable]
public class PlayerScoreData
{
    public PlayerScoreData(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
    
    public string playerName;
    public int score;
}
