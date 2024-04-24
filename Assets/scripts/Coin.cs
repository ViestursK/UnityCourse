using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CoinPurse coinPurse = other.GetComponent<CoinPurse>();

        if (coinPurse != null)
        {
            coinPurse.CoinCollected();
            gameObject.SetActive(false);
        }
    }
}
