using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] bool isAIDice;
    [SerializeField] DiceBase _base;
    [SerializeField] GameObject[] diceEyes;
    [SerializeField] GameObject silence;
    [SerializeField] int level;

    SpriteRenderer image;
    Vector3 originPos;
    List<Transform> attackPos;

    KeyValuePair<Buff, int> upBuff = new KeyValuePair<Buff, int>();
    KeyValuePair<Buff, int> downBuff = new KeyValuePair<Buff, int>();
    KeyValuePair<Buff, int> leftBuff = new KeyValuePair<Buff, int>();
    KeyValuePair<Buff, int> rightBuff = new KeyValuePair<Buff, int>();
    bool isAttacking;
    float speedBuff = 0f;
    [SerializeField] bool canAttack;
    public int Level { get => level; }
    public int Damage { get => _base.Attack + UpgradeInfo.i.Upgradeinfo[_base] * Base.AttackPerUpgrade; }
    public float Speed { get => _base.Speed * ((100 - speedBuff) / 100); }
    public int Effect { get => _base.Effect + UpgradeInfo.i.Upgradeinfo[_base] * Base.EffectPerUpgrade; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<Transform> AttackPos { get => attackPos; }
    public DiceBase Base { get => _base; }
    public float SpeedBuff { get => speedBuff; set => speedBuff = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public bool IsAIDice { get => isAIDice; set => isAIDice = value; }

    public Dice(Dice dice)
    {
        this._base = dice.Base;
    }
    public void init(DiceBase diceBase, int level = 0, bool isAIDice = false)
    {
        this._base = diceBase;
        this.level = level;
        this.IsAIDice = isAIDice;
        SetDiceEyeColor();
        SetDiceEye();
    }
    public DiceInfo GetDiceInfo()
    {
        DiceInfo diceInfo = new DiceInfo(_base, level);

        return diceInfo;
    }
    public void UpgradeDice()
    {
        level++;
        level = Mathf.Clamp(level, 0, 5);
        SetDiceEye();
    }
    void SetDiceEyeColor()
    {
        foreach(var gameobject in diceEyes)
        {
            var spriteRenderers = gameobject.GetComponentsInChildren<SpriteRenderer>();
            foreach(var diceEye in spriteRenderers)
            {
                diceEye.color = _base.DiceEyeColor;
            }
        }
    }
    void SetDiceEye()
    {
        if(Base.Attackable)
        {
            for (int i = 0; i < diceEyes.Length; i++)
            {
                if (i == level)
                {
                    diceEyes[i].SetActive(true);
                    attackPos = diceEyes[i].GetComponentsInChildren<Transform>().ToList();
                    attackPos.RemoveAt(0);
                }
                else
                {
                    diceEyes[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < diceEyes.Length; i++)
            {
                diceEyes[i].SetActive(false);
            }
        }
    }
    private void Start()
    {
        var slots = Physics2D.OverlapCircleAll(this.transform.position, 0.1f, GameLayers.i.SlotLayer);
        transform.position = slots.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault().transform.position;
        image = GetComponent<SpriteRenderer>();
        image.sprite = _base.Sprite;
        SetDiceEyeColor();
        SetDiceEye();
    }
    private void FixedUpdate()
    {
        GetBuff();
        SetBuff();
    }
    private void OnMouseDown()
    {
        if(!IsAIDice)
        {
            originPos = this.transform.position;
            this.gameObject.layer = 8;
            image.sortingOrder = 1;
        }

    }
    private void OnMouseDrag()
    {
        if(!IsAIDice)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = mousePosition;
        }

    }
    private void OnMouseUp()
    {
        if(!IsAIDice)
        {
            var slots = Physics2D.OverlapCircleAll(this.transform.position, 0.1f, GameLayers.i.SlotLayer);
            var dices = Physics2D.OverlapCircleAll(this.transform.position, 0.1f, GameLayers.i.DiceLayer);

            if (slots.Length == 0)
            {
                transform.position = originPos;
            }
            else
            {
                if (dices.Length == 0)
                {
                    transform.position = slots.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault().transform.position;
                }
                else
                {
                    var collidedDice = dices.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault().GetComponent<Dice>();
                    var diceInfo = collidedDice.GetDiceInfo();

                    // max level 예외 처리 하기
                    if (this.level == diceInfo.Level && this._base.Name == diceInfo.DiceName)
                    {
                        if (level < 4)
                            FusionDice(collidedDice, this);
                    }
                    else
                    {
                        transform.position = originPos;
                    }
                }

            }
            this.gameObject.layer = 7;
            image.sortingOrder = 0;
        }
        
    }
    public void FusionDice(Dice original, Dice newThing)
    {
        newThing.UpgradeDice();
        newThing.transform.position = original.transform.position;
        Destroy(original.gameObject);
    }
    void GetBuff()
    {
        if (this.gameObject.layer == 7)
        {
            var upDiceCheck = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector3.up, 0.2f, GameLayers.i.DiceLayer);
            if (upDiceCheck)
            {
                var upDice = upDiceCheck.collider.GetComponent<Dice>();
                if (!upDice.Base.Attackable && !upDice.CanAttack)
                {
                    upBuff = new KeyValuePair<Buff, int>(upDice.Base.Buff, upDice.Base.Effect);
                }
            }
            else
            {
                upBuff = new KeyValuePair<Buff, int>();
            }
            var downDiceCheck = Physics2D.Raycast(transform.position + Vector3.down * 0.5f, Vector3.down, 0.2f, GameLayers.i.DiceLayer);
            if (downDiceCheck)
            {
                var downDice = downDiceCheck.collider.GetComponent<Dice>();
                if (!downDice.Base.Attackable && !downDice.canAttack)
                {
                    downBuff = new KeyValuePair<Buff, int>(downDice.Base.Buff, downDice.Base.Effect);
                }
            }
            else
            {
                downBuff = new KeyValuePair<Buff, int>();
            }
            var leftDiceCheck = Physics2D.Raycast(transform.position + Vector3.left * 0.5f, Vector3.left, 0.2f, GameLayers.i.DiceLayer);
            if (leftDiceCheck)
            {
                var leftDice = leftDiceCheck.collider.GetComponent<Dice>();
                if (!leftDice.Base.Attackable && !leftDice.CanAttack)
                {
                    leftBuff = new KeyValuePair<Buff, int>(leftDice.Base.Buff, leftDice.Base.Effect);
                }
            }
            else
            {
                leftBuff = new KeyValuePair<Buff, int>();
            }
            var rightDiceCheck = Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector3.right, 0.2f, GameLayers.i.DiceLayer);
            if (rightDiceCheck)
            {
                var rightDice = rightDiceCheck.collider.GetComponent<Dice>();
                if (!rightDice.Base.Attackable && !rightDice.CanAttack)
                {
                    rightBuff = new KeyValuePair<Buff, int>(rightDice.Base.Buff, rightDice.Base.Effect);
                }
            }
            else
            {
                rightBuff = new KeyValuePair<Buff, int>();
            }
        }
    }
    void SetBuff()
    {
        speedBuff = 0f;
        if(upBuff.Key == Buff.Speed)
        {
            speedBuff += upBuff.Value;
        }
        if (downBuff.Key == Buff.Speed)
        {
            speedBuff += downBuff.Value;
        }
        if (leftBuff.Key == Buff.Speed)
        {
            speedBuff += leftBuff.Value;
        }
        if (rightBuff.Key == Buff.Speed)
        {
            speedBuff += rightBuff.Value;
        }
    }
    public void SetSilence(bool check)
    {
        CanAttack = check;
        silence.SetActive(check);
    }
}
public class DiceInfo
{
    string diceName;
    int level;
    public DiceInfo(DiceBase diceBase, int level)
    {
        this.diceName = diceBase.Name;
        this.level = level;
    }

    public string DiceName { get => diceName;  }
    public int Level { get => level; }
}