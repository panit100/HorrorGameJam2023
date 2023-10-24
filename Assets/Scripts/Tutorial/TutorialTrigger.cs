using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HorrorJam.Audio;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField, TextArea] private string text;
    [SerializeField] private Button tutorialButton;

    private void Start()
    {
        SetupObject();
    }
    private void SetupObject()
    {
        tutorialCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            ActiveTutorialCanvas();
        }
    }
    private void ActiveTutorialCanvas()
    {
        tutorialCanvas.SetActive(true);
        tutorialText.text = text;
        tutorialButton.onClick.AddListener(DisableTutorialCanvas);
        GameManager.Instance.LockCursor(false);
        Time.timeScale = 0;
    }
    private void DisableTutorialCanvas()
    {
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
        GameManager.Instance.LockCursor(true);
        AudioManager.Instance.PlayOneShot("clickUI");
        this.gameObject.SetActive(false);
    }
}