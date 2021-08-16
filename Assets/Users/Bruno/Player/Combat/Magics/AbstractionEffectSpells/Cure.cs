﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cure : Spells, IEffect
{
    public override void Aim()
    {
        base.Aim();
        transform.position = Camera.main.WorldToScreenPoint(Input.mousePosition);
    }
    public override void Use()
    {
    }
    [PunRPC]
    public void Apply(Animator animator)
    {
        this.Use();
        this.animator = animator;
        this.animator.Play("Active");
    }
}
