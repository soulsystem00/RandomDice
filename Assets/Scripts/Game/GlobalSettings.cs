using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    [SerializeField] Transform[] points;
    [SerializeField] Transform[] aIPoints;
    [SerializeField] float bossTime;
    public int BulletSpeed { get => bulletSpeed; }
    public static GlobalSettings i { get; set; }
    public Transform[] Points { get => points; }
    public float BossTime { get => bossTime; set => bossTime = value; }
    public Transform[] AIPoints { get => aIPoints; }

    private void Awake()
    {
        i = this;
    }
}
