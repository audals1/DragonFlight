using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
    [SerializeField]
    UILabel m_distScoreLable;
    [SerializeField]
    UILabel m_huntScoreLable;
    [SerializeField]
    UILabel m_goldCountLable;
    
    StringBuilder m_sb = new StringBuilder();
    int m_distScore;
    int m_huntScore;
    int m_goldCount;
    public int DistScore { get { return m_distScore; } }
    public int HuntScore { get { return m_huntScore; } }
    public int GoldCount { get { return m_goldCount; } }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void SetPause()
    {
        Time.timeScale = Time.timeScale != 0f ? 0f : 1f;
    }
    public void SetDistanceScore(float dist)
    {
        m_distScore = Mathf.RoundToInt(dist * 100f);
        m_sb.AppendFormat("{0:n0}M", m_distScore);
        m_distScoreLable.text = m_sb.ToString();
        m_sb.Clear();
        
    }
    public void SetHuntScore(int score)
    {
        m_huntScore += score;
        m_sb.AppendFormat("{0:n0}", m_huntScore);
        m_huntScoreLable.text = m_sb.ToString();
        m_sb.Clear();
    }
    public void SetGoldCount(int gold)
    {
        m_goldCount += gold;
        m_sb.AppendFormat("{0:n0}", m_goldCount);
        m_goldCountLable.text = m_sb.ToString();
        m_sb.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_distScore = 0;
        m_huntScore = 0;
        m_goldCount = 0;
        SetDistanceScore(0);
        SetHuntScore(0);
        SetGoldCount(0);
    }
}
