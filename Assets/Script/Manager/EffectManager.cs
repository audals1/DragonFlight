using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [SerializeField]
    GameObject m_explosionPrefab;
    GameObjectPool<EffectController> m_effectPool;

    public void RemoveEffect(EffectController effect)
    {
        effect.gameObject.SetActive(false);
        m_effectPool.Set(effect);
    }
    public void CreateEffect(Vector3 pos)
    {
        var effect = m_effectPool.Get();
        effect.transform.position = pos;
        effect.gameObject.SetActive(true);
        effect.SetEffect();
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_effectPool = new GameObjectPool<EffectController>(5, () =>
        {
            var obj = Instantiate(m_explosionPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var effect = obj.GetComponent<EffectController>();
            return effect;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
