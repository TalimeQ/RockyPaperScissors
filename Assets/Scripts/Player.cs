using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS.player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerInitializer initializer;

        private int playerScore = 0;

        void Start()
        {
            Vector3 startPos = transform.position + new Vector3(2, 0, 0);
            initializer.Init(startPos, 5);
        }
    }
}

