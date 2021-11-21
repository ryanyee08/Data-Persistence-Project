using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// GameManager is used to track all data between scenes such as playername, highscore,etc
public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    public TMP_InputField playerNameInput;
    public static string currentPlayerName;

    private void Awake()
    {
        //Implement a Singleton pattern - will prevent more than one instance from occuring
        if (GameManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        GameManagerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Start the game
    public void StartGame()
    {
        currentPlayerName = playerNameInput.text;
        Debug.Log("Player is named: " + currentPlayerName);
        SceneManager.LoadScene("main");
    }

    public void ExitApplication()
    {

    }
}
