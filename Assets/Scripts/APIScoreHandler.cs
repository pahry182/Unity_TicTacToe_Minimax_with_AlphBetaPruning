using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserScore
{
    public string username = "";
    public int tictactoe_score_easy = 0;
    public int tictactoe_score_normal = 0;
    public int tictactoe_score_hard = 0;
}

public class APIScoreHandler : MonoBehaviour
{
    public static APIScoreHandler Instance { get; private set; }
    private const string URL = "https://cp-api-unej.herokuapp.com/tictactoe/";
    private UserScore score = new();    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonPutScoreTictactoe()
    {
       
    }

    public IEnumerator TictactoeScorePut()
    {
        //string _username = "pahry182";
        score.username = AccountHandler.Instance.SessionUsername;
        UnityWebRequest request = UnityWebRequest.Get(URL + score.username);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            print("Error:" + request.error);
        }
        else
        {
            UserScore _score = JsonUtility.FromJson<UserScore>(request.downloadHandler.text);

            if (string.IsNullOrEmpty(_score.username))
            {
                string data1 = JsonUtility.ToJson(score);
                request = new UnityWebRequest(URL + score.username, UnityWebRequest.kHttpVerbPUT);
                request.SetRequestHeader("Content-Type", "application/json");
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data1));
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                _score = JsonUtility.FromJson<UserScore>(request.downloadHandler.text);
            }

            switch (BoardAdvanced.difficulty)
            {
                case AIDifficulty.EASY:
                    _score.tictactoe_score_easy++;
                    break;
                case AIDifficulty.NORMAL:
                    _score.tictactoe_score_normal++;
                    break;
                case AIDifficulty.HARD:
                    _score.tictactoe_score_hard++;
                    break;
                default:
                    break;
            }

            string data = JsonUtility.ToJson(_score);
            request = new UnityWebRequest(URL + _score.username, UnityWebRequest.kHttpVerbPUT);
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    
}
