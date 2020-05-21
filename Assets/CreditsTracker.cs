using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsTracker : MonoBehaviour
{
    private static int credits;
    private static bool initialized = false;
    public Text text;

    private void Start()
    {
        if (!initialized)
        {
            initialized = true;
            credits = SaveLoadSystem.Credits();
        }

        if(Accountant.Receipts.Count > 0)
        {
            ReceivePayout();
        }
    }

    private void Update()
    {
        text.text = credits.ToString();
    }

    public void ReceivePayout()
    {
        credits += Accountant.Payout();
    }

    public static int Credits()
    {
        return credits;
    }

    public static void AddCredits(int amount)
    {
        credits += amount;
        SaveLoadSystem.Save();
    }
}
