using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Pool : MonoBehaviourPun
{
    public List<int> activeGroup;
    public List<int> inactiveGroup;

    public GameObject projectable;
    public int count;
    private GameObject factory = null;
    public int selected;
    [PunRPC]
    private void OpenFactory(int id)
    {
        factory = PhotonView.Find(id).gameObject;
    }
    void Start()
    {
        activeGroup = new List<int>();
        inactiveGroup = new List<int>();

        if (photonView.IsMine)
        {
            var obj = PhotonNetwork.Instantiate(Path.Combine("Projectable", projectable.name), Vector3.zero, Quaternion.identity, 0);
            photonView.RPC("OpenFactory", RpcTarget.AllViaServer, obj.gameObject.GetComponent<PhotonView>().ViewID);
            for (int i = 0; i < count; i++)
            {
                photonView.RPC("AddMoreElement", RpcTarget.AllViaServer);
            }

        }

        //AddMoreElement();
    }

    [PunRPC]
    private void AddMoreElement()
    {
        var f = factory.GetComponent<ProjectableFactory>();

        int bulletId;
        int vfxId;

        if (photonView.IsMine)
        {
            bulletId = f.BulletFactory();
            vfxId = f.BulletEffect();

            GameObject bullet = PhotonView.Find(bulletId).gameObject;

            f.photonView.RPC("PhotonSetParent", RpcTarget.All, bulletId, vfxId);

            f.photonView.RPC("BulletSetUp", RpcTarget.All, bulletId, photonView.ViewID);

            bullet.GetComponent<Bullet>().photonView.RPC("ActiveAll", RpcTarget.All, false);

            photonView.RPC("add", RpcTarget.AllBufferedViaServer, bulletId);

        }
    }
    [PunRPC]
    private void add(int id)
    {
        inactiveGroup.Add(id);
    }
    private bool HalfEmpty()
    {
        if (inactiveGroup.Count > 1)
        {
            return true;
        }
        return false;
    }

    public void In(int id)
    {
        inactiveGroup.Add(id);
        activeGroup.Remove(id);
    }
    public void Out(int id)
    {
        inactiveGroup.Remove(id);
        activeGroup.Add(id);
    }
    [PunRPC]
    public int ActiveInstance()
    {
        if (!photonView.IsMine)
            return -1;
        if (!HalfEmpty())
        {
            AddMoreElement();
        }
        return inactiveGroup[0];
    }
}
