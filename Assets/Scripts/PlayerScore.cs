﻿using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScore : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material lostMaterial;
    [SerializeField] private Material wonMaterial;

    private MeshRenderer healthRenderer;
    private PhotonView photonController;
    private bool isActive = false;

    private void Start()
    {
        photonController = GetComponent<PhotonView>();
        healthRenderer = GetComponent<MeshRenderer>();
        healthRenderer.material = inactiveMaterial;
    }

    public void Activate()
    {
        healthRenderer.material = highlightMaterial;
        isActive = true;
    }

    public void Init(PhotonView owner)
    {
        photonController = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(photonController.IsMine)
        {
            NotifyServerClick();
            GetComponent<PhotonView>()?.RPC("NotifyServerClick", RpcTarget.AllBuffered);
        }   
    }

    public void SetFinished(bool isWon)
    {
        if(isWon)
        {
            healthRenderer.material = lostMaterial;
        }
        else
        {
            healthRenderer.material = wonMaterial;
        }
    }
    
    [PunRPC]
    private void NotifyServerClick()
    {
        Activate();
    }


}
