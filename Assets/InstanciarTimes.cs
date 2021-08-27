﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;
using Photon.Pun.UtilityScripts;


public class InstanciarTimes : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    public GameObject[] SpawnPointsBlue, SpawnPointsRed;
    //public byte meutime, JogadorEscolhido;
    public GameObject[] Characters;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }


    void Start()
    {

        InstantiatingPlayersCharacters();
    //    PV = GetComponent<PhotonView>();
    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //   {

        //        if (PV.IsMine)
        //        { 
        ////            if(GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().MyTeam== 1)
        ////            {
        ////            meutime = GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().MyTeam;
        ////            JogadorEscolhido = GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().jogadorEscolhido;
        //PV.RPC("RPCStartGame", player, this.SpawnPointsBlue[i].transform.position, this.SpawnPointsBlue[i].transform.rotation);
        ////            }
        ////           else if (GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().MyTeam == 2)
        ////            {
        ////                meutime = GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().MyTeam;
        ////                JogadorEscolhido = GameObject.Find("PlayerAssister").GetComponent<PlayerAssister>().jogadorEscolhido;
        ////                PV.RPC("RPCStartGame", PhotonNetwork.PlayerList[i], this.SpawnPointsB[i].transform.position, this.SpawnPointsB[i].transform.rotation);
        ////            }
        //        }
        //    }

    }

    private void InstantiatingPlayersCharacters()
    {

        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
            string pTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
        //    //Debug.Log("Time de" + player.NickName + ": " + pTeam);

            if (pTeam == "Blue")
            {
                PV.RPC("RPCStartGame", PhotonNetwork.LocalPlayer, SpawnPointsBlue[0].transform.position, SpawnPointsBlue[0].transform.rotation);
                //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                //SpawnPointsBlue[0].transform.position, SpawnPointsBlue[0].transform.rotation, 0);
            }
            if (pTeam == "Red")
            {
                PV.RPC("RPCStartGame", PhotonNetwork.LocalPlayer, SpawnPointsRed[0].transform.position, SpawnPointsRed[0].transform.rotation);
            }

        //}
    }

    [PunRPC]
    void RPCStartGame(Vector3 spawnPos, Quaternion spawnRot)
    {
        //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), spawnPos, spawnRot, 0);
        myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), spawnPos, spawnRot);
    }
}