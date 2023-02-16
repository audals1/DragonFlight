using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBuffController : MonoBehaviour
{
    HeroController m_hero;
    CameraShake m_camShake;
    public enum BuffType
    {
        Invincible,
        Magnet,
        Max
    }

    Dictionary<BuffType, float> m_buffList = new Dictionary<BuffType, float>();
    List<float>m_buffDuration = new List<float>() { 3f, 5f };

    IEnumerator Coroutine_BuffProcess(BuffType type)
    {
        switch (type)
        {
            case BuffType.Invincible:
                m_hero.SetInvincibleEffect(true);
                break;
            case BuffType.Magnet:
                m_hero.SetMagnetEffect(true);
                break;
        }
        while (true)
        {
            m_buffList[type] += Time.deltaTime;
            if (m_buffList[type] > m_buffDuration[(int)type])
            {
                switch (type)
                {
                    case BuffType.Invincible:
                        m_hero.SetInvincibleEffect(false);
                        m_hero.SetShockWaveEffect();
                        GameStateManager.Instance.SetState(GameStateManager.GameState.Default);
                        break;
                    case BuffType.Magnet:
                        m_hero.SetMagnetEffect(false);
                        break;
                }
                m_buffList.Remove(type);
                yield break;
            }
            yield return null;
        }
    }
    public void SetBuff(BuffType type)
    {
        if(m_buffList.ContainsKey(type))
        {
            m_buffList[type] = 0f;
        }
        else
        {
            m_buffList.Add(type, 0f);
            StartCoroutine(Coroutine_BuffProcess(type));
        }
        if(type == BuffType.Invincible)
        {
            m_camShake.PlayShake(m_buffDuration[(int)type]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_hero = GetComponent<HeroController>();
        m_camShake = Camera.main.GetComponent<CameraShake>();
    }
}
