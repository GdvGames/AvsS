using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Minion")]
public class Minion : ScriptableObject
{
    public string minionName;
    public string description;

    public int health;
    public int givenExp;
    public int givenPoints;
    public int minionDamage;

    public Sprite demonSprite;
    public ParticleSystem popFX;
    public ParticleSystem bounceFX;

    public Color minionColor;

    public GameObject[] minionDrop;
}
