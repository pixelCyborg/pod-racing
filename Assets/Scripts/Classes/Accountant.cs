using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accountant : MonoBehaviour
{
    public static List<CreditsEarned> Receipts = new List<CreditsEarned>();

    public class CreditsEarned
    {
        public string description;
        public int amount;

        public CreditsEarned(string _description, int _amount)
        {
            description = _description;
            amount = _amount;
        }
    }

    public static void AddValue(string description, int amount)
    {
        Receipts.Add(new CreditsEarned(description, amount));
    }

    public static int Payout(bool refresh = true)
    {
        int total = 0;

        for(int i = 0; i < Receipts.Count; i++)
        {
            total += Receipts[i].amount;
        }

        if(refresh) Receipts = new List<CreditsEarned>();
        return total;
    }
}
