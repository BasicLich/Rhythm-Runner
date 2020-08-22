using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyNotes : MonoBehaviour {
    private ButtonController btn;
    private GameManager gm;

    [SerializeField] private AudioMixer mixer;

    private void Start() {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Note")) {
            btn = collision.GetComponent<ButtonController>();
            if (btn.sr.sprite != btn.pressedImage) {
                gm.LoosePoints();
            }
            else {
                gm.GetPoints();
            }
            mixer.SetFloat("Rate", gm.rate);
            mixer.SetFloat("Pitch", gm.pitch);
        }
        Destroy(collision.gameObject);
    }
}
