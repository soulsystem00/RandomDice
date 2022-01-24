using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Effect : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sprite;
    int index = 0;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        index = 0;
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        while (index < sprites.Length)
        {
            sprite.sprite = sprites[index];
            index++;
            yield return new WaitForSeconds(0.1f);
        }
        this.gameObject.SetActive(false);
    }
}
