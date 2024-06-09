using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSymbolImage : MonoBehaviour
{
    int maxSymbolType = 4;

    [SerializeField] int currentSymbolType = 0;
    [SerializeField] Button nextSymbolButton;
    [SerializeField] Button previousSymbolButton;

    [SerializeField] Image symbolImage;

    [SerializeField] Sprite[] symbolSprite;
    void Start()
    {
        nextSymbolButton.onClick.AddListener(NextSymbol);
        previousSymbolButton.onClick.AddListener(PreviousSymbol);
    }

    public void setSymbolImage()
    {
        symbolImage.sprite = symbolSprite[currentSymbolType];
    }

    public void setCurrentType(int index)
    {
        this.currentSymbolType = index;
        setSymbolImage();
    }

    public void NextSymbol()
    {
        currentSymbolType++;

        if (currentSymbolType >= maxSymbolType)
            currentSymbolType = 0;

        setSymbolImage();
    }

    public void PreviousSymbol()
    {
        currentSymbolType--;

        if (currentSymbolType < 0)
            currentSymbolType = maxSymbolType - 1;

        setSymbolImage();
    }

    public int GetSymbolType()
    {
        return currentSymbolType;
    }

}
