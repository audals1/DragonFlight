using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static EventDelegate.Parameter MakeParamater(Object value, System.Type type )
    {
        EventDelegate.Parameter param = new EventDelegate.Parameter();
        param.obj = value;
        param.expectedType = type;
        return param;
    }
    public static int GetPriority(int[] table)
    {
        if (table.Length == 0) return -1;
        int sum = 0;
        int index = 0;
        for (int i = 0; i < table.Length; i++)
        {
            sum += table[i];
        }
        index = Random.Range(0, sum) + 1;

        sum = 0;
        for (int i = 0; i < table.Length; i++)
        {
            if(index > sum && index <= sum + table[i])
            {
                return i;
            }
            sum += table[i];
        }
        return -1;
    }
}
