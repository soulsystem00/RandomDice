using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public enum State { Poison, Ice, }
public class Enemy : MonoBehaviour
{
    Transform[] pos;
    [SerializeField] bool isAIEnemy;
    [SerializeField] TextMesh text;
    [SerializeField] float speed;
    [SerializeField] GameObject poison;
    [SerializeField] GameObject[] ice;
    [SerializeField] GameObject electric;
    [SerializeField] GameObject _lock;
    [SerializeField] bool isBoss;
    DiceBase diceBase;
    int health = 100;
    int condition = 0;
    float dist = 0f;
    bool isLocked = false;  
    public int Health { get => health; }
    List<int> iceList = new List<int>();
    public float Dist { get => dist; }
    public bool IsBoss { get => isBoss; set => isBoss = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsAIEnemy { get => isAIEnemy; set => isAIEnemy = value; }

    private void Start()
    {
        //pos = GlobalSettings.i.Points;
    }
    public void Init(DiceBase diceBase, int health, bool isBoss = false, bool isAIenemy = false)
    {
        this.isAIEnemy = isAIenemy;
        this.health += health;
        if(!isAIEnemy)
        {
            pos = GlobalSettings.i.Points;
        }
        else
        {
            pos = GlobalSettings.i.AIPoints;
        }
        
        this.diceBase = diceBase;
        GetComponent<SpriteRenderer>().sprite = diceBase.Sprite;
        this.IsBoss = isBoss;
    }
    private void Update()
    {
        dist += speed * Time.deltaTime;
        text.text = health.ToString();
        if(condition == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos[1].position, speed * Time.deltaTime);
        }
        else if(condition == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos[2].position, speed * Time.deltaTime);
        }
        else if(condition == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos[3].position, speed * Time.deltaTime);
        }

        if((Vector2)transform.position == (Vector2)pos[1].position)
        {
            condition++;
        }
        else if ((Vector2)transform.position == (Vector2)pos[2].position)
        {
            condition++;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
    public void GetDamage(float damage)
    {
        health -= (int)damage;
        if (health <= 0)
        {
            health = 0;
            if(isAIEnemy)
            {
                AIEnemyList.i.Enemies.Remove(this);
                AIController.i.Money += 10;
            }
            else
            {
                EnemyList.i.Enemies.Remove(this);
                GameManager.i.Money += 10;
            }
            
            Destroy(this.gameObject);
        }
    }
    public void GetLock(int percent, DiceBase dice)
    {
        var tmp = Random.Range(1, 101);
        if(tmp <= percent && !isLocked)
        {
            StartCoroutine(Lock(UpgradeInfo.i.Upgradeinfo[dice] * 0.5f));
        }
    }
    public void GetElectric(int damage)
    {
        if(electric != null)
        {
            electric.SetActive(true);
        }
    }
    public void GetFire(int damage)
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, 0.7f, GameLayers.i.EnemyLayer).Where(x => x.gameObject != this.gameObject);
        foreach(var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().GetDamage(damage);
        }
    }
    public void GetIce(int percent)
    {
        iceList.Add(percent);
        while(iceList.Count >= 4)
        {
            iceList = iceList.OrderBy(x => x).ToList();
            iceList.RemoveAt(0);
        }
        for (int i = 0; i < ice.Length; i++)
        {
            if(i == iceList.Count - 1)
            {
                ice[i].SetActive(true);
            }
            else
            {
                ice[i].SetActive(false);
            }
        }
        int sum = iceList.Sum();
        sum = Mathf.Clamp(sum, 0, 30);
        speed = speed * ((float)(100 - sum) / 100);
    }
    public void GetPoison(int damage)
    {
        poison.SetActive(true);
        StartCoroutine(Poison(damage));
    }
    IEnumerator Poison(int damage)
    {
        while (true)
        {
            GetDamage(damage);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Lock(float time)
    {
        isLocked = true;
        _lock.SetActive(true);
        var originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(3f + time);
        speed = originalSpeed;
        _lock.SetActive(false);
    }
}
