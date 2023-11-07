using UnityEngine;
using TMPro;
using HorrorJam.Audio;

public class MassageText : MonoBehaviour
{
    [SerializeField] TMP_Text massageText;

    public void SetMassageText(string text)
    {
        // AudioManager.Instance.PlayOneShot("newMassage");
        massageText.text = text;
    }
}
