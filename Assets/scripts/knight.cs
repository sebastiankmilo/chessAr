using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : chessman
{
    public override bool[,] possiblemove()
    {
        bool[,] r = new bool[8, 8];
        //upleft
        caballomove(Currentx - 1, Currenty + 2, ref r);
        //upright
        caballomove(Currentx + 1, Currenty + 2, ref r);
        //rightup
        caballomove(Currentx +2, Currenty + 1, ref r);
        //rightdown
        caballomove(Currentx + 2, Currenty - 1, ref r);

        //downleft
        caballomove(Currentx - 1, Currenty - 2, ref r);
        //downright
        caballomove(Currentx + 1, Currenty - 2, ref r);
        //leftup
        caballomove(Currentx - 2, Currenty + 1, ref r);
        //leftdown
        caballomove(Currentx - 2, Currenty - 1, ref r);
        return r;
    }
    public void caballomove(int x,int y,ref bool[,] r)
    {
        chessman c;
        if(x>=0 && x<8 && y>=0 && y < 8)
        {
            c = boarmanager.Instance.chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else
            {
                if (c.iswhite != iswhite)
                    r[x, y] = true;
            }
        }
    }
}
