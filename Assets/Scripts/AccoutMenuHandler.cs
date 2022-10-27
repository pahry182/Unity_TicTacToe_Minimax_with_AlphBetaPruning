using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccoutMenuHandler : MonoBehaviour
{
    public Text usernameText;
    public Text tictactoe_score_easyText;
    public Text tictactoe_score_normalText;
    public Text tictactoe_score_hardText;
    private UserScore score = new UserScore();

    [System.Serializable]
    public class UserScore
    {
        public string username = "";
        public int tictactoe_score_easy = 0;
        public int tictactoe_score_normal = 0;
        public int tictactoe_score_hard = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        usernameText.text = AccountHandler.Instance.SessionUsername;
        StartCoroutine(TictactoeScoreGet());
        GameManager.Instance.PlayBgm("MenuBGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSFX()
    {
        GameManager.Instance.PlaySfx("ButtonSFX");
    }

    private IEnumerator TictactoeScoreGet()
    {
        score.username = AccountHandler.Instance.SessionUsername;
        UnityWebRequest request = UnityWebRequest.Get("https://cp-api-unej.herokuapp.com/tictactoe/" + score.username);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            print("Error:" + request.error);
        }
        else
        {
            score = JsonUtility.FromJson<UserScore>(request.downloadHandler.text);
            tictactoe_score_easyText.text = "Tictactoe Win Easy: " + score.tictactoe_score_easy.ToString();
            tictactoe_score_normalText.text = "Tictactoe Win Normal: " + score.tictactoe_score_normal.ToString();
            tictactoe_score_hardText.text = "Tictactoe Win Hard: " + score.tictactoe_score_hard.ToString();
        }

    }
}
