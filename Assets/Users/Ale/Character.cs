﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    public GameObject characterPrefab;
    public string characterName, frase, Descricao;
    public int characterIndex, damage, ammo;
    public float HP;
    public Sprite characterIcon;
    public Sprite[] ordenedHabillityIcon;
    public string[] ordenedHabillityName, ordenedHabillityDescription;
 

}