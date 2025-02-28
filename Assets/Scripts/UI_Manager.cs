using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    /// <summary>
    /// This is a testCode
    /// </summary>
    public TMP_InputField input;
    public TMP_Text playerName;
    public TMP_Text XP_Text;
    public TMP_Text Level_Text;
    public TMP_Text Currency_Text;

    public void ApplyNameChangeButton()
    {
        SetPlayerName(input.text);
    }
    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
    public void SetXpText(float currentXP)
    {
        XP_Text.text = currentXP.ToString();
    }
    public void SetLevelText(int currentLevel)
    {
        Level_Text.text = currentLevel.ToString();
    }
    public void SetCurrencyText(float currentCurrency)
    {
        Currency_Text.text = currentCurrency.ToString();
    }
}

