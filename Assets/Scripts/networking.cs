using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networking : MonoBehaviourPunCallbacks
{
    //public GameObject board;
    public Camera mycamera;

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(mycamera.ScreenPointToRay(Input.mousePosition));
       //mycamera.ScreenPointToRay(Input.mousePosition);
    }

    void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");
        JoinRandomRoom();
    }

    void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join, creating a new room.");
        CreateRoom();
    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined a room");

        // Check if the local player is the master client
        if (PhotonNetwork.IsMasterClient)
        {
            // Handle master client logic

            //board.transform.position = new Vector3(540f, 127.3906f, 129.8881f);
            //board.transform.rotation = Quaternion.Euler(0, 0, 0);
            mycamera.transform.position = new Vector3(515.5f, 261.4f, -10f);
            mycamera.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            // Handle non-master client logic

            //board.transform.position = new Vector3(500f, 405f, 129.8881f);
            //board.transform.rotation = Quaternion.Euler(0, 0, 180);
            mycamera.transform.position = new Vector3(515.5f, 261.4f, -10f);
            mycamera.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}