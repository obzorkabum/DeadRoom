using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float TimeStart;
    public Text timeText;
    public static bool KillerWin = false;

    void Start()
    {
        timeText.text = TimeStart.ToString();
        StartCoroutine(Referee());
    }
    void Update()
    {
        TimeStart -= Time.deltaTime;
        timeText.text = Mathf.Round(TimeStart).ToString();

        if (!GameManager.HostInRoom || PhotonNetwork.CurrentRoom.PlayerCount==1)
        {
            TimeStart = 0;
        }
    }

    private IEnumerator Referee()
    {
        do
        {
            yield return new WaitForSeconds(1f);
        } while (TimeStart > 0);


        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && GameManager.HostInRoom)
        {
            Timer.KillerWin = true;
                StartCoroutine(ShowEndGame());
        }
        else {
            Timer.KillerWin = false;
                StartCoroutine(ShowEndGame());
        }

    }

    private IEnumerator ShowEndGame()
    {
            PhotonNetwork.Instantiate("WinOrLost", new Vector3(0, 0), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        GameObject[] texts = GameObject.FindGameObjectsWithTag("EndGame");

        for(int i=0;i< texts.Length;i++)
        {
            if(Timer.KillerWin)
            {
             Text endGame=texts[i].GetComponent<Text>();
                endGame.text= "Killer wins!";
            }
            else
            {
                Text endGame = texts[i].GetComponent<Text>();
                endGame.text = "Player wins!";
            }
        }

            yield return new WaitForSeconds(5f);
            GameManager.Leave();
        
        
    }
}
