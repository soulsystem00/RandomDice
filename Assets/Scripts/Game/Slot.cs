using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] bool isUsed;

    public bool IsUsed { get => isUsed; set => isUsed = value; }

    private void FixedUpdate()
    {
        isUsed = (Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.DiceLayer) != null);
    }
}
