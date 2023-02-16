using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    ParticleSystem[] m_particles;
    bool m_isPlay;
    public void SetEffect()
    {
        m_isPlay = true;
        m_particles[0].Play();
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isPlay)
        {
            bool isPlaying = false;
            for (int i = 0; i < m_particles.Length; i++)
            {
                if (m_particles[i].isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }
            if (!isPlaying)
            {
                EffectManager.Instance.RemoveEffect(this);
            }
        }
    }
}
