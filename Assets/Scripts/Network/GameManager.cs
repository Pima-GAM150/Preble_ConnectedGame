using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (playerPrefab == null)
        {
            Debug.LogError("< Color = Red >< a > Missing </ a ></ Color > playerPrefab Reference.Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer.");
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, -10.39756f), Quaternion.identity, 0);
        }
        /*if (PlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level}");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEneteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }
}
