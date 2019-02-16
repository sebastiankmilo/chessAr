using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : chessman
{
    public override bool[,] possiblemove()
    {
        bool[,] r = new bool[8, 8];
        chessman c, c2;
        // white team move
        if (iswhite)
        {
            //diagonal left
            if(Currentx !=0 && Currenty != 7)
            {
                c = boarmanager.Instance.chessmans[Currentx - 1, Currenty + 1];
                if (c != null && !c.iswhite)
                    r[Currentx - 1, Currenty +1] = true;
            }
            //diagonal right
            if (Currentx != 7 && Currenty != 7)
            {
                c = boarmanager.Instance.chessmans[Currentx + 1, Currenty + 1];
                if (c != null && !c.iswhite)
                    r[Currentx + 1, Currenty + 1] = true;
            }
            //midlle 
            if(Currenty != 7)
            {
                c = boarmanager.Instance.chessmans[Currentx, Currenty + 1];
                if(c==null)
                    r[Currentx, Currenty + 1] = true;
            }
            //midle first move
            if (Currenty == 1)
            {
                c = boarmanager.Instance.chessmans[Currentx, Currenty + 1];
                c2 = boarmanager.Instance.chessmans[Currentx, Currenty + 2];
                if (c == null & c2 == null)
                    r[Currentx, Currenty + 2] = true;

            }


        }
        else
        {
            //diagonal left
            if (Currentx != 0 && Currenty != 7)
            {
                c = boarmanager.Instance.chessmans[Currentx - 1, Currenty -1];
                if (c != null && c.iswhite)
                    r[Currentx - 1, Currenty - 1] = true;
            }
            //diagonal right
            if (Currentx != 7 && Currenty != 0)
            {
                c = boarmanager.Instance.chessmans[Currentx + 1, Currenty - 1];
                if (c != null && c.iswhite)
                    r[Currentx + 1, Currenty - 1] = true;
            }
            //midlle 
            if (Currenty != 0)
            {
                c = boarmanager.Instance.chessmans[Currentx, Currenty - 1];
                if (c == null)
                    r[Currentx, Currenty - 1] = true;
            }
            //midle first move
            if (Currenty == 6)
            {
                c = boarmanager.Instance.chessmans[Currentx, Currenty - 1];
                c2 = boarmanager.Instance.chessmans[Currentx, Currenty - 2];
                if (c == null & c2 == null)
                    r[Currentx, Currenty - 2] = true;

            }


        }
        return r;
    }
}
