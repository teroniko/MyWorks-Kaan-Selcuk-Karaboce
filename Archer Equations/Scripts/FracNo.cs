using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracNo
{


    public byte Oparetion;//+,-,x,/
    public int UpNo, DownNo;
    public FracNo(byte Oparation, int UpNo, int DownNo)
    {
        this.Oparetion = Oparation;
        this.UpNo = UpNo;
        this.DownNo = DownNo;
    }
}
