using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss_2 : MonoBehaviour
{
    [SerializeField] GameObject effect;
    Enemy enemy;
    EnemySpawner spawner;
    float originalSpeed;
    List<Dice> effectedDices;
    private void Start()
    {
        effectedDices = new List<Dice>();
        enemy = GetComponent<Enemy>();
        spawner = FindObjectOfType<EnemySpawner>();
        StartCoroutine(BossSkill());
        var asdf = FindObjectOfType<GameController>();
        spawner.bossdead = () =>
        {
            foreach (var item in effectedDices)
            {
                item.SetSilence(false);
            }
        };
    }
    IEnumerator BossSkill()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            effect.SetActive(true);
            originalSpeed = enemy.Speed;
            enemy.Speed = 0f;
            var rnd = new System.Random();
            var randomized = FindObjectsOfType<Dice>().Where(x => x.CanAttack == false).OrderBy(x => rnd.Next()).Take(2).ToList();
            foreach (var item in randomized)
            {
                item.SetSilence(true);
                effectedDices.Add(item);
            }
            yield return new WaitForSeconds(1.5f);
            enemy.Speed = originalSpeed;
            yield return new WaitForSeconds(10f);
        }
    }
    private void OnDisable()
    {
        spawner.BossDie();
    }
}
