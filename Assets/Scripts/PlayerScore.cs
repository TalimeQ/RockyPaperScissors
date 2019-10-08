﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScore : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material lostMaterial;
    [SerializeField] private Material wonMaterial;
    [SerializeField] private MeshRenderer healthRenderer;
    [SerializeField] private List<GameObject> choicesRepresentation;
    [SerializeField] private Transform spawnPoint;

    private PhotonView photonController;
    private GameObject localRepresentation;
    private GameObject serverRepresentation;
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

    public void Deactivate()
    {
        healthRenderer.material = inactiveMaterial;
        isActive = false;
    }

    public void Init(PhotonView owner)
    {
        photonController = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(photonController.IsMine && isActive)
        {
            GetComponentInParent<PlayerController>()?.TryGetPickupUi(GetUiCallback);
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

    private void GetUiCallback(int pickedOption)
    {
        if(localRepresentation == null)
        {
            Instantiate(choicesRepresentation[pickedOption], spawnPoint.position, Quaternion.identity, transform);
        }
        else
        {
            Destroy(localRepresentation.gameObject);
            Instantiate(choicesRepresentation[pickedOption], spawnPoint.position, Quaternion.identity, transform);
        }
        photonController.RPC("RPCPickupNotify",RpcTarget.All, pickedOption);
    }

    [PunRPC]
    private void RPCPickupNotify(int pickedOption)
    {
        Instantiate(choicesRepresentation[0], spawnPoint.position, Quaternion.identity, transform);
    }

}
