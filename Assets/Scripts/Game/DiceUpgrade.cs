using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUpgrade : MonoBehaviour
{
    Dice dice;
    [SerializeField] Image image;
    [SerializeField] Text lvText;
    [SerializeField] Text spText;
    int level = 0;
    int sp = 100;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Upgrade);
    }
    public void SetDice(Dice dice)
    {
        this.dice = dice;
        image.sprite = dice.Base.Sprite;
        Init();
    }
    void Init()
    {
        level = UpgradeInfo.i.Upgradeinfo[dice.Base];
        sp = 100;
        SetText();
    }
    void SetText()
    {
        lvText.text = "LV." + (level + 1).ToString();
        spText.text = sp.ToString();
    }
    private void Upgrade()
    {
        if(level < 4)
        {
            sp *= 2;
            level++;
            UpgradeInfo.i.Upgradeinfo[dice.Base] = level;
            SetText();
        }
        if(level == 4)
        {
            lvText.text = "LV." + "Max";
            spText.text = "Max";
        }
    }

}
