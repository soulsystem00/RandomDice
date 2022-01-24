using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [SerializeField] Slot[] slots;
    [SerializeField] Text costText;
    public event Action OnHeartChanged;
    int money = 100;
    int heart = 3;
    int cost = 10;
    public static AIController i { get; set; }
    List<GameObject> aiDice = new List<GameObject>();
    public int Money { get => money; set => money = value; }
    public int Heart { get => heart; set => heart = value; }

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        StartAIFunction();
    }
    public void Init()
    {
        StartAIFunction();
        money = 100;
        heart = 3;
        cost = 10;
        OnHeartChanged?.Invoke();
    }
    public void DiscountHeart()
    {
        Heart--;
        Heart = Mathf.Clamp(Heart, 0, 3);
        OnHeartChanged?.Invoke();
        if (Heart == 0)
        {
            StopAIFunction();
            GameController.i.GameOver();
        }
    }
    public void Spawn(DiceBase diceBase)
    {
        var unUsedSlots = slots.Where(x => x.IsUsed == false).ToList();
        if (unUsedSlots.Count > 0)
        {
            var rnd = new System.Random();
            var randomizedSlot = unUsedSlots.OrderBy(x => rnd.Next()).FirstOrDefault();
            randomizedSlot.GetComponent<Slot>().IsUsed = true;

            var initObj = GameController.i.Dices.Where(x => x.GetComponent<Dice>().Base == diceBase).FirstOrDefault();
            var newDice = Instantiate(initObj, randomizedSlot.transform.position, Quaternion.identity);
            var tmp = newDice.GetComponent<Dice>();
            tmp.IsAIDice = true;
            aiDice.Add(initObj);
        }
    }
    void Spawn()
    {
        if (Money >= cost)
        {
            var unUsedSlots = slots.Where(x => x.IsUsed == false).ToList();
            if (unUsedSlots.Count > 0)
            {
                var rnd = new System.Random();
                var randomizedSlot = unUsedSlots.OrderBy(x => rnd.Next()).FirstOrDefault();
                var randomizedDice = aiDice.OrderBy(x => rnd.Next()).FirstOrDefault();
                var newDice = Instantiate(randomizedDice, randomizedSlot.transform.position, Quaternion.identity).GetComponent<Dice>();
                newDice.IsAIDice = true;
            }
            cost += 10;
            costText.text = cost.ToString();
        }
    }
    void StartAIFunction()
    {
        InvokeRepeating("SpawnAI", 1f, 1f);
        InvokeRepeating("FusionAI", 2f, 2f);
        InvokeRepeating("UpgradeAI", 3f, 3f);
    }
    void StopAIFunction()
    {
        CancelInvoke("SpawnAI");
        CancelInvoke("FusionAI");
        CancelInvoke("UpgradeAI");
    }
    void SpawnAI()
    {
        if(money >= cost)
        {
            Spawn();
        }
    }
    void FusionAI()
    {
        var dice = FindObjectsOfType<Dice>().Where(x => x.IsAIDice).ToList();
        var tmp = dice.OrderBy(x => x.GetDiceInfo().DiceName).ThenBy(x=>x.GetDiceInfo().Level).ToList();
        if(tmp!= null)
        {
            for (int i = 0; i < tmp.Count - 1; i++)
            {
                if(tmp[i].GetDiceInfo().DiceName == tmp[i+1].GetDiceInfo().DiceName && tmp[i].GetDiceInfo().Level == tmp[i + 1].GetDiceInfo().Level)
                {
                    tmp[i].FusionDice(tmp[i], tmp[i + 1]);
                    break;
                }
            }
        }
        
        
    }
    void UpgradeAI()
    {
        var dice = FindObjectsOfType<Dice>().Where(x => x.IsAIDice).ToList();
        var tmp = dice.GroupBy(x => x.Base).Select(n => new { metricname = n, metricCount = n.Count() }).OrderByDescending(n => n.metricCount).ToList();
        foreach (var item in tmp)
        {
            var upgradeCost = UpgradeInfo.i.Upgradeinfo[item.metricname.Key] * 100 + 100;
            if (money >= upgradeCost)
            {
                money -= upgradeCost;
                UpgradeInfo.i.Upgradeinfo[item.metricname.Key]++;
                break;
            }
        }
    }
}
