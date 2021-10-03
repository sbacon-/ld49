using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stock 
{

    public string name;
    public string symbol;
    public float price;
    public float[] history = new float[5];
    public int owned = 0;
    public Trend trend;
    public Trend volatility;

}

public enum Trend{
    PUSH, RISE, FALL, MOON, CRASH
}
