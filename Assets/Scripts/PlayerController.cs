using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> scorePedestals;
    [SerializeField] private GameObject pickupUiPrefab;

    private PickupUi pickupUi;
    private int currentScore = 0;

    public PickupUi PickupUi { get => pickupUi; set => pickupUi = value; }

    public void RearrangePedestals()
    {
        scorePedestals.Reverse();
    }

    public void AddScorePedestal(PlayerScore pedestalToAdd)
    {
        scorePedestals.Add(pedestalToAdd);
    }

    public void OnChoiceStart(int pedestalToActivate)
    {
        scorePedestals[pedestalToActivate]?.Activate();
    }

    public void OnChoiceEnd(int pedestalToDeactivate)
    {
        scorePedestals[pedestalToDeactivate]?.Deactivate();
    }

    public int GetChoice(int comparedPedestal)
    {
        return scorePedestals[comparedPedestal].CurrentlyPickedOption;
    }
    
    public void Score(int comparedPedestal)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            currentScore++;
        }
        scorePedestals[comparedPedestal]?.SetFinished(true);
    }

    public void Lose(int comparedPedestal)
    {
        scorePedestals[comparedPedestal]?.SetFinished(false);
    }

    public void TryGetPickupUi(System.Action<int> pickupCallback)
    {
        if (pickupUi == null)
        {
            SpawnPickupUi();
        }
        pickupUi.Activate(pickupCallback);
    }

    private void SpawnPickupUi()
    {
        GameObject pickupGameobject = Instantiate(pickupUiPrefab);
        pickupGameobject.transform.parent = FindObjectOfType<Canvas>().transform;
        pickupGameobject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        pickupUi = pickupGameobject.GetComponent<PickupUi>();
    }
}
