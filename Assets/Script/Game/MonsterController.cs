using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    MonsterController m_monster;
    [SerializeField]
    float m_speed = 1f;
    int m_hp;
    int m_hpMax;
    int m_line;
    float m_scale = 1f;
    Animator m_animator;
    MonsterManager.MonsterType m_type;
    public MonsterManager.MonsterType Type { get { return m_type; } }
    public int Line { get { return m_line; } set { m_line = value; } }
    public void SetScale(float scale)
    {
        m_scale = scale;
    }
    public void SetMonster(MonsterManager.MonsterType type)
    {
        m_type = type;
        m_hpMax = ((int)type + 1) * 2;
    }
    public void SetDie()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.mon_die);
        GameUIManager.Instance.SetHuntScore((int)(m_type + 1) * 23);
        EffectManager.Instance.CreateEffect(transform.position);
        GameItemManager.Instance.CreateItem(transform.position);
    }
    public void SetDamage(int attack)
    {
        if (attack < 0)
        {
            m_hp = 0;
        }
        else
        {
            m_hp -= attack;
        }
        m_animator.Play("Hit", 0, 0f);
        if(m_hp <=0)
        {
            if(m_type == MonsterManager.MonsterType.Bomb)
            {
                MonsterManager.Instance.RemoveMonsters(m_line);
            }
            else
            {
                SetDie();
                MonsterManager.Instance.RemoveMonster(this);
            }

        }
    }
    public void Initialized(int line)
    {
        m_hp = m_hpMax;
        m_line = line;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collider_Bottom"))
        {
            MonsterManager.Instance.RemoveMonster(this);
        }
        else if(collision.CompareTag("Invincible"))
        {
            SetDamage(-1);
        }
        else if(collision.CompareTag("Player"))
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Result);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * m_speed * m_scale * Time.deltaTime;

    }
}
