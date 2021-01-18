using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    Vector3 pos;
    [SerializeField] GameObject ghost;
    public static int TrapCounterInRoom;
    public static bool isHost = false;
    public static bool HostInRoom = true;
    public static bool GmStarted;



    void Start()
    {
        pos = new Vector3(Random.Range(3, 8), Random.Range(0, 1), -10);
        PhotonNetwork.Instantiate(ghost.name, pos, Quaternion.identity);
        PhotonPeer.RegisterType(typeof(Vector2Int), 224, SerializeVector2Int, DeserializeVector2Int);

        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.isHost = true;
        }      
    }

    public static void Leave()
    {
        if(GameManager.isHost==true && GameManager.GmStarted==true)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i=0;i< players.Length;i++)
            {
                PhotonView pv = players[i].GetComponent<PhotonView>();
                pv.RPC("KillerLeave", RpcTarget.All);
            }
           
        }
        
        
        RoleDispenser.iKiller = -1;
        GameManager.GmStarted = false;
        GameManager.isHost = false;
        KillerController.CanIKill = false;
        PhotonNetwork.LeaveRoom();
    }

    public void Killed()
    {
        PhotonNetwork.LeaveRoom();
        RoleDispenser.iKiller = -1;
        GameManager.GmStarted = false;
    }



    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }
    //--------------------------------------CUSTOM METODS SERIALIZATION--------------------------------------------------------
    public static object DeserializeVector2Int(byte[] data)
    {
        Vector2Int result = new Vector2Int();
        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);

        return result;
    }

    public static byte[] SerializeVector2Int(object obj)
    {
        Vector2Int vector = (Vector2Int)obj;
        byte[] result = new byte[8];

        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);

        return result;
    }
}
