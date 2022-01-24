using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : MonoBehaviour
{
    Enemy enemy;
    EnemySpawner[] spawners;
    EnemySpawner spawner;
    float originalSpeed;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        spawners = FindObjectsOfType<EnemySpawner>();
        foreach (var item in spawners)
        {
            if(item.IsAIPart == enemy.IsAIEnemy)
            {
                spawner = item;
            }
        }
        StartCoroutine(BossSkill());
    }
    IEnumerator BossSkill()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            originalSpeed = enemy.Speed;
            enemy.Speed = 0f;
            spawner.Spawn((int)(enemy.Health * 0.1));
            yield return new WaitForSeconds(0.2f);
            spawner.Spawn((int)(enemy.Health * 0.1));
            yield return new WaitForSeconds(1.2f);
            enemy.Speed = originalSpeed;
            yield return new WaitForSeconds(8.6f);
            
        }
    }
    private void OnDisable()
    {
        spawner.BossDie();
    }
}
