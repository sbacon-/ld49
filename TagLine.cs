using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class TagLine : MonoBehaviour
{
    [SerializeField]
    public String[] taglines;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0,taglines.Length);
        GetComponent<TextMeshProUGUI>().text = taglines[rand];   
    }

}
