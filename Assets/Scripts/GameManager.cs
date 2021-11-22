using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System;

// GameManager is used to track all data between scenes such as playername, highscore,etc
public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    public TMP_InputField playerNameInput;
    public static string currentPlayerName;

    public List<PreviousPlayerScore> HighScoreList = new List<PreviousPlayerScore>();

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

        // Get the Previous high score records from save file for display
        LoadPreviousScores();
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

    [System.Serializable]
    public class PreviousPlayerScore : IComparable<PreviousPlayerScore>
    {
        public string PreviousPlayerName;
        public int PreviousPlayerScoreValue;

        public int CompareTo(PreviousPlayerScore playerscore)
        {
            if (playerscore == null)
            {
                return 1;
            }
            else
            {
                return this.PreviousPlayerScoreValue.CompareTo(playerscore.PreviousPlayerScoreValue);
            }
        }
    }

    [System.Serializable]
    public class ListWrapper
    {
        public List<PreviousPlayerScore> wrappedList;
    }


    public static void SaveScore(int playerscore)
    {

        // Add the the player name and score values to the list (3 -> 4)
        GameManagerInstance.HighScoreList.Add(new PreviousPlayerScore() { PreviousPlayerName = currentPlayerName, PreviousPlayerScoreValue = playerscore });
       
        // Sort the list by the playerscore value, then reverse to place items in descending order
        GameManagerInstance.HighScoreList.Sort();
        GameManagerInstance.HighScoreList.Reverse();

        // DEBUG - Print the Sorted List to Console
        Debug.Log("After Sorting Score: ");
        foreach (PreviousPlayerScore aPlayerScore in GameManagerInstance.HighScoreList)
        {
            Debug.Log("High Scores- Name: " + aPlayerScore.PreviousPlayerName + " Score: " +aPlayerScore.PreviousPlayerScoreValue);
        }

        // Save to JSON
        //First Wrap up the list for some reason
        var ListWrapper = new ListWrapper();
        ListWrapper.wrappedList = GameManagerInstance.HighScoreList;

        string json = JsonUtility.ToJson(ListWrapper);

        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    public void LoadPreviousScores()
    {
        Debug.Log("Starting Load For Previous Scores");
        // This only needs to be run when the game is first started so when gamemanager is awakened
        // Basically grab the JSON and unpack it to the List
        string path = Application.persistentDataPath + "/highscores.json";

        // Checks to see if a file exists from the filepath
        // If the file exists then it will read the file, unpack the json to the list
        if (File.Exists(path))
        {
            Debug.Log("A Save File was Found");
            
            string json = File.ReadAllText(path);

            var ListWrapper = new ListWrapper();

            ListWrapper = JsonUtility.FromJson<ListWrapper>(json);

            GameManagerInstance.HighScoreList = ListWrapper.wrappedList;

            // DEBUG - Print the Sorted List to Console
            Debug.Log("After Sorting Score: ");
            foreach (PreviousPlayerScore aPlayerScore in GameManagerInstance.HighScoreList)
            {
                Debug.Log("High Scores- Name: " + aPlayerScore.PreviousPlayerName + " Score: " + aPlayerScore.PreviousPlayerScoreValue);
            }

        }
    }
}
