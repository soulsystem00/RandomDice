using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyList : MonoBehaviour
{
    List<Enemy> enemies;

    public static AIEnemyList i { get; set; }
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }

    private void Awake()
    {
        i = this;
        enemies = new List<Enemy>();
    }
    public void Init()
    {
        enemies = new List<Enemy>();
    }
}
