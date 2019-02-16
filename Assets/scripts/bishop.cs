using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : chessman
{
    public override bool[,] possiblemove()
    {
        bool[,] r = new bool[8, 8];
        chessman c;
        int i, j;
        //top left
        i= Currentx;
        j = Currenty;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;
            c = boarmanager.Instance.chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (iswhite != c.iswhite)
                    r[i, j] = true;
                break;
            }

        }
        //top right
        i = Currentx;
        j = Currenty;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8)
                break;
            c = boarmanager.Instance.chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (iswhite != c.iswhite)
                    r[i, j] = true;
                break;
            }

        }
        //down left
        i = Currentx;
        j = Currenty;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;
            c = boarmanager.Instance.chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (iswhite != c.iswhite)
                    r[i, j] = true;
                break;
            }

        }
        //down right
        i = Currentx;
        j = Currenty;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j <0)
                break;
            c = boarmanager.Instance.chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (iswhite != c.iswhite)
                    r[i, j] = true;
                break;
            }

        }
        return r;
    }
}
