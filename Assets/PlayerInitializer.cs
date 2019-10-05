using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] GameObject healthPedestal;

    public void Init(Vector3 startPosition, int pointsToScore)
    {
        SpawnHealthRepresentation(startPosition, pointsToScore);
    }

    private void SpawnHealthRepresentation(Vector3 startPosition, int pointsToScore)
    {
        if(healthPedestal != null)
        {
           for(int i = 0; i < pointsToScore; i ++)
           {
                Instantiate(healthPedestal, startPosition, Quaternion.identity, transform.parent);
                startPosition += new Vector3(0, 1, 0);
           }
        }
        else
        {

        }
    }
}
