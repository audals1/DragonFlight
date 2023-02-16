using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField]
    UILabel m_totalScoreLabel;
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_goldCountLabel;
    [SerializeField]
    UILabel m_bestScoreLabel;
    [SerializeField]
    UI2DSprite m_sdSprite;
    [SerializeField]
    GameObject m_bestobj;

    public void Show()
    {
        bool isBest = false;
        gameObject.SetActive(true);
        GameUIManager.Instance.Hide();
        int totalScore = GameUIManager.Instance.DistScore + GameUIManager.Instance.HuntScore;
        if(PlayerDataManager.Instance.BestScore < totalScore)
        {
            isBest = true;
            m_bestobj.SetActive(true);
        }
        else
        {
            m_bestobj.SetActive(false);
        }
        string path = string.Format("SD/sd_{0:00}{1}", PlayerDataManager.Instance.HeroIndex + 1, isBest ? "_highscore" : string.Empty);
        m_sdSprite.sprite2D = Resources.Load<Sprite>(path);
        PlayerDataManager.Instance.BestScore = isBest ? totalScore : PlayerDataManager.Instance.BestScore;
        m_totalScoreLabel.text = string.Format("{0:n0}", totalScore);
        m_distScoreLabel.text = string.Format("{0:n0}", GameUIManager.Instance.DistScore);
        m_huntScoreLabel.text = string.Format("{0:n0}", GameUIManager.Instance.HuntScore);
        m_goldCountLabel.text = string.Format("{0:n0}", GameUIManager.Instance.GoldCount);
        m_bestScoreLabel.text = string.Format("{0:n0}", PlayerDataManager.Instance.BestScore);
        PlayerDataManager.Instance.IncreaseGold(GameUIManager.Instance.GoldCount);
        
        PlayerDataManager.Instance.Save();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void GoLobbyScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Lobby);
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }
}
