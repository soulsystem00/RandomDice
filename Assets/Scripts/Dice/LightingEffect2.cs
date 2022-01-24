using System.Collections;
using UnityEngine;

public class LightingEffect2 : MonoBehaviour
{
    GameObject first;
    GameObject second;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sprite;
    int index = 0;
    public void init(GameObject a, GameObject b)
    {
        sprite = GetComponent<SpriteRenderer>();
        index = 0;

        first = a;
        second = b;
        StartCoroutine(PlayAnimaiton());
    }
    IEnumerator PlayAnimaiton()
    {
        while (index < sprites.Length)
        {
            sprite.sprite = sprites[index];
            index++;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if(first != null && second != null)
        {
            transform.position = (first.transform.position + second.transform.position) / 2;
            Vector3 dir = second.transform.position - first.transform.position;
            var abc = Vector3.Distance(first.transform.position, second.transform.position);
            var tmp = Mathf.Rad2Deg * (Mathf.Atan2(dir.normalized.y, dir.normalized.x));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, tmp));
            transform.localScale = new Vector3(abc, 1, 1);
            //Debug.Log(Vector3.Distance(first.transform.position, second.transform.position));
        }

    }
}
