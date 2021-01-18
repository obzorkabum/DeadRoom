using Photon.Pun;
using UnityEngine;

public class trapLifetime : MonoBehaviour
{
    public float LifeTime;
    public GameObject trap;

    void Update()
    {
            if (LifeTime <= 0)
            {
                GameManager.TrapCounterInRoom--;
                PhotonNetwork.Destroy(trap);
            }
               
            if (LifeTime >= 0)
                LifeTime -= Time.deltaTime;
    }
}
