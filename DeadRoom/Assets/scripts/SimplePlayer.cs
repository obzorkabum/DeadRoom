using UnityEngine;
using Photon.Pun;
using TMPro;

public class SimplePlayer : MonoBehaviourPun
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject killer;
    [SerializeField] private TextMeshProUGUI PlayersCount;
    Vector3 pos;

    private void Start()
    {
        pos = new Vector3(Random.Range(3, 8), Random.Range(0, 1), 0);
    }

    [PunRPC]
    private void SetKiller()
    {
        Debug.Log("You are killer!");
        RoleDispenser.iKiller = 1;
        PhotonNetwork.Instantiate(killer.name, pos, Quaternion.identity);
        GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        for(int i=0;i<Ghosts.Length;i++)
        Destroy(Ghosts[i]);
    }

    [PunRPC]
    private void SetPlayer()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("You are player!");
            RoleDispenser.iKiller = 0;
            PhotonNetwork.Instantiate(player.name, pos, Quaternion.identity);
            GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            for (int i = 0; i < Ghosts.Length; i++)
                Destroy(Ghosts[i]);
        }
    }

    private void Update()
    {
        int players = PhotonNetwork.CurrentRoom.PlayerCount;
        if (PlayersCount != null)
            PlayersCount.SetText("Кол-во игроков: " + players + " из " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }
}
