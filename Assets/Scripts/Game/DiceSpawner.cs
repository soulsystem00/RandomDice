using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class DiceSpawner : MonoBehaviour
{
    [SerializeField] Slot[] slots;
    [SerializeField] Text costText;
    int cost = 10;
    private void Start()
    {
        costText.text = cost.ToString();
        GetComponent<Button>().onClick.AddListener(Spawn);
    }
    void Spawn()
    {
        if(GameManager.i.Money >= cost)
        {
            var unUsedSlots = slots.Where(x => x.IsUsed == false).ToList();
            if (unUsedSlots.Count > 0)
            {
                var rnd = new System.Random();
                var randomizedSlot = unUsedSlots.OrderBy(x => rnd.Next()).FirstOrDefault();
                var randomizedDice = GameController.i.MyDice.OrderBy(x => rnd.Next()).FirstOrDefault();
                var newDice = Instantiate(randomizedDice, randomizedSlot.transform.position, Quaternion.identity);
            }
            cost += 10;
            costText.text = cost.ToString();
        }
        
    }
    public void Spawn(DiceBase dice)
    {
        var unUsedSlots = slots.Where(x => x.IsUsed == false).ToList();
        if (unUsedSlots.Count > 0)
        {
            var rnd = new System.Random();
            var randomizedSlot = unUsedSlots.OrderBy(x => rnd.Next()).FirstOrDefault();
            randomizedSlot.GetComponent<Slot>().IsUsed = true;
            var initObj = GameController.i.Dices.Where(x => x.GetComponent<Dice>().Base == dice).FirstOrDefault();
            var newDice = Instantiate(initObj, randomizedSlot.transform.position, Quaternion.identity);
            var tmp = newDice.GetComponent<Dice>();
            GameController.i.MyDice.Add(initObj);
        }
    }
}
