using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask diceLayer;
    [SerializeField] LayerMask slotLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask bossLayer;
    public static GameLayers i { get; set; }
    private void Awake()
    {
        i = this;
    }
    public LayerMask DiceLayer { get => diceLayer; }
    public LayerMask SlotLayer { get => slotLayer; }
    public LayerMask EnemyLayer { get => enemyLayer | bossLayer; }
}
