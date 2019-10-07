using Photon.Pun;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject healthPedestal;
    [SerializeField] private float spawnOffset = 1.5f;

    public void Init(Vector3 startPosition, int pointsToScore)
    {
        SpawnHealthRepresentation(startPosition, pointsToScore);
    }

    private void SpawnHealthRepresentation(Vector3 startPosition, int pointsToScore)
    {
        if(healthPedestal != null)
        {
           Vector3 spawnPosition;
           if(pointsToScore % 2 == 0)
           {
                spawnPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);     
           }
           else
           {
                spawnPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - Mathf.FloorToInt(pointsToScore / 2) * spawnOffset);
            }
           for(int i = 0; i < pointsToScore; i ++)
           {
                GameObject spawnedObj = PhotonNetwork.Instantiate(healthPedestal.name, spawnPosition, Quaternion.identity);
                spawnedObj.transform.parent = transform;
                spawnPosition += new Vector3(0, 0, spawnOffset);
           }
        }
    }
}
