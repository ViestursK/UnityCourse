using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinPurseUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCoinText(CoinPurse coinPurse)
    {
        coinText.text = coinPurse.coins.ToString();
    }
}
