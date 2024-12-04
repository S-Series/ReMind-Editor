using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System.Threading;
using TMPro;

public class test : MonoBehaviour
{
    [SerializeField] int[] data;
    void Start()
    {
        int input = 578;
        data = ConvertBase(input, 16, 15);
    }

    //$ Convert input Number from Base N to Base
    private int[] ConvertBase(int input, int N, int M)
    {
        List<int> ret = new List<int>();

        int passNum = 0;
        for (int index = 0; true; index++)
        {
            passNum += Mathf.FloorToInt(input % Mathf.Pow(10, index + 1) 
                / Mathf.Pow(10, index)) * Mathf.FloorToInt(Mathf.Pow(N, index));
            if (MathF.Pow(10 , index) >= input) { break;}
        }

        print(passNum);

        for (int index = 0; true; index++)
        {
            if (Mathf.Pow(M, index) >= passNum) { break; }
            ret.Add(Mathf.FloorToInt(passNum / Mathf.Pow(M, index)) % M);
        }
        ret.Reverse();
        return ret.ToArray();
    }
}