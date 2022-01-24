using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceDice : MonoBehaviour, IAttack
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

            // 타겟 설정
            if(!dice.IsAIDice)
            {
                var target = EnemyList.i.Enemies.OrderByDescending(x => x.Dist).FirstOrDefault();
                bullet.Init(dice.Damage, target, () =>
                {
                    target.GetIce(dice.Effect);
                });
            }
            else
            {
                var target = AIEnemyList.i.Enemies.OrderByDescending(x => x.Dist).FirstOrDefault();
                bullet.Init(dice.Damage, target, () =>
                {
                    target.GetIce(dice.Effect);
                });
            }
            
        }
        yield return new WaitForSeconds(dice.Base.Speed);
        dice.IsAttacking = false;
    }
}
