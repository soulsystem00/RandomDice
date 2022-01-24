using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int money;
    [SerializeField] int heart;
    int round = 1;
    public static GameManager i { get; set; }
    private void Awake()
    {
        i = this;
    }
    public void Init()
    {
        var dices = FindObjectsOfType<Dice>();
        var enemis = FindObjectsOfType<Enemy>();
        foreach(var dice in dices)
        {
            Destroy(dice.gameObject);
        }
        foreach(var enemy in enemis)
        {
            Destroy(enemy.gameObject);
        }
        money = 100;
        heart = 3;
        round = 1;
    }
    public int Money { get => money; set => money = value; }
    public int Heart { get => heart; set => heart = value; }
    public int Round { get => round; set => round = value; }
}
