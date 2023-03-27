using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class PlayerDataManager : DontDestroy<PlayerDataManager>
{
    [SerializeField]
    HerodData[] m_heroDatas;
    PlayerData m_myData;
    public int HeroIndex { get { return m_myData.m_heroIndex; } set { m_myData.m_heroIndex = value; } }
    public int BestScore { get { return m_myData.m_bestScore; } set { m_myData.m_bestScore = value; } }
    public int CountOfHeroes { get { return m_myData.m_heroList.Count; } }
    public HerodData GetHeroData(int heroIndex)
    {
        return m_heroDatas[heroIndex];
    }
    public bool IsPlayable(int heroIndex)
    {
        return m_myData.m_heroList[heroIndex].m_isOpen;
    }
    public void OpenHero(int heroIndex)
    {
        m_myData.m_heroList[heroIndex].m_isOpen = true;
    }

    public void IncreaseGem(int gem)
    {
        m_myData.m_gemOwned += gem;
    }
    public int DecreaseGem(int gem)
    {
        if (m_myData.m_gemOwned - gem >= 0)
        {
            return m_myData.m_gemOwned -= gem;
        }
        return -1;
    }


    public void IncreaseGold(int gold)
    {
        m_myData.m_goldOwned += gold;
    }
    public int DecreaseGold(int gold)
    {
        if(m_myData.m_goldOwned - gold >=0)
        {
            return m_myData.m_goldOwned -= gold;
        }
        return -1;
    }
    public bool Load()
    {
        var jsonStr = PlayerPrefs.GetString("PLAYER_DATA", string.Empty);
        if(string.IsNullOrEmpty(jsonStr))
        {
            m_myData = new PlayerData() { m_goldOwned = 1000, m_gemOwned = 100 };
            m_myData.m_heroList = new List<HerodData>(m_heroDatas);
            return false;
        }
        else
        {
            Debug.Log(jsonStr);
            //m_myData = JsonReader.Deserialize<PlayerData>(jsonStr); 외부 플러그인
            m_myData = JsonUtility.FromJson<PlayerData>(jsonStr); //유니티내부기능
            return true;
        }
    }
    public void Save()
    {
        //var jsonStr = JsonWriter.Serialize(m_myData); 외부 플러그인
        var jsonStr = JsonUtility.ToJson(m_myData); //유니티 내부기능
        Debug.Log(jsonStr);
        PlayerPrefs.SetString("PLAYER_DATA", jsonStr);
        PlayerPrefs.Save();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!Load())
        {
            Save();
        }
        //PlayerPrefs.DeleteAll();
    }

}
