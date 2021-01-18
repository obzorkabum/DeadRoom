using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Camera_Control : MonoBehaviourPunCallbacks
{
    private Camera m_Camera;
    private PhotonView photonView;
    public GameObject PlayerPrefab;

    void Start()
    {
            m_Camera = GetComponent<Camera>();
            photonView = PlayerPrefab.GetPhotonView();
            if (!photonView.IsMine)
            {
                m_Camera.enabled = false;
              
            }
        
    }

}