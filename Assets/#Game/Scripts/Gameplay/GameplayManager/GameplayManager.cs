using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class GameplayManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject setUpGameplay;
    public TMP_Text timeToDisplay;
    public static GameplayManager instance;
    private bool endingGame = false;
    private TimerCountdown gameplayRoomTimer;
    public GameObject vitoriaUi;
    public GameObject derrotaUi;
    public string msg;
   public Text msgGameEnd;
    [SerializeField] GameObject spawnWallBlue;
    [SerializeField] GameObject spawnWallRed;
    private bool wallDown=false;
    public PhotonView PV;
    private string tempTimer;
    public audioGameplayController audioGameplaySceneScript;
    private void Start()
    {
        gameplayRoomTimer = GetComponent<TimerCountdown>();
        gameplayRoomTimer.CurrentTime = RoomConfigs.instance.heightTime;
        gameplayRoomTimer.BaseTime = RoomConfigs.instance.gameplayTimeBase;
        tempTimer = string.Format("{0:00}", gameplayRoomTimer.BaseTime);
        wallDown = false;
        instance = this;
        vitoriaUi.SetActive(false);
        derrotaUi.SetActive(false);
        

    }

    private void Update()
    {
        UIUpdate();
        audioFortySecondsRemaning();
        if (gameplayRoomTimer.IsBasedownOver() && wallDown == false)
        {
            
            downWallBase();
            gameplayRoomTimer.CurrentTime = RoomConfigs.instance.gameplayMaxTime;
            gameplayRoomTimer.BaseTime = RoomConfigs.instance.heightTime;

            voiceLineStartGame();
            wallDown = true;
        }
            if (gameplayRoomTimer.IsCountdownOver() && wallDown==true )
        {
            if (endingGame) return;
            EndGamebyTimer();
        }
       
    }
    private void audioFortySecondsRemaning()
    {
        if (gameplayRoomTimer.CurrentTime < 41.1f && gameplayRoomTimer.CurrentTime > 40.9f)
        {
            audioGameplaySceneScript.audioGameplayScenePV("gameplayScene");
            audioGameplaySceneScript.audioGameplayScenePV("secondsRemaning");
        }
    }
    private void voiceLineStartGame()
    {
        //GetComponent<audioCharacterSceneController>().audioPlayerVoiceLines("startGame", 1);
        audioGameplaySceneScript.audioPlayerVoiceLines("startGame",1);

    }
    private void UIUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("sendTime", RpcTarget.Others, gameplayRoomTimer.CurrentTime);
            PV.RPC("sendBaseTime", RpcTarget.Others, gameplayRoomTimer.BaseTime);
            PV.RPC("SendwallDown", RpcTarget.Others, wallDown);
    }
        if(wallDown)tempTimer = string.Format("{0:00}", gameplayRoomTimer.CurrentTime);
        if(!wallDown)tempTimer = string.Format("{0:00}", gameplayRoomTimer.BaseTime);
        timeToDisplay.text = tempTimer;
    }

    public void EndGamebyTimer()
    {
        string pTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;

        endingGame = true;
        CircleAreaPoints.instance.endingGame = true;
        Debug.Log("Acabou o jogo pelo tempo: ");
       
        if (CircleAreaPoints.instance.pointsTeam1PerCent > CircleAreaPoints.instance.pointsTeam2PerCent) 
        {
            Debug.Log("TEAM1 WINS");
            msg = ("Time is over: TEAM BLUE WINS ");
            if (pTeam == "Blue")
            {
                vitoriaUi.SetActive(true);
            }
            if (pTeam == "Red")
            {
                derrotaUi.SetActive(true);
            }

        }
        else if(CircleAreaPoints.instance.pointsTeam1PerCent < CircleAreaPoints.instance.pointsTeam2PerCent)
        {
            Debug.Log("TEAM2 WINS");
            msg = ("Time is over: TEAM RED WINS ");
            if (pTeam == "Blue")
            {
                derrotaUi.SetActive(true);
            }
            if (pTeam == "Red")
            {
                vitoriaUi.SetActive(true);
            }
            
        }
        else if(CircleAreaPoints.instance.pointsTeam1PerCent == CircleAreaPoints.instance.pointsTeam2PerCent)
        {
            Debug.Log("EMPATE");
            msg = ("Time is over: EMPATOU ");
            vitoriaUi.SetActive(true);
        }
        gameEndActive();

    }
    private void downWallBase()
    {
        Destroy(spawnWallBlue);
        Destroy(spawnWallRed);
    }
    public void gameEndActive()
    {
        //Time.timeScale = 0;
       
        
        //msgGameEnd.text = msg;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

    }

    public void backToMenu()
    {
        Time.timeScale = 1;
        //Application.Quit();
        SceneManager.LoadScene(RoomConfigs.instance.menuSceneIndex);

    }
    [PunRPC]
    public void sendTime(float time)
    {

        gameplayRoomTimer.CurrentTime = time;


    }
    public void sendBaseTime(float time)
    {

        gameplayRoomTimer.BaseTime = time;


    }
    public void SendwallDown(bool wallDownMaster)
    {

        wallDown= wallDownMaster;


    }
}
