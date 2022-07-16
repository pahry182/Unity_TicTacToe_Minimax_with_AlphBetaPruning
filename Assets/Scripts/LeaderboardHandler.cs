using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum Difficulty { Easy, Normal, Hard}

public class LeaderboardHandler : MonoBehaviour
{
    private const string URL = "https://cp-api-unej.herokuapp.com/tictactoe/all";

    public GameObject leaderboardWindow;
    public GameObject entryPrefab;
    public Transform entryParent;
    public Text leaderboardTitle;
    public LeaderboardEntryTTTScore currentSessionEntry;

    [HideInInspector] public List<UserScore> userScoresData;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetScoreTictactoe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GetScoreTictactoe()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            print("Error:" + request.error);
        }
        else
        {
            userScoresData = JsonConvert.DeserializeObject<List<UserScore>>(request.downloadHandler.text);
        }
    }

    private int SortByScoreEasy(UserScore p1, UserScore p2)
    {
        return p2.tictactoe_score_easy.CompareTo(p1.tictactoe_score_easy);
    }

    private int SortByScoreNormal(UserScore p1, UserScore p2)
    {
        return p2.tictactoe_score_normal.CompareTo(p1.tictactoe_score_normal);
    }

    private int SortByScoreHard(UserScore p1, UserScore p2)
    {
        return p2.tictactoe_score_hard.CompareTo(p1.tictactoe_score_hard);
    }

    private List<LeaderboardEntryTTTScore> SetupEntry()
    {
        leaderboardWindow.SetActive(true);
        List<LeaderboardEntryTTTScore> entryList = new();

        for (int i = 0; i < userScoresData.Count; i++)
        {
            entryList.Add(Instantiate(entryPrefab, entryParent).GetComponent<LeaderboardEntryTTTScore>());
        }

        return entryList;
    }

    private void DataEntry(Difficulty difficulty, List<LeaderboardEntryTTTScore> entryList)
    {
        leaderboardTitle.text = "Leaderboard Tictactoe " + difficulty;
        currentSessionEntry.username.text = AccountHandler.Instance.SessionUsername;

        switch (difficulty)
        {
            case Difficulty.Easy:
                currentSessionEntry.score.text = userScoresData.Find((x) => x.username == AccountHandler.Instance.SessionUsername).tictactoe_score_easy.ToString();
                break;
            case Difficulty.Normal:
                currentSessionEntry.score.text = userScoresData.Find((x) => x.username == AccountHandler.Instance.SessionUsername).tictactoe_score_normal.ToString();
                break;
            case Difficulty.Hard:
                currentSessionEntry.score.text = userScoresData.Find((x) => x.username == AccountHandler.Instance.SessionUsername).tictactoe_score_hard.ToString();
                break;
            default:
                break;
        }

        for (int i = 0; i < userScoresData.Count; i++)
        {
            entryList[i].username.text = i+1 + ". " + userScoresData[i].username;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    entryList[i].score.text = userScoresData[i].tictactoe_score_easy.ToString();
                    break;
                case Difficulty.Normal:
                    entryList[i].score.text = userScoresData[i].tictactoe_score_normal.ToString();
                    break;
                case Difficulty.Hard:
                    entryList[i].score.text = userScoresData[i].tictactoe_score_hard.ToString();
                    break;
                default:
                    break;
            }
        }
    }

    public void LeaderboardEasyButton()
    {
        List<LeaderboardEntryTTTScore> entryList = SetupEntry();
        userScoresData.Sort(SortByScoreEasy);
        DataEntry(Difficulty.Easy, entryList);
    }

    public void LeaderboardNormalButton()
    {
        List<LeaderboardEntryTTTScore> entryList = SetupEntry();
        userScoresData.Sort(SortByScoreNormal);
        DataEntry(Difficulty.Normal, entryList);
    }

    public void LeaderboardHardButton()
    {
        List<LeaderboardEntryTTTScore> entryList = SetupEntry();
        userScoresData.Sort(SortByScoreHard);
        DataEntry(Difficulty.Hard, entryList);
    }

    public void CloseLeaderboard()
    {
        foreach (Transform child in entryParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
