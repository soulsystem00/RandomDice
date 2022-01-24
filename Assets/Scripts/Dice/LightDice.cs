using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightDice : MonoBehaviour
{
    Dice dice;

    private void Awake()
    {
        dice = GetComponent<Dice>();
    }
}
