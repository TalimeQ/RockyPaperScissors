﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace RPS.PlayerComp
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject healthPedestal;
        [SerializeField] private Vector3 posOffset = new Vector3(2, 0, 0); 
        [SerializeField] private float spawnOffset = 1.5f;

        public static GameObject LocalPlayerInstance;

        private int playerScore = 0;
        private int pointsToScore = 5;

        public void Init(int roomPlayerCount)
        {
            SpawnPointsRepresentation();
            RPCRotateIfEvent(roomPlayerCount);
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                Player.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }


        private void SpawnPointsRepresentation()
        {
            Vector3 startPos = transform.position + posOffset;
            Vector3 spawnPosition = GetStartPositionZ(startPos);
            SpawnHealth(spawnPosition);
        }

        private Vector3 GetStartPositionZ(Vector3 startPos)
        {
            if (pointsToScore % 2 == 0)
            {
                startPos = new Vector3(startPos.x, startPos.y, startPos.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);
            }
            else
            {
                startPos = new Vector3(startPos.x, startPos.y, startPos.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);
            }
            return startPos;
        }

        private void SpawnHealth(Vector3 spawnPosition)
        {
            for (int i = 0; i < pointsToScore; i++)
            {
                int photonViewID = PhotonNetwork.AllocateViewID(false);
                photonView.RPC("RPCSpawnHealthRepresentation", RpcTarget.AllBuffered, spawnPosition, 5, photonViewID);
                spawnPosition += new Vector3(0, 0, 1.5f);
            }
        }

        private void RotateIfEven(int roomPlayerCount)
        {
            photonView.RPC("RPCRotateIfEven", RpcTarget.AllBuffered, roomPlayerCount);
        }

        [PunRPC]
        private void RPCSpawnHealthRepresentation(Vector3 spawnPosition, int pointsToScore,int id)
        {
            if (healthPedestal != null)
            {
                    GameObject spawnedObj = Instantiate(healthPedestal, spawnPosition, Quaternion.identity, transform);
                    PhotonView view = spawnedObj.GetComponent<PhotonView>();
                    view.ViewID = id;
            }
        }

        [PunRPC]
        private void RPCRotateIfEvent(int roomPlayerCount)
        {
            bool isEven = (roomPlayerCount == 2);
            if (isEven)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
            }
        }
    }
}

