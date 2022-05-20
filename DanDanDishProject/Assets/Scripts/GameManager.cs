using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { CHOOSE, ACTION, RELOCATE };
    public GameState state;

    public float countDown;
    public Transform cameraTarget;
    public GameObject player1, player2;

    private void Start()
    {
        countDown = 5;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            player1.GetComponent<PlayerControl>().Win();

        if(Input.GetKeyDown(KeyCode.Alpha2))
            player2.GetComponent<PlayerControl>().Win();

        if (Input.GetKeyDown(KeyCode.W))
            player1.GetComponent<PlayerControl>().Lose();

        if (Input.GetKeyDown(KeyCode.Q))
            player2.GetComponent<PlayerControl>().Lose();

        switch (state)
        {
            case GameState.CHOOSE:
                ChooseUpdate();
                break;
            case GameState.ACTION:
                ActionUpdate();
                break;
            case GameState.RELOCATE:
                RelocateUpdate();
                break;
        }
    }

    void ChooseUpdate()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
            state = GameState.ACTION;
    }

    void ActionUpdate()
    {
        int _Player1 = player1.GetComponent<PlayerControl>().CurrentAction;
        int _Player2 = player2.GetComponent<PlayerControl>().CurrentAction;
        if (_Player1 == _Player2)
        {
            countDown = 5;
            Invoke("ReturnToChoose", 1f);
        }else if(_Player1 < _Player2 && _Player2 != 3)
        {
            Player2Win();
        }
        else if (_Player1 > _Player2 && _Player1 != 3)
        {
            Player1Win();
        }
        else if (_Player1 < _Player2 && _Player2 != 2)
        {
            countDown = 5;
            Invoke("ReturnToChoose", 1f);
        }
        else if (_Player1 > _Player2 && _Player1 != 2)
        {
            countDown = 5;
            Invoke("ReturnToChoose", 1f);
        }
    }

    public void Player1Win()
    {
        player1.GetComponent<PlayerControl>().currentCheckpoint++;
        player2.GetComponent<PlayerControl>().currentCheckpoint--;
        player1.GetComponent<PlayerControl>().Win();
        player2.GetComponent<PlayerControl>().Lose();
        state = GameState.RELOCATE;
    }

    public void Player2Win()
    {
        player2.GetComponent<PlayerControl>().currentCheckpoint++;

        player1.GetComponent<PlayerControl>().currentCheckpoint--;
        player2.GetComponent<PlayerControl>().Win();
        player1.GetComponent<PlayerControl>().Lose();
        state = GameState.RELOCATE;
    }

    public void ReturnToChoose()
    {
        player1.GetComponent<PlayerControl>().Empate();
        player2.GetComponent<PlayerControl>().Empate();
        player1.GetComponent<PlayerControl>().CurrentAction = 0;
        player2.GetComponent<PlayerControl>().CurrentAction = 0;
        state = GameState.CHOOSE;
    }

    void RelocateUpdate()
    {

    }
}
