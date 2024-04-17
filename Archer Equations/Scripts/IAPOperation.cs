using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPOperation : MonoBehaviour
{
    public Main m;
    public TMP_Text PriceText;
    public void OnPurchaseComplete(Product p)
    {
        m.OnPurchaseComplete(p);
    }
    public void OnPurchaseFailed(Product p, PurchaseFailureDescription description)
    {
        m.OnPurchaseFailed(p, description);
    }
    public void OnProductFetched(Product p)
    {
        PriceText.text = string.Format("{0:0.0}", (int)(p.metadata.localizedPrice * 10) / 10f)
            //p.metadata.localizedPrice.ToString("C1")
            + "\n" + p.metadata.isoCurrencyCode;

        
    }
}


//<size=30%>text</size=30%>