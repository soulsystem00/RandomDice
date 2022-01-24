using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrokenDice : MonoBehaviour, IAttack
{
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
            var rnd = new System.Random();
            if(!dice.IsAIDice)
            {
                var randomized = EnemyList.i.Enemies.OrderBy(x => rnd.Next()).FirstOrDefault();
                bullet.Init(dice.Damage, randomized);
            }
            else
            {
                var randomized = AIEnemyList.i.Enemies.OrderBy(x => rnd.Next()).FirstOrDefault();
                bullet.Init(dice.Damage, randomized);
            }
            
        }
        yield return new WaitForSeconds(dice.Base.Speed);
        dice.IsAttacking = false;
    }
}
