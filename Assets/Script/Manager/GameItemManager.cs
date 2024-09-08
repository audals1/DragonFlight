using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemManager : SingletonMonoBehaviour<GameItemManager>
{
    public enum ItemType
    {
        None = -1,
        Coin,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        Invincible,
        Magnet,
        Max
    }
    [SerializeField]
    Sprite[] m_itemIcons;
    [SerializeField]
    GameObject m_itemPrefab;
    [SerializeField]
    HeroController m_hero;
    GameObjectPool<GameItem> m_itemPool;
    int[] m_itemTable = {2, 20, 20, 8, 30, 20};

    public float m_moveLimit = -3.5f;
    public float m_maxDuration = 2f;
    public float m_maxDistance;
    public void RemoveItem(GameItem item)
    {
        item.transform.localPosition = Vector3.zero;
        item.gameObject.SetActive(false);
        m_itemPool.Set(item);
    }
    public void CreateItem(Vector3 pos)
    {
        ItemType type = ItemType.None;
        var item = m_itemPool.Get();
        do
        {
            type = (ItemType)Util.GetPriority(m_itemTable);
        } while (type == ItemType.Invincible && GameStateManager.Instance.GetState() == GameStateManager.GameState.Invincible);
        {
            item.SetItem(pos, m_hero.transform.position, type, m_itemIcons[(int)type]);
        }
         
        
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        Vector3 start = new Vector3(0f, 3.1f);
        Vector3 end = new Vector3(0f, m_moveLimit);
        m_maxDistance = (end - start).magnitude; //루트씌운값

        m_itemPool = new GameObjectPool<GameItem>(5, () =>
        {
            var obj = Instantiate(m_itemPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var item = obj.GetComponent<GameItem>();
            item.Initialize(m_hero);
            return item;
        });
    }
}
