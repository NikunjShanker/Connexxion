using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderParticleSystemsScript : MonoBehaviour
{
    public static BorderParticleSystemsScript instance;

    private ParticleSystem[] borderPS;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        borderPS = new ParticleSystem[8];

        Transform border = GameObject.Find("Border Particle Objects").transform;
        borderPS = new ParticleSystem[border.childCount];
        for (int i = 0; i < borderPS.Length; i++)
        {
            borderPS[i] = border.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    public void changeColors(Color32 color1, Color32 color2)
    {
        Gradient grad = new Gradient();

        Color32 white = new Color32(255, 255, 255, 255);

        GradientColorKey[] gradCK = new GradientColorKey[2];
        GradientColorKey[] gradCK2 = new GradientColorKey[2];

        if (color1.Equals(white))
        {
            if (!color2.Equals(white))
            {
                color1 = color2;
            }
        }
        else if (color2.Equals(white))
        {
            color2 = color1;
        }

        gradCK[0] = new GradientColorKey(color1, 0.5f);
        gradCK[1] = new GradientColorKey(white, 1f);

        gradCK2[0] = new GradientColorKey(color2, 0.5f);
        gradCK2[1] = new GradientColorKey(white, 1f);

        GradientAlphaKey[] gradAlpha = new GradientAlphaKey[1];

        gradAlpha[0] = new GradientAlphaKey(1f, 0f);

        for (int i = 0; i < borderPS.Length; i++)
        {
            ParticleSystem.ColorOverLifetimeModule psColor = borderPS[i].colorOverLifetime;

            if (i > (borderPS.Length)/2 - 1) grad.SetKeys(gradCK, gradAlpha);
            else grad.SetKeys(gradCK2, gradAlpha);

            psColor.color = grad;
        }
    }
}
