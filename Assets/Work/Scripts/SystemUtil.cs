using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemUtil
{

    static public class VRObject
    {
        static private int selectNum = 0;
        static public void SetNum(int num)
        {
            selectNum = num;
        }
        static public int GetNum()
        {
            return selectNum;
        }
    }

    static public class Const
    {
        public const int VR_OBJECT_COUNT = 48;
    }

}
