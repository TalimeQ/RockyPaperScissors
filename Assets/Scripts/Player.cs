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

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;


        private int playerScore = 0;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                Player.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            Vector3 startPos = transform.position + new Vector3(2, 0, 0);
            initializer.Init(startPos, 5);
        }
    }
}

