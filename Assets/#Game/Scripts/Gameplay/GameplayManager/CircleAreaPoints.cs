using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Pun.UtilityScripts;

public class CircleAreaPoints : MonoBehaviour
{
    public static CircleAreaPoints instance;
    public float pointsTeam1;
    public float pointsTeam1PerCent;
    public float pointsTeam2;
    public float pointsTeam2PerCent;
    private float pointsTeam1Bar;
    private float pointsTeam2Bar;
    public PhotonView PV;

    [SerializeField] int maxPoints;

    [SerializeField] Image pointsBarImageTeam1;
    [SerializeField] Image pointsBarImageTeam2;

    [SerializeField] TMP_Text pointsUiTeam1;
    [SerializeField] TMP_Text pointsUiTeam2;
   
    [SerializeField] int constPoint;

  
    public int countPlayerinAreaTeam1;
    public int countPlayerinAreaTeam2;

    private int countPlayerExtraTeam1;
    private int countPlayerExtraTeam2;
    private float max;
    public bool endingGame = false;



    void Start()
    {
       // pointsTeam1 = 0;
        //pointsTeam2 = 0;
        //countPlayerinAreaTeam1 = 0;
       // countPlayerinAreaTeam2 = 0;
        instance = this;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PointsControl();

    }
 
    private void PointsControl()
    {
        pointsBarImageTeam1.fillAmount = pointsTeam1Bar;
        pointsBarImageTeam2.fillAmount = pointsTeam2Bar;



          // if (countPlayerinAreaTeam1 != countPlayerinAreaTeam2)
           // {
            if (pointsTeam1PerCent < 100 && pointsTeam2PerCent < 100 && endingGame==false)
            {
                

                
                string pointStringTeam1 = string.Format("{0:00}", pointsTeam1PerCent);
                pointsUiTeam1.text = (pointStringTeam1 +"%");


                
                string pointStringTeam2 = string.Format("{0:00}", pointsTeam2PerCent);
                pointsUiTeam2.text = (pointStringTeam2 + "%");

                int diffPlayerinArea = countPlayerinAreaTeam1 - countPlayerinAreaTeam2;
                if (diffPlayerinArea > 0)
                {
                    countPlayerExtraTeam1 = diffPlayerinArea;
                    countPlayerExtraTeam2 = 0;
                   audioGameplayController.instance.dominouBlue = true;
                if (audioGameplayController.instance.dominouRed == true) audioGameplayController.instance.audioAreaRed();
                }
                else if (diffPlayerinArea < 0)
                {
                    countPlayerExtraTeam2 = -diffPlayerinArea;
                    countPlayerExtraTeam1 = 0;
                audioGameplayController.instance.dominouRed = true;
                if (audioGameplayController.instance.dominouBlue == true) audioGameplayController.instance.audioAreaBlue();
                }
                else if (diffPlayerinArea == 0)
                {
                    countPlayerExtraTeam2 = 0;
                    countPlayerExtraTeam1 = 0;
                    audioGameplayController.instance.dominouBlue = true;
                    audioGameplayController.instance.dominouRed = true;
                }

                pointsTeam1 = pointsTeam1 + constPoint * Time.fixedDeltaTime * countPlayerExtraTeam1;
                pointsTeam2 = pointsTeam2 + constPoint * Time.fixedDeltaTime * countPlayerExtraTeam2;
                max = maxPoints / 100;
                pointsTeam1PerCent = pointsTeam1 / max;
                pointsTeam2PerCent = pointsTeam2 / max;
                pointsTeam1Bar = (pointsTeam1PerCent * max) / maxPoints;
                pointsTeam2Bar = (pointsTeam2PerCent * max) / maxPoints;

                if (PhotonNetwork.IsMasterClient)
                {
                    
                    PV.RPC("sendPoints", RpcTarget.Others, pointsTeam1PerCent, pointsTeam2PerCent);
                    PV.RPC("sendPointsBar", RpcTarget.Others, pointsTeam1Bar, pointsTeam2Bar);
                }


            }
            else
                 {
                    if (endingGame) return;
                    EndGamebyPoints();
                } 
      //  }
            
     
    }
    [PunRPC]
    public void RPC_sendIncrease(string team)
    {
        if(team=="Blue")countPlayerinAreaTeam1++;
        if(team=="Red")countPlayerinAreaTeam2++;
    }
    [PunRPC]
    public void RPC_sendDecrease(string team) 
    {
        if (team == "Blue") this.countPlayerinAreaTeam1--;
        if (team == "Red") this.countPlayerinAreaTeam2--;
    }

    [PunRPC]
    public void sendPoints(float teamPontoB, float teamPontoR)
    {

        pointsTeam1PerCent = teamPontoB;
        pointsTeam2PerCent = teamPontoR;

       
    }
    [PunRPC]
    public void sendPointsBar(float teamPontoB, float teamPontoR)
    {

        pointsTeam1Bar= teamPontoB;
        pointsTeam2Bar = teamPontoR;


    }



    private void EndGamebyPoints()
    {
        string pTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
        endingGame = true;
        Debug.Log("GAME ENDS: ");
        if (pointsTeam1PerCent > pointsTeam2PerCent)
        {
            if (pTeam == "Blue")
            {
               GameplayManager.instance.vitoriaUi.SetActive(true);
            }
            if (pTeam == "Red")
            {
                GameplayManager.instance.derrotaUi.SetActive(true);
            }

        }
        if (pointsTeam2PerCent > pointsTeam1PerCent)
        {
            if (pTeam == "Blue")
            {
                GameplayManager.instance.derrotaUi.SetActive(true);
            }
            if (pTeam == "Red")
            {
                GameplayManager.instance.vitoriaUi.SetActive(true);
            }
        }
        if (pointsTeam2PerCent == pointsTeam1PerCent)
        {
            Debug.Log("EMPATE");
            GameplayManager.instance.msg = ("EMPATOU");
            GameplayManager.instance.vitoriaUi.SetActive(true);
        }
        GameplayManager.instance.gameEndActive();
    }


    void OnTriggerEnter(Collider collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.layer==8)//team1
             {
                PV.RPC("RPC_sendIncrease", RpcTarget.All, "Blue");
            }
            if (collision.gameObject.layer == 9)//team2
            {
                PV.RPC("RPC_sendIncrease", RpcTarget.All, "Red");
            }
         }

    }


    void OnTriggerExit(Collider collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.layer == 8)//team1
            {
                PV.RPC("RPC_sendDecrease", RpcTarget.All, "Blue");
            }
            if (collision.gameObject.layer == 9)//team2
            {
                PV.RPC("RPC_sendDecrease", RpcTarget.All, "Red");
            }
        }

    }



}
