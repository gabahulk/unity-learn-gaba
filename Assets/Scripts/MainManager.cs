using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class MainManager : MonoBehaviour
{
    public DataScriptableObject dataScriptableObject;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private SaveData data;
    private string playerName;
    private int bestScore;

    
    // Start is called before the first frame update
    void Start()
    {
        playerName = dataScriptableObject.playerName;
        LoadBestScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > bestScore)
        {
            BestScoreText.text = $"Best Score: {playerName} : {m_Points}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveBestScore();
    }

    private void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            var bestPlayer = data.Players.OrderByDescending(o => o.score).ToList()[0];
            string bestPlayerName = bestPlayer.playerName;
            bestScore = bestPlayer.score;
            BestScoreText.text = $"Best Score: {bestPlayerName} : {bestScore}";
        }
        else
        {
            BestScoreText.text = "";
            data = new SaveData
            {
                Players = new List<PlayerScoreData>()
            };
        }
    }

    private void SaveBestScore()
    {
        data ??= new SaveData();
        data.Players ??= new List<PlayerScoreData>();
        int index = 0;
        bool hasFound = false;
        foreach (var player in data.Players)
        {
            if (player.playerName == playerName)
            {
                if (player.score < m_Points)
                {
                    data.Players[index].score = m_Points;
                }
                hasFound = true;
                break;
            }
            index++;
        }

        if (!hasFound)
        {
            data.Players.Add(new PlayerScoreData(dataScriptableObject.playerName, m_Points));
        }
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
