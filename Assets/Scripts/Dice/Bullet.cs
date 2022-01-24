using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    int speed;

    int damage;
    SpriteRenderer sprite;
    Enemy enemy;
    bool hit;
    Action afterEffect;
    public void Init(int damage, Enemy enemy, Action AfterEffect = null)
    {
        this.speed = GlobalSettings.i.BulletSpeed;
        this.damage = damage;
        this.enemy = enemy;
        this.afterEffect = AfterEffect;
    }
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[0];
    }
    private void Update()
    {
        if(enemy != null)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, enemy.transform.position, speed * Time.deltaTime);

            var tmp = Physics2D.OverlapCircleAll(transform.position, 0.1f, GameLayers.i.EnemyLayer).Where(x => x.gameObject == enemy.gameObject).FirstOrDefault();
            hit = (tmp != null) ? true : false;
            if (hit)
            {
                enemy.GetComponent<Enemy>().GetDamage(damage);
                afterEffect?.Invoke();
                StartCoroutine(PlayAnimation());
                hit = false;
                enemy = null;
            }
        }
        else
        {
            transform.position = transform.position;
            StartCoroutine(PlayAnimation());
        }
    }
    IEnumerator PlayAnimation()
    {
        while(sprites.Count != 0)
        {
            sprite.sprite = sprites[0];
            sprites.RemoveAt(0);
            yield return new WaitForSeconds(0.1f);
            Destroy(this.gameObject);
        }
    }
}
