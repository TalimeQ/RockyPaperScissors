using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView controlerView;
    [SerializeField] List<PlayerController> playerList;

    private GameState currentGameState = GameState.AwaitingPlayer;
    private bool isComparingScores = false;
    private int maxScore = 5;
    private int battledScorePoint = 0;

    public void AddPlayerController(PlayerController playerController)
    {
        playerList.Add(playerController);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Entered!");
        base.OnPlayerEnteredRoom(newPlayer);
        currentGameState = GameState.Startup;
        StartCoroutine(GameplayLoop());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        currentGameState = GameState.Finalizing;
    }

    private IEnumerator GameplayLoop()
    {
        WaitForSeconds delayTime = new WaitForSeconds(10);
        while(currentGameState == GameState.Running)
        {
            EnableChoice();
            yield return delayTime;
            CompareChoices();
            yield return null;
            battledScorePoint++;
            // Wait for chocie
            // Compare Choice
            // Score

        }
        
    }

    private void EnableChoice()
    {
        Debug.Log("Chocies goin!");
        controlerView.RPC("RPCEnableChoice", RpcTarget.All);
    }

    private void CompareChoices()
    {
        Debug.Log("Comparing Choices");
    }

    [PunRPC]
    private void RPCEnableChoice()
    {
        foreach(PlayerController calledPlayer in playerList)
        {
            Debug.Log("RPC choices!");
            calledPlayer.OnChoiceStart(battledScorePoint);
        }
    }

    enum GameState
    {
        AwaitingPlayer,
        Startup,
        Running,
        Finalizing,
        Finished,
        Unknown,
    }
}
