using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chessman: MonoBehaviour
{
   public int Currentx { set; get; }
    public int Currenty { set; get; }
    public bool iswhite;
    public void setposition(int x,int y)
    {
        Currentx = x;
        Currenty = y;
    }
    public virtual bool[,] possiblemove()
    {
        return new bool[8,8];
    }
}
