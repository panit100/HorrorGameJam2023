using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] Button backButton;
    private void Awake()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        gameObject.SetActive(false);
    }
    void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
