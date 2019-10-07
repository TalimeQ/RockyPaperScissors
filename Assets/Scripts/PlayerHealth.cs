using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material lostMaterial;
    [SerializeField] private Material wonMaterial;

    private MeshRenderer healthRenderer;
    private PhotonView photonController;
    private bool isActive = false;

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

    private void Start()
    {
        photonController = GetComponent<PhotonView>();
        healthRenderer = GetComponent<MeshRenderer>();
        healthRenderer.material = inactiveMaterial;
    }

}
