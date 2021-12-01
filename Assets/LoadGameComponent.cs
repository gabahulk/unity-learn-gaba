using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameComponent : MonoBehaviour
{
    public DataScriptableObject dataScriptableObject;
    public InputField inputField;

    private void Awake()
    {
        dataScriptableObject.playerName = "NoNaMe";
    }

    public void AddName()
    {
        dataScriptableObject.playerName = inputField.text;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex: 1, LoadSceneMode.Single);
    }
}
