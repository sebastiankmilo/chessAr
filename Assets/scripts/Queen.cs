using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : chessman
{
    public override bool[,] possiblemove()
    {
        bool[,] r = new bool[8, 8];
        chessman c;
        int i, j;
        //right
        i = Currentx;
        while (true)
        {
            i++;
            if (i >= 8)
                break;
            c = boarmanager.Instance.chessmans[i, Currenty];
            if (c == null)
                r[i, Currenty] = true;
            else
            {
                if (c.iswhite != iswhite)
                    r[i, Currenty] = true;
                break;
            }


        }
        //left
        i = Currentx;
        while (true)
        {
            i--;
            if (i < 0)
                break;
            c = boarmanager.Instance.chessmans[i, Currenty];
            if (c == null)
                r[i, Currenty] = true;
            else
            {
                if (c.iswhite != iswhite)
                    r[i, Currenty] = true;
                break;
            }


        }
        //up
        i = Currenty;
        while (true)
        {
            i++;
            if (i >= 8)
                break;
            c = boarmanager.Instance.chessmans[Currentx, i];
            if (c == null)
                r[Currentx, i] = true;
            else
            {
                if (c.iswhite != iswhite)
                    r[Currentx, i] = true;
                break;
            }


        }
        //down
        i = Currenty;
        while (true)
        {
            i--;
            if (i < 0)
                break;
            c = boarmanager.Instance.chessmans[Currentx, i];
            if (c == null)
                r[Currentx, i] = true;
            else
            {
                if (c.iswhite != iswhite)
                    r[Currentx, i] = true;
                break;
            }


        }
        //top left
        i = Currentx;
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
            if (i >= 8 || j < 0)
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
