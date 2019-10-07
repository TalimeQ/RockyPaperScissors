using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<PlayerScore> scorePedestals;

    public void AddScorePedestal(PlayerScore pedestalToAdd)
    {
        scorePedestals.Add(pedestalToAdd);
    }

    public void OnChoiceStart(int pedestalToActivate)
    {
        scorePedestals[pedestalToActivate]?.Activate();
    }

    public void OnChoiceEnd()
    {
        
    }

    public void OnCompareScore()
    {

    }
}
