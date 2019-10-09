using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField] PhotonView gameManagerView;

    private List<PlayerController> playerList = new List<PlayerController>();
    private GameState currentGameState = GameState.AwaitingPlayer;
    private bool isComparingScores = false;
    private int maxScore = 5;
    private int battledScorePoint = 0;

    #region PhotonCallbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        gameManagerView.RPC("RPCStartGame", RpcTarget.MasterClient);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        currentGameState = GameState.Finalizing;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentGameState);
            stream.SendNext(battledScorePoint);
        }
        else
        {
            currentGameState = (GameState)stream.ReceiveNext();
            battledScorePoint = (int)stream.ReceiveNext();
        }
    }

    #endregion

    public void AddPlayerController(PlayerController playerController)
    {
        playerList.Add(playerController);
    }

    private IEnumerator StartupPhase()
    {
        yield return new WaitForSeconds(2);
        currentGameState = GameState.Running;
        StartCoroutine(GameplayLoop());
    }

    private IEnumerator GameplayLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(10);
        WaitForSeconds shortDelay = new WaitForSeconds(2);
        while (currentGameState == GameState.Running)
        {
            EnableChoice();
            yield return delay;
            FinalizeChoices();
            yield return shortDelay;
            CompareChoices();
            yield return shortDelay;
            FinalizeTurn();
        }

        currentGameState = GameState.Finalizing;
    }

    private void EnableChoice()
    {
        gameManagerView.RPC("RPCEnableChoice", RpcTarget.All);
    }

    private void FinalizeChoices()
    {
        gameManagerView.RPC("RPCFinalizeChoices", RpcTarget.All);
    }

    private void CompareChoices()
    {
        gameManagerView.RPC("RPCCompareChoices", RpcTarget.AllBuffered);
    }

    private void FinalizeTurn()
    {
        bool isLastPoint = battledScorePoint == maxScore - 1;
        if (isLastPoint)
        {
            currentGameState = GameState.Finalizing;
        }
        else
        {
            gameManagerView.RPC("RPCIncreaseScorePoint", RpcTarget.All);
        }

    }

    // TODO Refactor if time allows , or do it properly not in jam style...
    private void CompareScores(int comparedPoint)
    {
        int playerOneChoice = playerList[0].GetChoice(comparedPoint);
        int playerTwoChoice = playerList[1].GetChoice(comparedPoint);

        if( playerOneChoice == playerTwoChoice)
        {
            foreach(PlayerController player in playerList)
            {
                player.Lose(comparedPoint);
            }
        }
        else if (playerOneChoice == 3 &&  playerTwoChoice == 1)
        {
            playerList[0].Score(comparedPoint);
            playerList[1].Lose(comparedPoint);
        }
        else if (playerOneChoice == 1 && playerTwoChoice == 3)
        {
            playerList[1].Score(comparedPoint);
            playerList[0].Lose(comparedPoint);
        }
        else
        {
            if(playerOneChoice > playerTwoChoice)
            {
                playerList[0].Score(comparedPoint);
                playerList[1].Lose(comparedPoint);
            }
            else
            {
                playerList[1].Score(comparedPoint);
                playerList[0].Lose(comparedPoint);
            }
        }

    }

    [PunRPC]
    private void RPCStartGame()
    {
        currentGameState = GameState.Startup;
        StartCoroutine(StartupPhase());
    }

    [PunRPC]
    private void RPCEnableChoice()
    {
        foreach (PlayerController calledPlayer in playerList)
        {
            calledPlayer.OnChoiceStart(battledScorePoint);
        }
    }

    [PunRPC]
    private void RPCCompareChoices()
    {
        foreach (PlayerController calledPlayer in playerList)
        {
            calledPlayer.OnChoiceEnd(battledScorePoint);
        }
        CompareScores(battledScorePoint);
    }


    [PunRPC]
    private void RPCFinalizeChoices()
    {
        foreach (PlayerController calledPlayer in playerList)
        {
            calledPlayer.PickupUi?.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void RPCIncreaseScorePoint()
    {
        battledScorePoint++;
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
