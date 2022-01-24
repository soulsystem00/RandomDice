using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] bool isAI;
    [SerializeField] Heart[] hearts;
    private void Start()
    {
        if(isAI)
        {
            AIController.i.OnHeartChanged += ChangeHearts;
        }
        else
        {
            GameController.i.OnHeartChanged += ChangeHearts;
        }
        
    }
    // Update is called once per frame
    void ChangeHearts()
    {
        if(isAI)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < AIController.i.Heart)
                {
                    hearts[i].EnableHeart();
                }
                else
                {
                    hearts[i].DisableHeart();
                }
            }
        }
        else
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < GameManager.i.Heart)
                {
                    hearts[i].EnableHeart();
                }
                else
                {
                    hearts[i].DisableHeart();
                }
            }
        }
    }
}
