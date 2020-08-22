using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacles : MonoBehaviour {
    private GameManager gm;

    private void Start() {
        gm = GameManager.instance;
    }

    private void FixedUpdate() {
        if (gm.playerHit) {
            return;
        }
        transform.position -= new Vector3(gm.currentbeatTempo * Time.deltaTime, 0f, 0f);
    }
}
