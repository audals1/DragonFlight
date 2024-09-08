using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu_Character : MonoBehaviour, ILobbyMenu
{
    [SerializeField]
    LobbyController m_lobby;
    [SerializeField]
    UI2DSprite m_heroSprite;
    [SerializeField]
    Vector2[] m_heroSpritesPos;
    [SerializeField]
    TweenPosition m_heroPosTween;
    [SerializeField]
    UIButton[] m_buttons;
    int m_select;
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnPressLeft()
    {
        m_select--;
        if(m_select < 0)
        {
            m_select = PlayerDataManager.Instance.CountOfHeroes - 1;
        }
        ResetHeroSprite();
    }
    public void OnPressRight()
    {
        m_select++;
        if(m_select > PlayerDataManager.Instance.CountOfHeroes - 1)
        {
            m_select = 0;
        }
        ResetHeroSprite();
    }
    public void OnPressSelect()
    {
        PlayerDataManager.Instance.HeroIndex = m_select;
        m_lobby.Show();
        m_lobby.SetHeroSprite(m_heroSprite.sprite2D, m_heroSpritesPos[m_select]);
        Hide();
    }
    public void OnPressBuy()
    {
        var data = PlayerDataManager.Instance.GetHeroData(m_select);
        int price = data.m_price;
        string name = data.m_name;
        PopupManager.Instance.OpenPopup_OkCancel("안내", string.Format("[FFFF00]{0}젬을 소모하여\r\n [00FF00]{1}[-] 캐릭터를 구매하시겠습니까?", price, name), ()=> { 
            if(PlayerDataManager.Instance.DecreaseGem(price) != -1)
            {
                PlayerDataManager.Instance.OpenHero(m_select);
                PlayerDataManager.Instance.Save();
                PopupManager.Instance.ClosePopup();
                ResetHeroInfo();
            }
            else
            {
                PopupManager.Instance.OpenPopup_Ok("[000000]안내[-]", "[000000]젬이 부족합니다.[-]", null, "확인");
            }
        }, null, "예", "아니오");
    }
    void ResetHeroInfo()
    {
        if (!PlayerDataManager.Instance.IsPlayable(m_select))
        {
            m_heroSprite.depth = -2;
            m_buttons[0].gameObject.SetActive(false);
            m_buttons[1].gameObject.SetActive(true);
        }
        else
        {
            m_heroSprite.depth = 0;
            m_buttons[0].gameObject.SetActive(true);
            m_buttons[1].gameObject.SetActive(false);
        }
    }
    void ResetHeroSprite()
    {
        m_heroSprite.sprite2D = LoadHeroSprite();
        m_heroSprite.MakePixelPerfect();
        m_heroSprite.transform.localPosition = m_heroSpritesPos[m_select];
        m_heroPosTween.from = m_heroSprite.transform.localPosition;
        m_heroPosTween.to = m_heroPosTween.from + Vector3.down * 20f;
        ResetHeroInfo();
    }
    Sprite LoadHeroSprite()
    {
        return Resources.Load<Sprite>(string.Format("Character/Character_{0:00}", m_select + 1));
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        m_select = PlayerDataManager.Instance.HeroIndex;
        ResetHeroSprite();
    }
}
