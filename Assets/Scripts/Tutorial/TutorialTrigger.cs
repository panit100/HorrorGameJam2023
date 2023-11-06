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

    void Start()
    {
        SetupObject();
    }
    void SetupObject()
    {
        tutorialCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            ActiveTutorialCanvas();
        }
    }

    void ActiveTutorialCanvas()
    {
        tutorialCanvas.SetActive(true);
        AudioManager.Instance.PlayOneShot("window_entry");
        tutorialText.text = text;
        tutorialButton.onClick.AddListener(DisableTutorialCanvas);
        GameManager.Instance.OnChangeGameStage(GameStage.Tutorial);
        Time.timeScale = 0;
    }

    void DisableTutorialCanvas()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlayOneShot("ui_click");
        GameManager.Instance.OnChangeGameStage(GameStage.Playing);
        tutorialCanvas.SetActive(false);
        this.gameObject.SetActive(false);
        tutorialButton.onClick.RemoveAllListeners();
    }
}
