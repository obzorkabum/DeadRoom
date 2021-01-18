using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapPlace : MonoBehaviour
{
    public static bool[,] SimpleTrapAccessiblePlace = new bool[41,14];


    private void Start()
    {

        for(int i=0;i<=22;i++)
        {
            trapPlace.SimpleTrapAccessiblePlace[i, 0] = true;
        }

        for (int i = 5; i <= 8; i++)
        {
            trapPlace.SimpleTrapAccessiblePlace[i, 3] = true;
        }

        for (int i = 16; i <= 19; i++)
        {
            trapPlace.SimpleTrapAccessiblePlace[i, 3] = true;
        }

        for (int i = 23; i <= 40; i++)
        {
            trapPlace.SimpleTrapAccessiblePlace[i, 3] = true;
        }

       
    }
}
