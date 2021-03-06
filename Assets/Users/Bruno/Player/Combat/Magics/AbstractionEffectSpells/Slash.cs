using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slash : Spells, IEffect
{
    private GameObject target;
    //public TrailRenderer trail;
    private float bufferAttack;
    public int damage;
    private void Start()
    {
        bufferAttack = Hertz;
    }
    public override void Aim()
    {
    }
    public override void Use()
    {
        Apply(null);
        if (target)
            GetComponent<PhotonView>().RPC("CalculateDamage", RpcTarget.All, target.gameObject.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    private void CalculateDamage(int targetId)
    {
        target = PhotonView.Find(targetId).gameObject;
        //target.GetComponent<PlayerProperty>().Life = damage;
        target.GetComponent<ChunkDetector>().DetectHit(GetComponent<Collider>());
        Apply(null);
    }
    public void Apply(Animator animatorId = null)
    {
        //trail.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.layer != gameObject.layer)
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
    private void Update()
    {
        Hertz -= Time.deltaTime;
        if (Hertz < 0)
        {
            Hertz = bufferAttack;
            //trail.enabled = false;
        }
        //devo rotacionar de acordo com a camera
        //é diminuir e distancia a area de ataque
    }
}
