using Photon.Pun;
using UnityEngine;

public class PreTrap : MonoBehaviour
{
    [SerializeField] private float PreTrapLifeTime;
    public GameObject NewPreTrap;


    void Update()
    {
        if(PreTrapLifeTime>=0)
        PreTrapLifeTime -= Time.deltaTime;

        if (PreTrapLifeTime <= 0)
        {
            PhotonNetwork.Instantiate("trap", NewPreTrap.transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(NewPreTrap);
        }
    }
}
