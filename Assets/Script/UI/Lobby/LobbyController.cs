using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    UI2DSprite m_heroSprite;
    [SerializeField]
    TweenPosition m_heroPosTween;
    [SerializeField]
    GameObject m_menuButtonobj;
    [SerializeField]
    GameObject m_menuobj;
    [SerializeField]
    UIButton[] m_menuButtons;
    ILobbyMenu[] m_lobbyMenu;
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnMenuButtonClick(UIButton button)
    {
        Hide();
        var num = int.Parse(button.name.Split('.')[0]) - 1;
        m_lobbyMenu[num].Show();
    }
    public void SetHeroSprite(Sprite heroSpr, Vector3 pos)
    {
        m_heroSprite.sprite2D = heroSpr;
        m_heroSprite.MakePixelPerfect();
        m_heroSprite.transform.position = pos;
        m_heroPosTween.from = pos;
        m_heroPosTween.to = m_heroPosTween.from + Vector3.down * 20f;
    }
    public void GameStart()
    {
        LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Game);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_menuButtons = m_menuButtonobj.GetComponentsInChildren<UIButton>();
        m_lobbyMenu = m_menuobj.GetComponentsInChildren<ILobbyMenu>(true);
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnMenuButtonClick");
            del.parameters[0] = Util.MakeParamater(m_menuButtons[i], typeof(UIButton));
            m_menuButtons[i].onClick.Add(del);
        }
    }
}
