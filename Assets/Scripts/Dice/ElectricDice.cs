using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectricDice : MonoBehaviour, IAttack
{
    [SerializeField] GameObject effect;
    Dice dice;

    private void Awake()
    {
        dice = GetComponent<Dice>();
    }
    private void Update()
    {
        if (!dice.IsAIDice)
        {
            if (EnemyList.i.Enemies.Count > 0 && !dice.IsAttacking)
            {
                if (!dice.CanAttack)
                    StartCoroutine(Attack());
            }
        }
        else
        {
            if (AIEnemyList.i.Enemies.Count > 0 && !dice.IsAttacking)
            {
                if (!dice.CanAttack)
                    StartCoroutine(Attack());
            }
        }
    }
    public IEnumerator Attack()
    {
        dice.IsAttacking = true;
        foreach (var attack in dice.AttackPos)
        {
            // 공격 효과 추가
            var bullet = Instantiate(dice.Base._Bullet, attack.transform.position, Quaternion.identity);
            bullet.GetComponent<SpriteRenderer>().color = dice.Base.DiceEyeColor;
            // 타겟 설정
            List<Enemy> target = new List<Enemy>();
            if (!dice.IsAIDice)
            {
                target = EnemyList.i.Enemies.OrderByDescending(x => x.Dist).Take(3).ToList();
            }
            else
            {
                target = AIEnemyList.i.Enemies.OrderByDescending(x => x.Dist).Take(3).ToList();
            }
            
            bullet.Init(dice.Damage, target[0], () =>
            {
                for (int i = 0; i < target.Count; i++)
                {
                    target[i].GetElectric(dice.Effect);
                    if(i == 1)
                    {
                        if(target[0] != null && target[1] != null)
                        {
                            var e = Instantiate(effect).GetComponent<LightingEffect2>();
                            e.init(target[0].gameObject, target[1].gameObject);
                            target[1].GetDamage(dice.Damage * 0.7f);
                        }
                    }
                    else if(i == 2)
                    {
                        if(target[1] != null && target[2] != null)
                        {
                            var e = Instantiate(effect).GetComponent<LightingEffect2>();
                            e.init(target[1].gameObject, target[2].gameObject);
                            target[2].GetDamage(dice.Damage * 0.3f);
                        }
                        
                    }
                }
            });
        }
        yield return new WaitForSeconds(dice.Base.Speed);
        dice.IsAttacking = false;
    }
}
