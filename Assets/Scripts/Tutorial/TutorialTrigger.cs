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
        tutorialText.text = text;
        tutorialButton.onClick.AddListener(DisableTutorialCanvas);
        GameManager.Instance.OnChangeGameStage(GameStage.Tutorial);
        Time.timeScale = 0;
    }

    void DisableTutorialCanvas()
    {
        Time.timeScale = 1;
        GameManager.Instance.OnChangeGameStage(GameStage.Playing);
        tutorialCanvas.SetActive(false);
        AudioManager.Instance.PlayOneShot("clickUI");
        this.gameObject.SetActive(false);
        tutorialButton.onClick.RemoveAllListeners();
    }
}
