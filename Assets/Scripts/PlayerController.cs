using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> scorePedestals;

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
}
