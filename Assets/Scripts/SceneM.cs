using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneM : MonoBehaviourPunCallbacks
{
    public DropDown dd;
    public MenuToggleManager menuToggleManager;

    public GameObject LobbyPanel;
    public GameObject ConnectingPanel;
    public GameObject CreateRoomPanel;

    public TMP_InputField roomNameText;

    public void LoadPlayScene()
    {
        if (dd.value == 0) 
        {
            //SceneManager.LoadScene("SampleScene");
            PhotonNetwork.LocalPlayer.NickName = menuToggleManager.GetUserName();
            PhotonNetwork.ConnectUsingSettings();
            ActivateMyPanel(ConnectingPanel.name);
        }
        if (dd.value == 1) 
        {
            SceneManager.LoadScene("AI");
        }

    }

    public void OnClickRoomCreate() 
    {
        string roomName = roomNameText.text;
        if (string.IsNullOrEmpty(roomName)) 
        {
            roomName = roomName+Random.Range(0, 1000);
        }
    }

    private void Update()
    {
        Debug.Log("Network state:"+PhotonNetwork.NetworkClientState);
    }

    public void ActivateMyPanel(string panelName)
    {
        LobbyPanel.SetActive(panelName.Equals(LobbyPanel.name));
        ConnectingPanel.SetActive(panelName.Equals(ConnectingPanel.name));
        CreateRoomPanel.SetActive(panelName.Equals(CreateRoomPanel.name));
    }

    #region Photon_Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+" is connected to photon.");
        ActivateMyPanel(LobbyPanel.name);
    }

    #endregion
}
