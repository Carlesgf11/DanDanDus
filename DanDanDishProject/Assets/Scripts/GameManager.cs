using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Prueba Commit
    public Animator ButtonsAnim;
    public List<GameObject> flagsImages;
    public enum GameState { CHOOSE, ACTION, RELOCATE, GAMEFINISHED };
    public GameState state;
    public bool TimelineIsDone = false;
    bool TimelineOutDone = false;
    public float countDown;
    public Transform cameraTarget;

    public Transform[] playersInstSpots;
    public GameObject player1, player2;

    public Text countDownText;

    public GameObject winnerPanel;
    public Text winnerText;

    [Header("CargarInfoPlayers")]
    public int Player1Char, Player2Char;
    public List<ScriptableCharacters> characters;

    [Header("Pause")]
    public GameObject pausePanel;
    public bool pause;
    public GameObject pauseContentPanel;
    public GameObject optionsPanel;

    [Header("Sounds")]
    public AudioManager audioManager;
    public AudioSource arrowImpact;
    public AudioSource arrowImpact2;

    PhotonView view;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        SetGame();
    }

    private void SetGame()
    {
        pause = false;
        player1 = playersInstSpots[0].transform.GetChild(0).transform.GetChild(0).gameObject;
        player2 = playersInstSpots[1].transform.GetChild(0).transform.GetChild(0).gameObject;

        countDown = 4;
        pauseContentPanel.SetActive(true);
        winnerPanel.SetActive(false);
        pausePanel.SetActive(false);
        //optionsPanel.SetActive(false); //Si se hace este set active se activa la funcion onDisable del VolumeSetting y se pone chungo todo
        Player1Char = PlayerPrefs.GetInt("Player1", 0);
        Player2Char = PlayerPrefs.GetInt("Player2", 0);
        //print(Player1Char);
        //print(Player2Char);
        for (int i = 0; i < flagsImages.Count; i++)
        {
            flagsImages[i].GetComponent<Image>().sprite = characters[Player1Char].flagSprite;
            flagsImages[i].transform.GetChild(0).GetComponent<Image>().color = characters[Player1Char].UIColor;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Player1Win();
            //player1.GetComponent<PlayerControl>().Win();
        //
        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //    player2.GetComponent<PlayerControl>().Win();
        //
        //if (Input.GetKeyDown(KeyCode.W))
        //    player1.GetComponent<PlayerControl>().Lose();
        //
        //if (Input.GetKeyDown(KeyCode.Q))
        //    player2.GetComponent<PlayerControl>().Lose();

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
            case GameState.GAMEFINISHED:
                break;
        }
    }

    void ChooseUpdate()
    {
        countDown -= Time.deltaTime;
        countDownText.text = ((int)countDown).ToString();
        player1.GetComponent<PlayerControl>().ButtonsAnim.SetBool("Appear", true);
        player2.GetComponent<PlayerControl>().ButtonsAnim.SetBool("Appear", true);

        if (countDown < 1)
        {
            player1.GetComponent<PlayerControl>().ButtonsAnim.SetBool("Appear", false);
            player2.GetComponent<PlayerControl>().ButtonsAnim.SetBool("Appear", false);
            countDown = 0;
            state = GameState.ACTION;
        }
    }

    void ActionUpdate()
    {
        EventSystem.current.SetSelectedGameObject(null);
        int _Player1 = player1.GetComponent<PlayerControl>().CurrentAction;
        int _Player2 = player2.GetComponent<PlayerControl>().CurrentAction;
        if (_Player1 == _Player2)
        {
            countDown = 4;
            Invoke("ReturnToChoose", 1f);
        }
        else if (_Player1 < _Player2 && _Player2 != 3 && _Player2 != 1)
        {
            Player2Win();
        }
        else if (_Player1 > _Player2 && _Player1 != 3 && _Player1 != 1)
        {
            Player1Win();
        }
        else if (_Player1 < _Player2 && _Player2 != 2)
        {
            countDown = 4;
            Invoke("ReturnToChoose", 1f);
        }
        else if (_Player1 > _Player2 && _Player1 != 2)
        {
            countDown = 4;
            Invoke("ReturnToChoose", 1f);
        }
    }


    public void Player1Win()
    {
        player1.GetComponent<PlayerControl>().currentCheckpoint++;
        player2.GetComponent<PlayerControl>().currentCheckpoint--;
        player1.GetComponent<PlayerControl>().Win();
        player2.GetComponent<PlayerControl>().Lose();
        audioManager.PlaySound(arrowImpact);
        state = GameState.RELOCATE;
    }

    public void Player2Win()
    {
        player2.GetComponent<PlayerControl>().currentCheckpoint++;
        player1.GetComponent<PlayerControl>().currentCheckpoint--;
        player2.GetComponent<PlayerControl>().Win();
        player1.GetComponent<PlayerControl>().Lose();
        audioManager.PlaySound(arrowImpact);
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

    public void FinishGame(GameObject _winner)
    {
        winnerPanel.SetActive(true);
        winnerText.text = _winner.name + " wins";
        print(_winner + "Wins");
        state = GameState.GAMEFINISHED;
    }

    void RelocateUpdate()
    {
        
    }

    #region Pause
    public void OnApplicationPause(bool _pause)
    {
        if(_pause)
        {
            Time.timeScale = 0;
            pause = true;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pause = false;
            pausePanel.SetActive(false);
        }
    }
    #endregion
}
