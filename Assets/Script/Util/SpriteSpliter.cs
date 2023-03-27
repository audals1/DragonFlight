using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteSpliter : MonoBehaviour
{
    [SerializeField]
    UITexture m_uiTexture;
    Sprite[] m_sprites;
    // Start is called before the first frame update
    void Start()
    {
        m_sprites = Resources.LoadAll<Sprite>("Fonts");
        Debug.Log(m_sprites.Length);
        for (int i = 0; i < m_sprites.Length; i++)
        {
            Sprite spr = m_sprites[i];
            int width = (int)spr.rect.width;
            int height = (int)spr.rect.height;
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, spr.texture.GetPixel((int)spr.rect.x+x,(int)spr.rect.y+y));
                }
            }
            texture.Apply();
            var image = texture.EncodeToPNG();
            File.WriteAllBytes(string.Format(@"c:\test\imagefont{0:00}.png", i+1), image);
            m_uiTexture.mainTexture = texture;
            m_uiTexture.MakePixelPerfect();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
