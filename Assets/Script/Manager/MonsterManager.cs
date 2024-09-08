using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonoBehaviour<MonsterManager>
{
    public enum MonsterType
    {
        None = -1,
        White,
        Yellow,
        Pink,
        Bomb,
        Max
    }
    [SerializeField]
    GameObject[] m_monsterPrefabs;
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPool = new Dictionary<MonsterType, GameObjectPool<MonsterController>>();
    List<MonsterController> m_MonsterList = new List<MonsterController>() ;
    Vector2 m_startPos = new Vector2(-7.3f, 5.99f);
    float m_posGap = 0.64f;
    float m_scale = 1f;
    float m_spawnInterval=  4f;
    public void CancelGenerateMonster()
    {
        CancelInvoke("CreateMonster");
    }
    public void ResetMonsters(float scale)
    {
        CancelInvoke("CreateMonster");
        InvokeRepeating("CreateMonster", 0.5f, m_spawnInterval / scale);

        for (int i = 0; i < m_MonsterList.Count; i++)
        {
            m_MonsterList[i].SetScale(scale);
        }
        m_scale = scale;
    }
    public void ClearMonsters()
    {
        for (int i = 0; i < m_MonsterList.Count; i++)
        {
            ResetMonster(m_MonsterList[i]);
            m_MonsterList[i].SetDie();
        }
        m_MonsterList.RemoveAll(monster => monster.gameObject.activeSelf == false);
    }
    int m_line;
    public void RemoveMonsters(int line)
    {
        for (int i = 0; i < m_MonsterList.Count; i++)
        {
            if(m_MonsterList[i].Line == line)
            {
                ResetMonster(m_MonsterList[i]);
                m_MonsterList[i].SetDie();
            }
        }
        m_MonsterList.RemoveAll(monster => monster.gameObject.activeSelf == false);
    }
    void ResetMonster(MonsterController monster)
    {
        
        monster.gameObject.SetActive(false);
        m_monsterPool[monster.Type].Set(monster);
    }
    public void RemoveMonster(MonsterController monster)
    {
        if (m_MonsterList.Remove(monster))
        {
            monster.gameObject.SetActive(false);
            m_monsterPool[monster.Type].Set(monster);
        }
    }

    void CreateMonster()
    {
        MonsterType type = MonsterType.None;
        bool isBomb = false;
        bool isTry;
        m_line++;
        for (int i = 0; i < 5; i++)
        {
            do
            {
                isTry = false;
                type = (MonsterType)Random.Range((int)MonsterType.White, (int)MonsterType.Max);
                if (type == MonsterType.Bomb)
                {
                    if (!isBomb)
                    {
                        isBomb = true;
                    }
                    else
                    {
                        isTry = true;
                    }
                }
            } while (isTry);
            var monster = m_monsterPool[type].Get();
            monster.Initialized(m_line);
            monster.SetScale(m_scale);
            monster.gameObject.SetActive(true);
            monster.transform.position = m_startPos + Vector2.right * i * m_posGap;
            m_MonsterList.Add(monster);
        }
    }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_monsterPrefabs = Resources.LoadAll<GameObject>("Monster");
        //for (int i = 0; i < m_monsterPrefabs.Length; i++)
        foreach(var prefab in m_monsterPrefabs)
        {
            var results = prefab.name.Split('.');
            MonsterType type = (MonsterType)(int.Parse(results[0]) - 1);
            var pool = new GameObjectPool<MonsterController>(3, () =>
            {
                var obj = Instantiate(prefab);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = prefab.transform.localScale;
                obj.SetActive(false);
                var monster = obj.GetComponent<MonsterController>();
                monster.SetMonster(type);
                return monster;
            });
            m_monsterPool.Add(type, pool);
        }
        
        InvokeRepeating("CreateMonster", 3f, m_spawnInterval);

    } 
}




