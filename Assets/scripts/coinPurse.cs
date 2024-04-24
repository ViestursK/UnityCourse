using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinPurse : MonoBehaviour
{
    public int coins { get; private set; }

    public UnityEvent<CoinPurse> OnCoinCollected;

    public void CoinCollected()
    {
        coins++;
        OnCoinCollected.Invoke(this);
    }
}
