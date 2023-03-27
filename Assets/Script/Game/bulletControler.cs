using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletControler : MonoBehaviour
{
    #region constants and fields
    [SerializeField]
    float m_speed = 15f;
    [SerializeField]
    float m_duration = 2f;
    [SerializeField]
    HeroController m_hero;
    int m_atk = 1;
    #endregion
    #region public Methods and Properties
    public void Initialize(HeroController hero)
    {
        m_hero = hero;
    }
    public void SetBullet(Vector3 pos)
    {
        transform.position = pos;
        if (IsInvoking("Remove"))
            CancelInvoke("Remove");
        Invoke("Remove", m_duration);
    }
    #endregion
    #region Methods
    void Remove()
    {
        m_hero.RemoveBullet(this);
    }
    #endregion
    #region Unity Methods
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
           var monster = collision.gameObject.GetComponent<MonsterController>();
            monster.SetDamage(m_atk);
           m_hero.RemoveBullet(this);
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      transform.position += Vector3.up * m_speed * Time.deltaTime;
    }
    #endregion
}
