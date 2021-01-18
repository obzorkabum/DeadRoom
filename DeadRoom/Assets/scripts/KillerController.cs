using Photon.Pun;
using UnityEngine;

public class KillerController : MonoBehaviourPunCallbacks
{
    public static bool CanIKill= false;
    public static bool SimpleTrap= false;
    
    public Camera cam;
    public GameObject SimpleTrapPrefab;
    //------------------------------------------
    private Vector2 trapPos;
    //------------------------------------------
    private PhotonView photonView;
    [SerializeField] private GameObject KillerControl;
    [SerializeField] private  int TrapOnScene=10;
    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        GameManager.TrapCounterInRoom = 0;
        if (RoleDispenser.iKiller == 1)
            Instantiate(KillerControl);
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (SimpleTrap)
            {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (GameManager.TrapCounterInRoom <= TrapOnScene)
                        {
                            float posx = Mathf.Floor(cam.ScreenToWorldPoint(Input.mousePosition).x);
                            float posy = Mathf.Floor(cam.ScreenToWorldPoint(Input.mousePosition).y);

                            int Intposx = (int)posx;
                            int Intposy = (int)posy;

                            trapPos.x = Intposx;
                            trapPos.y = Intposy;

                        //Debug.Log("X = " + Intposx + ", Y= " + Intposy);
                        //Debug.Log("Access = "+ trapPlace.SimpleTrapAccessiblePlace[Intposx, Intposy]);
                        if (trapPlace.SimpleTrapAccessiblePlace[Intposx, Intposy]==true)
                            {
                                GameManager.TrapCounterInRoom++;
                                Debug.Log("traps on scene = " + GameManager.TrapCounterInRoom);
                                PhotonNetwork.Instantiate("PreTrap", new Vector3(trapPos.x, trapPos.y), Quaternion.identity);
                            }
                            
                        }
                    }
            }
        }
    }

    public void KillMode()
    {
        CanIKill = !CanIKill;
        if (!CanIKill) LockTraps();
    }

    public void setSimpleTrap()
    {
        if(CanIKill)
        SimpleTrap = !SimpleTrap;
    }

    private void LockTraps()
    {SimpleTrap = false;}

}