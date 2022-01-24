using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice/Create Dice Base")]
[System.Serializable]
public class DiceBase : ScriptableObject
{
    [SerializeField] string _name;
    
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite sprite;

    [Header("Stats")]
    [SerializeField] bool attackable;
    [SerializeField] Buff buff;
    [SerializeField] int attack;
    [SerializeField] int attackPerUpgrade;
    [SerializeField] int effect;
    [SerializeField] int effectPerUpgrade;
    [SerializeField] float speed;

    [SerializeField] Color diceEyeColor;
    [SerializeField] Bullet bullet;

    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite Sprite { get => sprite; }
    public int Attack { get => attack; }
    public float Speed { get => speed; }
    public Color DiceEyeColor { get => diceEyeColor; }
    public Bullet _Bullet { get => bullet; }
    public int Effect { get => effect; }
    public bool Attackable { get => attackable; }
    public Buff Buff { get => buff; }
    public int AttackPerUpgrade { get => attackPerUpgrade; }
    public int EffectPerUpgrade { get => effectPerUpgrade; }
}
[System.Serializable]
public enum Buff
{
    None,
    Speed,
    Critical,
}