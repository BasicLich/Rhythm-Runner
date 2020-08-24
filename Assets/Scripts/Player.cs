using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField] private AnimationClip endClip;
    [SerializeField] private AnimationClip endClip2;
    [SerializeField] private AnimationClip hitClip;
    [SerializeField] private Image[] sprites;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite noHeart;
    [SerializeField] private Sprite altsprite;
    [SerializeField] private Text lifeText;
    [SerializeField] private SpriteRenderer[] emoji;
    [SerializeField] private Sprite[] emojisprites;

    private Animator anim;
    private GameManager gm;

    private void Start() {
        anim = GetComponent<Animator>();
        gm = GameManager.instance;
        lifeText.text = gm.life.ToString();
    }

    private void Update() {
        if (gm.invincibility <= 0) {
            gm.shield.SetActive(false);
        }

        if (gm.missedNotes >= gm.life) {
            StartCoroutine(Hit());
            gm.missedNotes = 0;
            lifeText.text = gm.life.ToString();
            RemoveHeart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Obstacle")) {
            anim.SetTrigger("Obstacle");
        }
        else if (collision.CompareTag("Enemy")) {
            anim.SetTrigger("Enemy");
            Destroy(collision.gameObject, 0.25f);
        }
        else if (collision.CompareTag("Door")) {
            collision.GetComponent<SpriteRenderer>().sprite = altsprite;
        }
        else if (collision.CompareTag("Goal")) {
            StartCoroutine(GameWon());
        }
        collision.tag = "Untagged";
    }

    private void RemoveHeart() {
        if (sprites[0].sprite == fullHeart) {
            sprites[0].sprite = halfHeart;
        }
        else if (sprites[0].sprite == halfHeart) {
            sprites[0].sprite = noHeart;
        }
        else if (sprites[1].sprite == fullHeart) {
            sprites[1].sprite = halfHeart;
        }
        else if (sprites[1].sprite == halfHeart) {
            sprites[1].sprite = noHeart;
        }
        else if (sprites[2].sprite == fullHeart) {
            sprites[2].sprite = halfHeart;
        }
        else if (sprites[2].sprite == halfHeart) {
            StartCoroutine(GameLost());
        }
    }

    private IEnumerator Hit() {
        int random = Random.Range(0, emojisprites.Length);
        foreach (SpriteRenderer sr in emoji) {
            sr.sprite = emojisprites[random];
        }
        anim.SetTrigger("Hit");
        float length = hitClip.length;
        gm.playerHit = true;
        yield return new WaitForSeconds(length);
        gm.playerHit = false;
        gm.invincibility = 4f;
    }

    private IEnumerator GameLost() {
        anim.SetTrigger("GameLost");
        float length = endClip2.length;
        yield return new WaitForSeconds(length);
        gm.songs[GameManager.index].src.Stop();
        gm.canvas4.SetActive(true);
        gm.canvas3.SetActive(false);
    }

    private IEnumerator GameWon() {
        anim.SetTrigger("GameWon");
        float length = endClip.length;
        yield return new WaitForSeconds(length);
        float endscore = Mathf.Round((gm.currentbeatTempo - (gm.totalMissedNotes / (gm.playSpeed / 4))) * 100);
        if (endscore < 0) {
            endscore = 0f;
        }
        float maxscore = Mathf.Round(gm.currentbeatTempo * 100);
        gm.scoreText.text = "Score: " + endscore.ToString() + " / " + maxscore.ToString();
        if (endscore > PlayerPrefs.GetFloat(gm.currentSong.src.name, 0)) {
            PlayerPrefs.SetFloat(gm.currentSong.src.name, endscore);
        }
        gm.canvas.SetActive(true);
        gm.canvas3.SetActive(false);
    }
}
