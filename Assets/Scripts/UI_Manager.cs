using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides functionality for updating and displaying UI elements.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField nameInputField;
    [SerializeField] public TMP_Text playerNameText;
    [SerializeField] public TMP_Text xpText;
    [SerializeField] public TMP_Text levelText;
    [SerializeField] public TMP_Text currencyText;

    /// <summary>
    /// Applies the name change using the text from the input field.
    /// </summary>
    public void ApplyNameChange()
    {
        SetPlayerName(nameInputField.text);
    }

    /// <summary>
    /// Updates the player's name in the UI.
    /// </summary>
    /// <param name="name">The new player name.</param>
    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }

    /// <summary>
    /// Updates the XP display with the current XP value.
    /// </summary>
    /// <param name="currentXP">The current XP.</param>
    public void SetXpText(float currentXP)
    {
        xpText.text = currentXP.ToString();
    }

    /// <summary>
    /// Updates the level display with the current level.
    /// </summary>
    /// <param name="currentLevel">The current level.</param>
    public void SetLevelText(int currentLevel)
    {
        levelText.text = currentLevel.ToString();
    }

    /// <summary>
    /// Updates the currency display with the current currency amount.
    /// </summary>
    /// <param name="currentCurrency">The current currency value.</param>
    public void SetCurrencyText(float currentCurrency)
    {
        currencyText.text = currentCurrency.ToString();
    }
}
