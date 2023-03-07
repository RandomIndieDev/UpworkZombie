using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomizer : MonoBehaviour
{
    public List<GameObject> enemySkins;

    public Renderer renderer;

    public void ActivateRandomSkin()
    {
        var value = Random.Range(0, enemySkins.Count);
        enemySkins[value].SetActive(true);
        renderer = enemySkins[value].GetComponent<Renderer>();
    }
}
