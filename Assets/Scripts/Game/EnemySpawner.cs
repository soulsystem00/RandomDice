using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public enum SpawnerState { Run, Boss, Wait}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] bool isAIPart;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject Boss2;
    [SerializeField] GameObject Boss3;
    [SerializeField] DiceBase diceBase;
    [SerializeField] DiceBase bossBase;
    [SerializeField] DiceBase bossBase2;
    [SerializeField] DiceBase bossBase3;
    [SerializeField] Text timeText;
    [SerializeField] Text roundText;
    [SerializeField] int time;

    public Action bossdead;
    float count = 0;
    float bossHealth = 0f;
    SpawnerState state = SpawnerState.Run;

    public bool IsAIPart { get => isAIPart; set => isAIPart = value; }

    private void Start()
    {
        GameController.i.OnGameStart += SpawnRepeating;
        GameController.i.OnGameOver += StopSpawn;
    }
    private void Update()
    {
        roundText.text = "Round " + GameManager.i.Round;
        if (state == SpawnerState.Run)
        {
            timeText.text = Mathf.Round(count).ToString();
            if (count >= GlobalSettings.i.BossTime)
            {
                count = 0f;
                GameManager.i.Round++;
                SpawnBoss();
            }
        }
    }
    private void FixedUpdate()
    {
        if (state == SpawnerState.Run)
        {
            count += Time.deltaTime;
        }
            
    }
    void SpawnRepeating()
    {
        InvokeRepeating("Spawn", time, time);
    }
    void StopSpawn()
    {
        CancelInvoke("Spawn");
    }
    public void Spawn()
    {
        var obj = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
        obj.Init(diceBase, (int)count, default, isAIPart);
        obj.name += count;
        count++;
        if (!isAIPart)
            EnemyList.i.Enemies.Add(obj);
        else
            AIEnemyList.i.Enemies.Add(obj);
    }
    public void Spawn(int health)
    {
        var obj = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
        obj.Init(diceBase, (int)health, default, isAIPart);
        obj.name += count;
        count++;
        if (!isAIPart)
            EnemyList.i.Enemies.Add(obj);
        else
            AIEnemyList.i.Enemies.Add(obj);
    }
    void SpawnBoss()
    {
        bossHealth = 0f;
        if(!isAIPart)
        {
            EnemyList.i.Enemies.ForEach(x => { bossHealth += x.Health; });
            //EnemyList.i.Enemies.ForEach(x => Destroy(x.gameObject));
        }
        else
        {
            AIEnemyList.i.Enemies.ForEach(x => { bossHealth += x.Health; });
            //AIEnemyList.i.Enemies.ForEach(x => Destroy(x.gameObject));
        }
        if(GameManager.i.Round <= 5)
        {
            var obj = Instantiate(Boss, transform.position, Quaternion.identity).GetComponent<Enemy>();
            obj.Init(bossBase, 5000 * GameManager.i.Round + (int)(bossHealth * 0.5), true,isAIPart);
            if (!isAIPart)
                EnemyList.i.Enemies.Add(obj);
            else
                AIEnemyList.i.Enemies.Add(obj);
        }
        else if(GameManager.i.Round <= 10)
        {
            var obj = Instantiate(Boss2, transform.position, Quaternion.identity).GetComponent<Enemy>();
            obj.Init(bossBase2, 5000 * GameManager.i.Round + (int)(bossHealth * 0.5), true, isAIPart);
            if (!isAIPart)
                EnemyList.i.Enemies.Add(obj);
            else
                AIEnemyList.i.Enemies.Add(obj);
        }
        else if(GameManager.i.Round > 10)
        {
            var obj = Instantiate(Boss3, transform.position, Quaternion.identity).GetComponent<Enemy>();
            obj.Init(bossBase3, 5000 * GameManager.i.Round + (int)(bossHealth * 0.5), true, isAIPart);
            if (!isAIPart)
                EnemyList.i.Enemies.Add(obj);
            else
                AIEnemyList.i.Enemies.Add(obj);
        }
        
    }
    public void BossDie()
    {
        count = 0f;
        bossdead?.Invoke();
    }
    void StageStart()
    {
        count = 0f;
        state = SpawnerState.Run;
        SpawnRepeating();
    }
}
