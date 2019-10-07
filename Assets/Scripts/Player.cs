using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace RPS.PlayerComp
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerInitializer initializer;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject healthPedestal;
        [SerializeField] private float spawnOffset = 1.5f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        private int playerScore = 0;
        private int pointsToScore = 5;

        public void Init()
        {
            Vector3 startPos = transform.position + new Vector3(2, 0, 0);
            if (pointsToScore % 2 == 0)
            {
                startPos = new Vector3(startPos.x, startPos.y, startPos.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);
            }
            else
            {
                startPos = new Vector3(startPos.x, startPos.y, startPos.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);
            }
            Vector3 spawnPosition = startPos;

            for (int i = 0; i < pointsToScore; i++)
            {
                int photonViewID = PhotonNetwork.AllocateViewID(false);
                photonView.RPC("SpawnHealthRepresentation", RpcTarget.AllBuffered, spawnPosition, 5, photonViewID);
                spawnPosition += new Vector3(0, 0, 1.5f);
            }
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                Player.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        [PunRPC]
        private void SpawnHealthRepresentation(Vector3 spawnPosition, int pointsToScore,int id)
        {
            if (healthPedestal != null)
            {
                    GameObject spawnedObj = Instantiate(healthPedestal, spawnPosition, Quaternion.identity, transform);
                    PhotonView view = spawnedObj.GetComponent<PhotonView>();
                    view.ViewID = id;
            }
        }
    }
}

