using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HerodData
{
    public string m_name;
    public string m_job;
    public int m_price;
    public int m_level;
    public bool m_isOpen;
}
[System.Serializable]
public class PlayerData
{
    public int m_bestScore;
    public int m_goldOwned;
    public int m_gemOwned;
    public int m_heroIndex;
    public List<HerodData> m_heroList = new List<HerodData>();
}
