using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BeatScroller : MonoBehaviour {
    [SerializeField] private GameObject[] notes;

    private GameManager gm;
    private Coroutine coroutine;

    private void Start() {
        gm = GameManager.instance;
    }

    private void Update() {
        if (coroutine == null && gm.startPlaying && !gm.gameFinished) {
            coroutine = StartCoroutine(CreateNote());
        }
    }

    private void GenerateNote() {
        int index = Random.Range(0, notes.Length);
        GameObject random = notes[index];
        random.GetComponent<ButtonController>().ChangeColor(gm.currentColor);
        Instantiate(random, transform.position, Quaternion.identity);
    }

    private IEnumerator CreateNote() {
        if (gm.playerHit) {
            yield return new WaitForSeconds(gm.spawnRate / gm.currentbeatTempo);
        }
        GenerateNote();
        yield return new WaitForSeconds(gm.spawnRate / gm.currentbeatTempo);
        gm.SetNewColor();
        coroutine = null;
    }
}
