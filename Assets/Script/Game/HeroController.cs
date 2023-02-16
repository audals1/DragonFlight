using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    Animator m_animator;
    [Header("주인공 스프라이트 Parts")]
    [SerializeField]
    SpriteRenderer[] m_heroParts;
    [Header("주인공 버프")]
    [SerializeField]
    HeroBuffController m_buffController;
    [SerializeField]
    Vector3 m_dir;
    [Range(2f, 10f)]//최소값 최대값 지정
    [SerializeField]
    float m_speed = 4f;
    bool m_isDrag;
    Vector3 m_startPos;
    [SerializeField]
    GameObject m_bulletPrefab;
    [SerializeField]
    Transform m_firePos;
    [Header("이펙트 오브젝트")]//메뉴에 제목달기
    [SerializeField]
    GameObject m_vfxShockWaveobj;
    TweenScale m_shockwavetweenScale;
    [SerializeField]
    GameObject m_VfxMagnetObj;
    [SerializeField]
    GameObject m_vfxInvincible;
    GameObjectPool<bulletControler> m_bulletPool;
    bool m_isShockWave;
    public void SetDie()
    {
        gameObject.SetActive(false);
        CancelInvoke("CreateBullet");
    }
    public void SetBuff(HeroBuffController.BuffType type)
    {
        m_buffController.SetBuff(type);
    }
    public void SetInvincibleEffect(bool isActive)
    {
        
        if(isActive)
        {
            CancelInvoke("CreateBullet");
            m_vfxInvincible.SetActive(true);
            m_animator.Play("Invincible");
        }
        else
        {
            InvokeRepeating("CreateBullet",0.15f,0.15f);
            m_vfxInvincible.SetActive(false);
            m_animator.Play("Idle3");
        }
    }
    public void SetShockWaveEffect()
    {
        m_vfxShockWaveobj.SetActive(true);
        MonsterManager.Instance.ClearMonsters();
        m_shockwavetweenScale.ResetToBeginning();
        m_shockwavetweenScale.PlayForward();      
    }
    public void EndShockWave()
    {
        m_vfxShockWaveobj.SetActive(false);
    }
    public void SetMagnetEffect(bool isActive)
    {
        m_VfxMagnetObj.SetActive(isActive);     
    }

    public void RemoveBullet(bulletControler bullet)
    {
        bullet.gameObject.SetActive(false);
        m_bulletPool.Set(bullet);
    }
    void CreateBullet()
    {
        var bullet = m_bulletPool.Get();
        bullet.gameObject.SetActive(true);
        bullet.SetBullet(m_firePos.position);
        
    }
    void LoadHeroSprite()
    {
        var path = string.Format("Heros/sunny_{0:00}", PlayerDataManager.Instance.HeroIndex + 1);
        var sprites = Resources.LoadAll<Sprite>(path);
        m_heroParts[0].sprite = sprites[0];
        m_heroParts[1].sprite = m_heroParts[2].sprite = sprites[1];
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_bulletPool = new GameObjectPool<bulletControler>(10, () =>
        {
            var obj = Instantiate(m_bulletPrefab);
            obj.SetActive(false);
            var bullet = obj.GetComponent<bulletControler>();
            bullet.Initialize(this);
            return bullet;
        });
        m_VfxMagnetObj.SetActive(false);
        m_vfxInvincible.SetActive(false);
        m_shockwavetweenScale = m_vfxShockWaveobj.GetComponentInChildren<TweenScale>();
        m_vfxShockWaveobj.SetActive(false);
        LoadHeroSprite();
        InvokeRepeating("CreateBullet", 3f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        m_dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f);

        if (Input.GetMouseButtonDown(0))
        {
            m_isDrag = true;
            m_startPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
        }
        if(m_isDrag)
        {
            var endPos = Input.mousePosition;
            var result = endPos - m_startPos;
            m_dir = new Vector3(result.x, 0f);
            m_startPos = endPos;
        }
        transform.position += m_dir * m_speed * Time.deltaTime;
        
    }

}
            
    
