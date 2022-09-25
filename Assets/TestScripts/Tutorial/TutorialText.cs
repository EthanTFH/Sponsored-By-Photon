using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using net.EthanTFH.BTSGameJam;

public class TutorialText : MonoBehaviour
{

    public GameObject parentPanel;
    public GameObject panelTwoDimension;
    public GameObject panelThreeDimension;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat("IsMasterClient = {0}\r\nSmallPlayer = {1}", PhotonNetwork.IsMasterClient, Camera.main.GetComponent<CameraModeOne>().SmallPlayer);
        if(PhotonNetwork.IsMasterClient && Camera.main.GetComponent<CameraModeOne>().SmallPlayer == CameraModeOne.ClientType.MasterClient)
        {
            panelTwoDimension.SetActive(true);
        }else if (!PhotonNetwork.IsMasterClient && Camera.main.GetComponent<CameraModeOne>().SmallPlayer == CameraModeOne.ClientType.NormalClient)
        {
            panelTwoDimension.SetActive(true);
        }
        else
        {
            panelThreeDimension.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClose()
    {
        Destroy(parentPanel);
    }
}
