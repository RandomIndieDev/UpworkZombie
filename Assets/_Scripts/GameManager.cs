using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.SetTweensCapacity(500, 50);
    }

}
