using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] bool isAIMoney;
    [SerializeField] Text text;
    // Update is called once per frame
    void Update()
    {
        if (!isAIMoney)
            text.text = GameManager.i.Money.ToString();
        else
            text.text = AIController.i.Money.ToString();
    }
}
