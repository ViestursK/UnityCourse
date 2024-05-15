using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPurseUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    // Update the coin text display
    public void UpdateCoinText(CoinPurse coinPurse)
    {
        coinText.text = coinPurse.coins.ToString();
    }
}
