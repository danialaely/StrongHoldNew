using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;  // Add this line
using Photon.Realtime;

public class networking : MonoBehaviourPunCallbacks
{
    //public GameObject board;
    public Camera mycamera;

    public GameObject[] player1;
    public string P1tag = "Player1";
    
    public GameObject[] player2;
    public string P2tag = "Player2";

    public GameObject[] CBackP1;
    public string CBacktag1 = "CBack1";

    public GameObject[] CBackP2;
    public string CBacktag2 = "CBack2";

    //public GameObject turnObject;
    //public GameObject shuffleObject;

    public GameObject turnButton;
    public GameObject phaseButton;
    public GameObject phaseText;
    public GameObject turnBar1;
    public GameObject turnBar2;
    public GameObject popupCard1;
    public GameObject popupCard2;

    // Start is called before the first frame update
    void Start()
     {
        //turnObject.SetActive(false);
        //shuffleObject.SetActive(false);

        player1 = GameObject.FindGameObjectsWithTag(P1tag);
        player2 = GameObject.FindGameObjectsWithTag(P2tag);
        CBackP1 = GameObject.FindGameObjectsWithTag(CBacktag1);
        CBackP2 = GameObject.FindGameObjectsWithTag(CBacktag2);

        // Connect();
        if (PhotonNetwork.IsMasterClient)
         {
            // Handle master client logic
             mycamera.transform.position = new Vector3(515.5f, 261.4f, -10f);
             mycamera.transform.rotation = Quaternion.Euler(0,0,0);
            
            //turnButton.transform.position = new Vector3(444.1642f, 284.612f, -59.94021f);
            turnButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            phaseButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            phaseText.transform.rotation = Quaternion.Euler(0, 0, 0);
            turnBar1.transform.rotation = Quaternion.Euler(0, 0, 0);
            turnBar2.transform.rotation = Quaternion.Euler(0, 0, 0);
            popupCard1.transform.rotation = Quaternion.Euler(0, 0, 0);
            popupCard2.transform.rotation = Quaternion.Euler(0, 0, 0);

            foreach (GameObject master in player1) 
            {
                PhotonView photonView = master.GetComponent<PhotonView>();
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            }

            foreach (GameObject backP1 in CBackP1) 
            {
                backP1.SetActive(false);
            }

             //board.transform.position = new Vector3(540f, 127.3906f, 129.8881f);
             //board.transform.rotation = Quaternion.Euler(0, 0, 0);
         }
         else
         {
            // Handle non-master client logic
            mycamera.transform.position = new Vector3(515.5f, 261.4f, -10f);
             mycamera.transform.rotation = Quaternion.Euler(0, 0, 180);
            
            //turnButton.transform.position = new Vector3(444.1642f, 284.612f, -10f);
            turnButton.transform.rotation = Quaternion.Euler(0, 0, 180);
            phaseButton.transform.rotation = Quaternion.Euler(0, 0, 180);
            phaseText.transform.rotation = Quaternion.Euler(0, 0, 180);
            turnBar1.transform.rotation = Quaternion.Euler(0, 0, 180);
            turnBar2.transform.rotation = Quaternion.Euler(0, 0, 180);
            popupCard1.transform.rotation = Quaternion.Euler(0, 0, 180);
            popupCard2.transform.rotation = Quaternion.Euler(0, 0, 180);

            foreach (GameObject remote in player2)
            {
                PhotonView photonView = remote.GetComponent<PhotonView>();
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

                remote.transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            foreach (GameObject p1 in player1) 
            {
                p1.transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            foreach (GameObject backP2 in CBackP2)
            {
                backP2.SetActive(false);
            }

            //board.transform.position = new Vector3(500f, 405f, 129.8881f);
            //board.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

       
     }

   


   

    

  

    


    // Update is called once per frame
    /*   void Update()
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
    }*/
}