using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] Vector2[] pos;
    [SerializeField] TextMesh text;
    DiceBase diceBase;
    int health = 1000;
    [SerializeField] int speed;

    int condition = 0;
    float dest = 0f;
    public void Init(DiceBase diceBase)
    {
        this.diceBase = diceBase;
        GetComponent<SpriteRenderer>().sprite = diceBase.Sprite;
    }
    private void Update()
    {
        dest += speed * Time.deltaTime;
        if (condition == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos[1], speed * Time.deltaTime);
        }
        else if (condition == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos[2], speed * Time.deltaTime);
        }
        else if (condition == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos[3], speed * Time.deltaTime);
        }

        if ((Vector2)transform.position == pos[1])
        {
            condition++;
        }
        else if ((Vector2)transform.position == pos[2])
        {
            condition++;
        }
    }
    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(this.gameObject);
        }

    }
}
