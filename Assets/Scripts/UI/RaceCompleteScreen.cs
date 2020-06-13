using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceCompleteScreen : MonoBehaviour
{
    public CanvasGroup positions;
    public CanvasGroup summary;
    public CanvasGroup receiptParent;
    public Text totalEarnings;
    private List<Receipt> receipts;

    public void Start()
    {
        receipts = new List<Receipt>();
        for(int i = 0; i < receiptParent.transform.childCount; i++)
        {
            Receipt receipt = receiptParent.transform.GetChild(i).GetComponent<Receipt>();
            if(receipt)
            {
                receipts.Add(receipt);
            }
        }

        for(int i = 0; i < receipts.Count; i++)
        {
            receipts[i].Hide();
        }
    }

    public void TotalEarnings()
    {
        for(int i = 0; i < receipts.Count; i++)
        {
            if(i < Accountant.Receipts.Count)
            {
                receipts[i].SetPayment(Accountant.Receipts[i].description, Accountant.Receipts[i].amount);
            }
            else
            {
                receipts[i].Hide();
            }
        }

        totalEarnings.text = Accountant.Payout(false).ToString() + " Credits";
    }

    public void ReturnToMenu()
    {
        SceneFader.instance.QueueBaseScene("Overview");
        SceneFader.instance.FadeOut();
    }
}
