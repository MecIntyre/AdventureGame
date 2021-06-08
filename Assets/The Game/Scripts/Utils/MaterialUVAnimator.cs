using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Verschiebt den material-Offset ("UV"), so dass eine Animation entsteht.
/// </summary>
public class MaterialUVAnimator : MonoBehaviour
{
    /// <summary>
    /// Das Material, dessen Offset verändert wird.
    /// </summary>
    public Material material;

    private IEnumerator Start()
    {
        material.mainTextureOffset = Vector2.zero;
        Vector2 delta = new Vector2(1f / 64f, 0f);

        while (enabled)
        {
            material.mainTextureOffset += delta;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
