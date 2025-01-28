using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class changesprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    int secondsToWait = 0;
    [SerializeField]
    private Sprite playerLoaded;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(changeSprite());
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            spriteRenderer.sprite = playerLoaded;
        }
    }

    // Update is called once per frame

    IEnumerator changeSprite()
    {
        yield return new WaitForSeconds(secondsToWait);

        spriteRenderer.sprite = playerLoaded;


    }
}
