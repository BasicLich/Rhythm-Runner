using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject panel;
    [SerializeField] private Text songNameText;
    [SerializeField] private Text highScoreText;

    private GameManager gm;
    private const string t = "\t\t";

    private void Start() {
        gm = GameManager.instance;
    }

    public void Quit() {
        Application.Quit();
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Controls() {
        controls.SetActive(!controls.activeSelf);
        start.SetActive(!start.activeSelf);
    }

    public void Scores() {
        highScoreText.text = "";
        songNameText.text = "";
        for (int i = 0; i < gm.songs.Length; i++) {
            float currentHighScore = Mathf.Round(PlayerPrefs.GetFloat(gm.songs[i].src.name, 0));
            float maxscore = gm.maxScores[i];
            songNameText.text += i.ToString() + "." + t + gm.songs[i].src.name + "\n";
            highScoreText.text += currentHighScore.ToString() + t + "/" + t + maxscore.ToString() + "\n";
            gm.SetMedals(i);
        }

        for (int i = 0; i < gm.medalScore.Length; i++) {
            gm.medalScore[i] = 0;
        }

        panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf == false) {
            gm.scoreScreenOpen = false;
        }
        else {
            gm.scoreScreenOpen = true;
        }
    }

    public void OpenWebsite() {
        Application.OpenURL("https://evankingmusic.com");
    }

    public void OpenYt() {
        Application.OpenURL("https://www.youtube.com/user/EvanKingAudio");
    }

}
