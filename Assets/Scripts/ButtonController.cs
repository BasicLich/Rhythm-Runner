using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private string keyToPress;

    public Sprite pressedImage;
    public SpriteRenderer sr;
    public bool canBePressed;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void Update()
    {
        if (Input.GetButtonDown(keyToPress))
        {
            if (canBePressed)
            {
                sr.sprite = pressedImage;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitpoint"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitpoint"))
        {
            canBePressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (gm.playerHit) {
            return;
        }
        transform.position -= new Vector3(gm.currentbeatTempo * Time.deltaTime, 0f, 0f);
    }
}
