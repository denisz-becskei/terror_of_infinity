using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Explode2DSprite : MonoBehaviour
{
    [SerializeField] bool explodeOnClick = true;

    [SerializeField] int gridSize;
    [SerializeField] float explosionForce = 5f;
    [SerializeField] float additionalRandomExplosionForce = 2f;

    [SerializeField] float randomDeform;
    [SerializeField][Range(0, 1)] float partsGravityScale = 1;
    [SerializeField] float partsMass = 1;
    [SerializeField] bool useRandomRotation;

    [SerializeField] float clearTime = 5f;
    [SerializeField] float additionalRandomClearTime = 3f;

    public void Explode()
    {
        Texture2D originalTexture = GetComponent<SpriteRenderer>().sprite.texture;

        int height = originalTexture.height / gridSize;
        int width = originalTexture.width / gridSize;

        for (int y = 0; y < originalTexture.height; y+=8)
        {
            for(int x = 0; x < originalTexture.width; x+=8)
            {
                try
                {
                    var texturePart = GetTexturePart(originalTexture, x, y, width, height);
                    AddExplodingPart(texturePart);
                } catch (ArgumentException)
                {
                    continue;
                }
            }
        }

        Destroy(gameObject);
    }

    Texture2D GetTexturePart(Texture2D tex, int x, int y, int w, int h)
    {
        var partColors = tex.GetPixels(x, y, w, h);
        Texture2D partTexture = new Texture2D(w, h);
        partTexture.SetPixels(0, 0, w, h, partColors);
        partTexture.Apply();
        return partTexture;
    }

    void AddExplodingPart(Texture2D tex)
    {
        var part = new GameObject(gameObject.name + "_part");
        var spriteRenderer = part.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        part.transform.localScale = transform.localScale + new Vector3(Random.Range(-randomDeform, randomDeform), Random.Range(-randomDeform, randomDeform), 0);
        if (useRandomRotation) part.transform.rotation = Random.rotation;

        var rB = part.AddComponent<Rigidbody2D>();
        rB.gravityScale = partsGravityScale;
        rB.mass = partsMass;

        float force = explosionForce + Random.Range(0, additionalRandomExplosionForce);
        part.transform.position = transform.position + Random.insideUnitSphere;
        var forceDirection = (part.transform.position - transform.position).normalized;
        rB.AddForce(forceDirection * force, ForceMode2D.Impulse);

        if (clearTime > 0)
            Destroy(part, clearTime + Random.Range(0, additionalRandomClearTime));
    }
}
