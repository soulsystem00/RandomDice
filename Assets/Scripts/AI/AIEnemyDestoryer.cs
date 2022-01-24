using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyDestoryer : MonoBehaviour
{
    private void Update()
    {
        var tmp = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.EnemyLayer);
        if (tmp != null)
        {
            AIEnemyList.i.Enemies.Remove(tmp.GetComponent<Enemy>());
            Destroy(tmp.gameObject);
            AIController.i.DiscountHeart();
        }
    }
}
