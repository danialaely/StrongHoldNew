using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class SceneM : MonoBehaviourPunCallbacks
{
    public DropDown dd;
    public MenuToggleManager menuToggleManager;

    public GameObject LobbyPanel;
    public GameObject ConnectingPanel;
    public GameObject CreateRoomPanel;
    public GameObject RoomListPanel;

    public TMP_InputField roomNameText;

    private Dictionary<string, RoomInfo> roomListData;

    public GameObject roomListPrefab;
    public GameObject roomListParent;

    private Dictionary<string, GameObject> roomListGameobject;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;

    private void Start()
    {
        roomListData = new Dictionary<string, RoomInfo>();
        roomListGameobject = new Dictionary<string, GameObject>();
    }
    private void Update()
    {
        Debug.Log("Network state:"+PhotonNetwork.NetworkClientState);
       // Debug.Log("Room List Prefab:"+roomListPrefab+" "+" RoomListParent:"+roomListParent);
    }

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
        if (!string.IsNullOrEmpty(roomName)) 
        {
            roomName = roomName+Random.Range(0, 1000);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)2;
        PhotonNetwork.CreateRoom(roomName,roomOptions);
    }

    public void OnCancelClick() 
    {
        ActivateMyPanel(LobbyPanel.name);
    }

    public void RoomListBtnClicked() 
    {
        if (!PhotonNetwork.InLobby) 
        {
            PhotonNetwork.JoinLobby();
        }
        ActivateMyPanel(RoomListPanel.name);
    }

    public void BackFromRoomList() 
    {
        if (PhotonNetwork.InLobby) 
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivateMyPanel(LobbyPanel.name);
    }

    public void ActivateMyPanel(string panelName)
    {
        LobbyPanel.SetActive(panelName.Equals(LobbyPanel.name));
        ConnectingPanel.SetActive(panelName.Equals(ConnectingPanel.name));
        CreateRoomPanel.SetActive(panelName.Equals(CreateRoomPanel.name));
        RoomListPanel.SetActive(panelName.Equals(RoomListPanel.name));
        InsideRoomPanel.SetActive(panelName.Equals(InsideRoomPanel.name));
    }

    public void RoomJoinFromList(string roomName) 
    {
        if (PhotonNetwork.InLobby) 
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void ClearRoomList() 
    {
        if (roomListGameobject.Count > 0) 
        {
            foreach (var v in roomListGameobject.Values) 
            {
                Destroy(v);
            }
            roomListGameobject.Clear();
        }
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

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name+" is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+" joined the room");
        ActivateMyPanel(InsideRoomPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Clear List
        ClearRoomList();
        Debug.Log("OnRoomList");

        foreach(RoomInfo rooms in roomList) 
        {
            Debug.Log("Room Name:"+rooms.Name);
            if (!rooms.IsOpen || !rooms.IsVisible || rooms.RemovedFromList)
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    roomListData.Remove(rooms.Name);
                }
            }
            else 
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    //Update List
                    roomListData[rooms.Name] = rooms;
                }
                else 
                {
                    roomListData.Add(rooms.Name,rooms);
                }
            }
        }

        //Generate List Item
        foreach (RoomInfo roomItem in roomListData.Values) 
        {
            GameObject roomListItemObject = Instantiate(roomListPrefab);
            roomListItemObject.transform.SetParent(roomListParent.transform);
            roomListItemObject.transform.localScale = Vector3.one;

            //room name  player Number  Button join room
            roomListItemObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = roomItem.Name;
            roomListItemObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = roomItem.PlayerCount+"/"+roomItem.MaxPlayers;
            roomListItemObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(()=> RoomJoinFromList(roomItem.Name));
            roomListGameobject.Add(roomItem.Name, roomListItemObject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomList();
        roomListData.Clear();
    }

    #endregion
}
