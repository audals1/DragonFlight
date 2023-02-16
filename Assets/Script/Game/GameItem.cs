using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    HeroController m_hero;
    SpriteRenderer m_iconSpr;
    [SerializeField]
    AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    TweenRotation m_tweenRot;
    bool m_isMagnet;
    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    [SerializeField]
    float m_duration = 1f;
    float m_time;

    Vector3 m_target;
    Vector3 m_dir;
    GameItemManager.ItemType m_type;
    IEnumerator Coroutine_PlayCurve()
    {
        float amountX = 0;
        while (m_time < 1f)
        {
            if (!m_isMagnet)
            {
                m_time += Time.deltaTime / m_duration;
                var value = m_curve.Evaluate(m_time);
                var result = m_from * (1f - value) + m_to * value; //커브 좌표 구하는 공식
                m_dir.y = 0f;
                amountX += Mathf.Abs(m_dir.normalized.x) * 0.4f * Time.deltaTime;
                transform.position = result + m_dir.normalized * amountX;
            }
            else
            {
                m_from = transform.position;
                m_to = new Vector3(m_from.x, m_to.y);
                m_duration = GameItemManager.Instance.m_maxDuration * ((m_to - m_from).magnitude / GameItemManager.Instance.m_maxDistance);
            }
                yield return null;
        }
        GameItemManager.Instance.RemoveItem(this);
    }
    public void Initialize(HeroController hero)
    {
        m_hero = hero;
    }
    public void SetItem(Vector3 pos, Vector3 target, GameItemManager.ItemType type, Sprite icon)
    {
        transform.position = pos;
        transform.localRotation = Quaternion.identity;
        m_from = transform.position;
        m_to = new Vector3(transform.position.x, GameItemManager.Instance.m_moveLimit);
        m_duration = GameItemManager.Instance.m_maxDuration * ((m_to - m_from).magnitude / GameItemManager.Instance.m_maxDistance);
        m_iconSpr.sprite = icon;
        m_target = target;
        m_type = type;
        
        m_time = 0f;
        m_dir = target - pos;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine("Coroutine_PlayCurve");
        m_tweenRot.enabled = false;
        if(type >= GameItemManager.ItemType.Gem_Red && type <= GameItemManager.ItemType.Gem_Blue)
        {
            m_tweenRot.enabled = true;
            m_tweenRot.from = Vector3.zero;
            m_tweenRot.to = (m_dir.x < 0f ? Vector3.forward : Vector3.back) * 180f;
            m_tweenRot.duration = 3f;
            m_tweenRot.ResetToBeginning();
            m_tweenRot.PlayForward();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameItemManager.Instance.RemoveItem(this);
            StopAllCoroutines();
            switch (m_type)
            {
                case GameItemManager.ItemType.Coin:
                    GameUIManager.Instance.SetGoldCount(1);
                    SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.get_coin);
                    break;
                case GameItemManager.ItemType.Gem_Red:
                case GameItemManager.ItemType.Gem_Green:
                case GameItemManager.ItemType.Gem_Blue:
                    GameUIManager.Instance.SetGoldCount(((int)m_type)*10);
                    SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.get_gem);
                    break;
                case GameItemManager.ItemType.Invincible:
                    GameStateManager.Instance.SetState(GameStateManager.GameState.Invincible);
                    SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.get_invincible);
                    break;
                case GameItemManager.ItemType.Magnet:
                    m_hero.SetBuff(HeroBuffController.BuffType.Magnet);
                    SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.get_item);
                    break;
            }
        }
        else if(collision.CompareTag("Magnet"))
        {
            m_isMagnet = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Magnet"))
        {
            m_isMagnet = false;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_iconSpr = GetComponent<SpriteRenderer>();
        m_tweenRot = GetComponent<TweenRotation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isMagnet)
        {
            transform.position += (m_hero.transform.position - transform.position).normalized * 10f * Time.deltaTime;
        }
    }
}
