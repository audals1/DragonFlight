using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public enum GameState
    {
        Default,
        Invincible,
        Result,
        Max
    }
    GameState m_state;
    [SerializeField]
    HeroController m_hero;
    [SerializeField]
    BGContorller m_bgController;
    [SerializeField]
    Result m_result;
    
    public GameState GetState()
    {
        return m_state;
    }
    public void SetState(GameState state)
    {
        m_state = state;
        switch (state)
        {
            case GameState.Default:
                m_bgController.SetScale(1f);
                MonsterManager.Instance.ResetMonsters(1f);
                break;
            case GameState.Invincible:
                m_hero.SetBuff(HeroBuffController.BuffType.Invincible);
                m_bgController.SetScale(6f);
                MonsterManager.Instance.ResetMonsters(6f);
                break;
            case GameState.Result:
                m_hero.SetDie();
                MonsterManager.Instance.CancelGenerateMonster();
                m_bgController.SetScale(1f);
                m_result.Show();
                break;
        }
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        
    }
}
