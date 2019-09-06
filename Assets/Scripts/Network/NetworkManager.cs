using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;

public class NetworkManager : MonoBehaviour {

    [SerializeField] private string m_serverIP = "127.0.0.1";
    [SerializeField] private int m_portNumber = 9933;

    [SerializeField] private GameManager m_gameManager;

    private const string m_zoneName = "TicTacToe";
    private const string m_roomName = "Lobby";

    private SmartFox m_smartFox;

    private int m_previousMessageSent = -1;

    private void Awake() {
        m_smartFox = new SmartFox();
        m_smartFox.ThreadSafeMode = true;

        m_smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        m_smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
        m_smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        m_smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
        m_smartFox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnJoinRoomError);
        m_smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);

        m_smartFox.Connect(m_serverIP, m_portNumber);
    }

    private void FixedUpdate() {
        if (m_smartFox != null) {
            m_smartFox.ProcessEvents();
        }
    }

    public void SendInput(int buttonIndex) {
        m_smartFox.Send(new PublicMessageRequest(buttonIndex.ToString()));
        Debug.Log("Message sent");
        m_previousMessageSent = buttonIndex;
    }

    private void OnConnection(BaseEvent e) {
        if ((bool)e.Params["success"]) {
            m_smartFox.Send(new LoginRequest("", "", m_zoneName));
            Debug.Log("Successfully connected");
        } else {
            Debug.Log("Failed to connect");
        }
    }

    private void OnLogin(BaseEvent e) {
        Debug.Log("LoginSuccessful");
        m_smartFox.Send(new JoinRoomRequest(m_roomName));
    }

    private void OnLoginError(BaseEvent e) {
        Debug.Log("Login Failed!");
    }

    private void OnJoinRoom(BaseEvent e) {
        Debug.Log("Join room");
        Room room = (Room)e.Params["room"];
        if (room.UserCount == 1) {
            Debug.Log("<color=red>PLAYER ONE</color>");
            m_gameManager.SetIsPlayerOne(true);
        } else {
            m_gameManager.SetIsPlayerOne(false);
        }
    }

    private void OnJoinRoomError(BaseEvent e) {
        Debug.Log("Failed to join room: " + e.Params["errorMessage"]);
    }

    private void OnApplicationQuit() {
        if (m_smartFox != null) {
            if (m_smartFox.IsConnected) {
                m_smartFox.Disconnect();
            }
        }
    }

    private void OnPublicMessage(BaseEvent e) {
        Debug.Log("Message received: " + e.Params["message"]);

        if (int.Parse(e.Params["message"].ToString()) != m_previousMessageSent) {
            m_gameManager.UserInput(int.Parse(e.Params["message"].ToString()));
        }
    }
}
