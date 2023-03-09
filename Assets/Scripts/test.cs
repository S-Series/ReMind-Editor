using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    private void Start()
    {
        print(0);
        print(1);
        print(2);
        print(3);
        for (int i = 0; i < 20; i++)
        {
            print(i + 4);
        }
    }
}
