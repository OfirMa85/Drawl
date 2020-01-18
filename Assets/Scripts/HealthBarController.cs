using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    Sprite[] sprites;
    public int dir;

    void Start()
    {
        Texture texture = GetComponent<SpriteRenderer>().sprite.texture;
        sprites = Resources.LoadAll<Sprite>(texture.name);
    }

    public void GotHit(float health, float maxHealth)
    {
        int displayIndex = (int)((maxHealth - health) / maxHealth * ((float)sprites.Length - 1));
        GetComponent<SpriteRenderer>().sprite = sprites[displayIndex];
        if (displayIndex != sprites.Length - 1)
            transform.localPosition = new Vector2(dir*-(float)displayIndex / 64, 0);
        else
            transform.localPosition = new Vector2(dir*-((float)displayIndex + 2) / 64, 0);
    }
}
