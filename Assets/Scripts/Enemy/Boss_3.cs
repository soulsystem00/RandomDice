using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss_3 : MonoBehaviour
{
    Enemy enemy;
    EnemySpawner spawner;
    DiceSpawner diceSpawner;
    float originalSpeed;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        spawner = FindObjectOfType<EnemySpawner>();
        diceSpawner = FindObjectOfType<DiceSpawner>();
        StartCoroutine(BossSkill());
    }
    IEnumerator BossSkill()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            originalSpeed = enemy.Speed;
            enemy.Speed = 0f;
            yield return new WaitForSeconds(1f);
            enemy.Speed = originalSpeed;
            var rnd = new System.Random();
            var randomized = FindObjectsOfType<Dice>().OrderBy(x => rnd.Next()).FirstOrDefault();
            var randomizedDice = GameController.i.MyDice.OrderBy(x => rnd.Next()).FirstOrDefault();
            var newDice = Instantiate(randomizedDice, randomized.transform.position, Quaternion.identity);
            randomized.init(randomizedDice.GetComponent<Dice>().Base,randomized.Level);
            Destroy(randomized.gameObject);
            yield return new WaitForSeconds(10f);
        }
    }
    private void OnDisable()
    {
        spawner.BossDie();
    }
}
