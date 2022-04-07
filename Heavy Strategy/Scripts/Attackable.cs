using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable// : MonoBehaviour
{
    public float attackPower = 0;
    public float attackSpeed = 0;
    public float life = 0;
    public float firstLife = 0;
    public void Reset()
    {
        life = firstLife;
    }
    public Attackable()
    {
        attackPower = 0;
        attackSpeed = 0;
        life = 0;
        firstLife = 0;
    }

}
