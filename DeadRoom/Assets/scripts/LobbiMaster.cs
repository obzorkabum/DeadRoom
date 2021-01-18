using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbiMaster : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField RoomName;
    private Text LogText;

    void Start()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(1000, 9999);
        Log("Player's name is set to" + PhotonNetwork.NickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "2";
        PhotonNetwork.ConnectUsingSettings();
    }
    void Update()
    {
            if (Input.GetKey("escape"))  // если нажата клавиша Esc (Escape)
             Application.Quit();    // закрыть приложение            
    }
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(RoomName.text)==false)
        PhotonNetwork.CreateRoom(RoomName.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 },null);
    }

    public void JoinRoom()
    { PhotonNetwork.JoinRandomRoom();}


    public void ExitGame()
    { Application.Quit();}
       
    public override void OnJoinedRoom()
    {
        Log("Joined on the room: "+ PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnConnectedToMaster()
    {Log("Connected to Master");}

    private void Log(string message)
    { Debug.Log(message); }
}