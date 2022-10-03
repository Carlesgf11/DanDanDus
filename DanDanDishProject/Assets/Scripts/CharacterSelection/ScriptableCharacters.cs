using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/New Character")]
public class ScriptableCharacters : ScriptableObject
{
    //Prueba Commit

    public int index;
    public Sprite charImage;
    public Sprite flagSprite;
    public GameObject bodyParts;
    public GameObject anim;
    public Color UIColor;
    public RuntimeAnimatorController flagAnim;
}

