using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public TMP_Text opportunityText;
    public TMP_Text displayText;
    public TMP_Text successText;
    public int maxOpportunities = 3;
    private int remainingOpportunities;
    private string targetCode;
    private string currentCode;
    private bool isSuccessful;

    private void Start()
    {
        ResetGame();
    }

    public void OnNumberButtonClick(string number)
    {
        if (!isSuccessful && currentCode.Length < 3)
        {
            currentCode += number;
            UpdateDisplay();
            opportunityText.gameObject.SetActive(false);
        }
    }

    public void OnCheckButtonClick()
    {
        if (!isSuccessful)
        {
            if (currentCode.Equals(targetCode))
            {
                successText.gameObject.SetActive(true);
                isSuccessful = true;
            }
            else
            {
                remainingOpportunities--;
                if (remainingOpportunities <= 0)
                {
                    opportunityText.text = "남은 기회 없음";
                }
                else
                {
                    opportunityText.text = "남은 기회: " + remainingOpportunities;
                }
                opportunityText.gameObject.SetActive(true);
            }
            currentCode = "";
            UpdateDisplay();
        }
    }

    public void OnClearButtonClick()
    {
        if (!isSuccessful && currentCode.Length > 0)
        {
            currentCode = currentCode.Substring(0, currentCode.Length - 1);
            UpdateDisplay();
        }
    }

    public void OnRestartButtonClick()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        remainingOpportunities = maxOpportunities;
        isSuccessful = false;
        targetCode = "149";
        currentCode = "";
        opportunityText.text = "남은 기회: " + remainingOpportunities;
        opportunityText.gameObject.SetActive(true);
        displayText.text = "";
        successText.gameObject.SetActive(false);
    }

    private void UpdateDisplay()
    {
        displayText.text = currentCode;
    }
}