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
        while (currentGameState == GameState.Running)
        {
            EnableChoice();
            yield return delay;
            CompareChoices();
            yield return null;
            FinalizeTurn();
        }

        currentGameState = GameState.Finalizing;
    }

    private void EnableChoice()
    {
        gameManagerView.RPC("RPCEnableChoice", RpcTarget.All);
    }

    private void CompareChoices()
    {
    }

    private void FinalizeTurn()
    {
        bool isLastPoint = battledScorePoint == maxScore - 1;
        if (isLastPoint)
        {
            currentGameState = GameState.Finalizing;
            //finalize game
        }
        else
        {
            gameManagerView.RPC("RPCIncreaseScorePoint", RpcTarget.All);
        }

    }

    [PunRPC]
    private void RPCEnableChoice()
    {
        foreach(PlayerController calledPlayer in playerList)
        {
            calledPlayer.OnChoiceStart(battledScorePoint);
        }
    }

    [PunRPC]
    private void RPCStartGame()
    {
        currentGameState = GameState.Startup;
        StartCoroutine(StartupPhase());
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
