using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BeatScroller : MonoBehaviour {

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
        GameObject random = gm.notes[gm.indexForObstacles];
        Instantiate(random, transform.position, Quaternion.identity);
    }

    private IEnumerator CreateNote() {
        if (gm.playerHit) {
            yield return new WaitForSeconds(gm.spawnRate / gm.currentbeatTempo);
        }
        GenerateNote();
        yield return new WaitForSeconds(gm.spawnRate / gm.currentbeatTempo);
        coroutine = null;
    }
}
