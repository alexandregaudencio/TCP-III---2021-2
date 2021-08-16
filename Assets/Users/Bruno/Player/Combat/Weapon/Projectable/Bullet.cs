﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed;
    public float existenceTomeout;
    private float countTime;
    public IEffect effect;
    [HideInInspector]
    public Pool pool;
    private float timeOfArrival;
    private float distance;
    private new Transform transform;
    private bool fired = false;
    public Vector3 hit;
    [HideInInspector]
    public string animationName;
    private GameObject photonBullet;

    private void Start()
    {
        photonBullet = PhotonView.Find(photonView.ViewID).gameObject;
    }

    [PunRPC]
    public void ActiveAll(bool value)
    {
        GameObject me = PhotonView.Find(photonView.ViewID).gameObject;

        PhotonView.Find(me.GetComponentInChildren<PhotonView>().ViewID).gameObject.SetActive(value);

        if (!value)
        {
            me.transform.position = Vector3.zero;
        }

        me.gameObject.SetActive(value);
    }

    [PunRPC]
    public void Inicialize(Vector3 point, float timeOfArrival, Vector3 pos, Vector3 rot)
    {
        pool.Out(photonView.ViewID);
        pool.ActiveInstance();
        hit = point;
        countTime = 0;
        this.transform = gameObject.transform;
        fired = true;
        ActiveAll(true);
        TimeOfArrival(timeOfArrival);
        transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));

    }
    void Update()
    {
        BulletLife();
    }
    [PunRPC]
    public void Timeout()
    {
        countTime = 0;
        pool.In(photonView.ViewID);
        ActiveAll(false);
 
    }
    public void TimeOfArrival(float distance)
    {
        this.distance = distance;
        timeOfArrival = distance / speed;
    }
    [PunRPC]
    public void CombineWithMaic()
    {
        var vfx = GetComponentInChildren<Animator>();
        vfx.transform.position = hit;
        effect.Apply(vfx);
    }
    public void CalculateDamage()
    {
    }
    [PunRPC]
    private void DetectCollier()
    {
        //detecta se atingiu o alvo e aplica todas as
        //animações magicas se houver e o dano causado pelo artefato
        //e desativa e retorna a bala para a piscina apos um tempo
        if (timeOfArrival <= 0)
        {
            CombineWithMaic();
            CalculateDamage();
            fired = false;
        }
    }
    [PunRPC]
    private void BulletLife()
    {
        //mover a bala pra frente
        if (fired)
        {
            timeOfArrival -= Time.deltaTime;
            countTime += Time.deltaTime;

            Debug.Log(countTime);
            // desativa e retorna a bala para a piscina apos um tempo
            if (countTime >= existenceTomeout)
            {
                Timeout();
                return;
            }
            DetectCollier();

            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            EndAniamtion();
        }
    }
    private void EndAniamtion()
    {
        if (transform == null)
        {
            return;
        }
        transform.position = hit;
        Animator ani = GetComponentInChildren<Animator>();

        if (ani.GetCurrentAnimatorStateInfo(0).IsName(animationName) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Timeout();
        }
    }
}
