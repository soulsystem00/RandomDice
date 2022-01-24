using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestoryer : MonoBehaviour
{
    private void Update()
    {
        var tmp = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.EnemyLayer);
        if(tmp != null)
        {
            EnemyList.i.Enemies.Remove(tmp.GetComponent<Enemy>());
            Destroy(tmp.gameObject);
            GameController.i.DiscountHeart();
        }
    }
}
