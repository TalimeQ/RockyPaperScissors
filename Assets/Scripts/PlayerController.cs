using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> scorePedestals;
    [SerializeField] private GameObject pickupUiPrefab;

    private PickupUi pickupUi;

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

    public void OnCompareScore(int comparedPedestal)
    {

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
