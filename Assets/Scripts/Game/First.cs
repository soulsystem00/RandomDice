using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class First : MonoBehaviour
{
    [SerializeField] Button Dice1;
    [SerializeField] Button Dice2;
    [SerializeField] Text dice1Text;
    [SerializeField] Text dice2Text;
    Image image1;
    Image image2;
    public int index = 0;

    private void Awake()
    {
        Dice1.onClick.AddListener(SetDice1);
        Dice2.onClick.AddListener(SetDice2);
        image1 = Dice1.GetComponent<Image>();
        image2 = Dice2.GetComponent<Image>();
    }
    public void Init()
    {
        image1.sprite = GameController.i.Dices[0].GetComponent<Dice>().Base.Sprite;
        dice1Text.text = GameController.i.Dices[0].GetComponent<Dice>().Base.Name;

        image2.sprite = GameController.i.Dices[1].GetComponent<Dice>().Base.Sprite;
        dice2Text.text = GameController.i.Dices[1].GetComponent<Dice>().Base.Name;
    }
    void SetDice1()
    {
        GameController.i.DiceSpawner.Spawn(GameController.i.Dices[index].GetComponent<Dice>().Base);
        AIController.i.Spawn(GameController.i.Dices[index + 1].GetComponent<Dice>().Base);
        index = index + 2;
        if(index >= 10)
        {
            index = 0;
            GameController.i.GameStart();
        }
        else
        {
            SetDiceImage(index);
        }
        
    }
    void SetDice2()
    {
        GameController.i.DiceSpawner.Spawn(GameController.i.Dices[index + 1].GetComponent<Dice>().Base);
        AIController.i.Spawn(GameController.i.Dices[index].GetComponent<Dice>().Base);
        index = index + 2;
        if (index >= 10)
        {
            index = 0;
            GameController.i.GameStart();
        }
        else
        {
            SetDiceImage(index);
        }
    }
    void SetDiceImage(int index)
    {
        image1.sprite = GameController.i.Dices[index].GetComponent<Dice>().Base.Sprite;
        dice1Text.text = GameController.i.Dices[index].GetComponent<Dice>().Base.Name;

        image2.sprite = GameController.i.Dices[index + 1].GetComponent<Dice>().Base.Sprite;
        dice2Text.text = GameController.i.Dices[index + 1].GetComponent<Dice>().Base.Name;
    }
}
