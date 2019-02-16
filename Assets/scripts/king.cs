using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king :chessman
{
    public override bool[,] possiblemove()
    {
        bool[,] r = new bool[8, 8];
        chessman c;
        int i, j;
        //top side
        i = Currentx - 1;
        j = Currenty + 1;
        if(Currenty != 7)
        {
            for(int k=0;k < 3; k++)
            {
                if(i>=0 || i < 8)
                {
                    c = boarmanager.Instance.chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (iswhite != c.iswhite)
                        r[i, j] = true;
                }
                i++;
            }
        }
        //down side
        i = Currentx - 1;
        j = Currenty - 1;
        if (Currenty != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 || i < 8)
                {
                    c = boarmanager.Instance.chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (iswhite != c.iswhite)
                        r[i, j] = true;
                }
                i++;
            }
        }
        //middle left
        if(Currentx != 0)
        {
            c = boarmanager.Instance.chessmans[Currentx - 1, Currenty];
            if (c == null)
                r[Currentx - 1, Currenty]=true;
            else if (iswhite != c.iswhite)
                r[Currentx - 1, Currenty]=true;
        }
        //middle right
        if (Currentx != 7)
        {
            c = boarmanager.Instance.chessmans[Currentx +1, Currenty];
            if (c == null)
                r[Currentx + 1, Currenty]=true;
            else if (iswhite != c.iswhite)
                r[Currentx + 1, Currenty]=true;
        }
        return r;
    }
}

