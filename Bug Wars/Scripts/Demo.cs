using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Text t;
    // Start is called before the first frame update
    void Start()
    {
        t.text = Dns.GetHostEntry(Dns.GetHostName())
             .AddressList.First(
                 f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
