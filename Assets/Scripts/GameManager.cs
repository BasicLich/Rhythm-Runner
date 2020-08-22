using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {
    #region Singelton
    public static GameManager instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one Instance of Inventory found");
            return;
        }

        dropdown.ClearOptions();
        foreach (var song in songs) {
            if (PlayerPrefs.GetFloat(song.src.name, 0) <= 0) {
                PlayerPrefs.SetFloat(song.src.name, 0);
            }
            maxScores.Add(Mathf.Round((song.beatTempo / playSpeed) * 100));
            dropOptions.Add(song.src.name);
        }
        dropdown.AddOptions(dropOptions);
        dropdown.value = index;

        instance = this;
    }
    #endregion

    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject obstacleScroller;
    [SerializeField] private GameObject goal;
    [SerializeField] private Text lifeText;
    [SerializeField] private Dropdown dropdown;

    public List<string> dropOptions = new List<string>();

    public Text[] medal;
    public float[] medalScore;

    public List<float> maxScores;

    public GameObject shield;

    public int totalMissedNotes = 0;
    public int missedNotes = 0;
    public int life = 4;
    public float invincibility = 0f;

    public static int index;
    public Audio[] songs;
    public Audio currentSong;

    public Text scoreText;
    public GameObject canvas;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;

    public float spawnRate = 4f;
    public float playSpeed = 30f;

    public Color currentColor;
    public float currentbeatTempo;
    public float startbeatTempo;
    public bool startPlaying;
    public bool gameFinished;
    public bool playerHit;
    public bool scoreScreenOpen;

    public float pitch = 1f;
    public float rate = 0f;


    private void Update() {
        if (invincibility > 0) {
            invincibility -= Time.deltaTime;
            shield.SetActive(true);
        }
        if (!startPlaying && !scoreScreenOpen) {
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0)) {
                canvas2.SetActive(false);
                canvas3.SetActive(true);
                // Get the current index of the dropDown
                index = dropdown.value;
                currentbeatTempo = songs[index].beatTempo;
                currentbeatTempo /= playSpeed;
                startbeatTempo = currentbeatTempo;
                currentSong = songs[index];
                songs[index].src.Play();
                startPlaying = true;
            }
        }
        else if (!scoreScreenOpen) {
            if (!songs[index].src.isPlaying) {
                Endgame();
            }
        }
    }

    public void SetNewColor() {
        int index = UnityEngine.Random.Range(0, colors.Length);
        currentColor = colors[index];
    }

    public void LoosePoints() {
        currentbeatTempo -= 0.2f;
        if (currentbeatTempo <= 0f) {
            currentbeatTempo += 0.4f;
        }
        rate += 25f / 1000f;
        pitch -= 12f / 1000f;
        // Add Missed Note Points
        totalMissedNotes += 1;
        if (invincibility > 0) {
            return;
        }
        missedNotes += 1;
        lifeText.text = (life - missedNotes).ToString();
    }

    public void GetPoints() {
        currentbeatTempo += 0.05f;
        if (rate > 0f) { rate -= 12.5f / 1000f; }
        if (pitch < 1f) { pitch += 6.25f / 1000f; }
    }

    private void Endgame() {
        if (!gameFinished) {
            StartCoroutine(SpawnGoal());
        }
        gameFinished = true;
    }

    IEnumerator SpawnGoal() {
        yield return new WaitForSeconds(spawnRate / currentbeatTempo);
        Instantiate(goal, obstacleScroller.transform.position, Quaternion.identity);
    }

    public void SetMedals(int i) {
        float score = PlayerPrefs.GetFloat(songs[i].src.name, 0);
        float maxScore = maxScores[i];

        if (score >= 0 && score < maxScore / 4f) {
            medalScore[0] += 1;
            medal[0].text = medalScore[0].ToString();
        }
        else if (score >= maxScore / 4f && score < maxScore / 2f) {
            medalScore[1] += 1;
            medal[1].text = medalScore[1].ToString();
        }
        else if (score >= maxScore / 2f && score < (maxScore / 4f) * 3) {
            medalScore[2] += 1;
            medal[2].text = medalScore[2].ToString();
        }
        else if (score >= (maxScore / 4f) * 3 && score <= maxScore) {
            medalScore[3] += 1;
            medal[3].text = medalScore[3].ToString();
        }
    }
}
