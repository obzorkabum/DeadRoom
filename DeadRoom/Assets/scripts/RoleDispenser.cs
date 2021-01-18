using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDispenser : MonoBehaviourPunCallbacks
{
    public static int iKiller=-1;
    public void Initialize()
    {
        Debug.Log("Initialize!");
        StartCoroutine(PickKiller());
    }

    private IEnumerator PickKiller()
    {
        GameObject[] players;
        List<int> PlayerIndex = new List<int>();
        int KillerNumber = 1;
        int tries = 0;

           do
           {
                players = GameObject.FindGameObjectsWithTag("Ghost");
                tries++;
                yield return new WaitForSeconds(0.25f);
           } while ((players.Length < PhotonNetwork.CurrentRoom.PlayerCount)&& tries<5);

        Debug.Log("Игроков - " + players.Length);

        for (int i = 0; i < players.Length; i++) { PlayerIndex.Add(i); }

        int PickKillerIndex;

         if(PhotonNetwork.CurrentRoom.PlayerCount>1)
         {
            for (int i = 0; i < players.Length-1; i++)
            {
                PhotonView pv = players[PlayerIndex[i]].GetComponent<PhotonView>();
                pv.RPC("SetPlayer", RpcTarget.Others);
            }
         }

        while (KillerNumber >= 1)
        {
            PickKillerIndex = PlayerIndex[Random.Range(0, PlayerIndex.Count)];
            PlayerIndex.Remove(PickKillerIndex); Debug.Log("Index =" + PickKillerIndex);
            PhotonView pv = players[PickKillerIndex].GetComponent<PhotonView>();
            pv.RPC("SetKiller", RpcTarget.MasterClient); Debug.Log("Name of Killer = " + pv.Owner.NickName);
            KillerNumber--;
        }
        yield return new WaitForSeconds(1f);

        Debug.Log("CLOCK Started!");
        if(PhotonNetwork.IsMasterClient)
         PhotonNetwork.Instantiate("Timer", new Vector3(0, 0), Quaternion.identity);

        GameManager.GmStarted = true;
    }
    
    private void Start()
    { StartCoroutine(WaitPlayers()); }

    private IEnumerator WaitPlayers()
    {
        do
        {
            Debug.Log("Waiting...");
            yield return new WaitForSeconds(1f);
        } while (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers);

        if (PhotonNetwork.IsMasterClient)
            Initialize();
        
        Debug.Log("All players in room!");
    }
}