using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] Sprite heartOn;
    [SerializeField] Sprite heartOff;

    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void EnableHeart()
    {
        image.sprite = heartOn;
    }
    public void DisableHeart()
    {
        image.sprite = heartOff;
    }
}
