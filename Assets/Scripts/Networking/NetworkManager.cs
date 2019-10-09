using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace RPS.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Serialized Variables
        [Tooltip("The prefab to use for representing the player")]
        [SerializeField] private List<Vector3> startPositions;
        [Tooltip("The prefab to use for representing the player")]
        [SerializeField] private List<Vector3> startRotations;
        [SerializeField] private GameManager gameManager;
        #endregion

        #region Public Variables
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (RPS.PlayerComp.PlayerSpawner.LocalPlayerInstance == null)
                {
                    SpawnPlayer();
                }
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            PhotonNetwork.LoadLevel("Game");
        }

        private void SpawnPlayer()
        {
            int roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, startPositions[roomPlayerCount - 1], Quaternion.identity, 0);
            spawnedPlayer.GetComponent<RPS.PlayerComp.PlayerSpawner>()?.Init(roomPlayerCount);
            PlayerController controllerComponent = spawnedPlayer.GetComponent<PlayerController>();
            if(controllerComponent) gameManager.AddPlayerController(controllerComponent);
        }
        #endregion
    }
}