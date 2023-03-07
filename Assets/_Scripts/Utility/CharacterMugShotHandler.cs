using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMugShotHandler : MonoBehaviour
{

    [Header("References")] 
    public GameObject characterLocation;
    
    private Animator animator;

    void Start()
    {
        UIEvents.Instance.OnRenderCharacter += RenderCharacter;
    }

    void OnDestroy()
    {
        UIEvents.Instance.OnRenderCharacter -= RenderCharacter;
    }

    public void RenderCharacter(GameObject character)
    {
        if (characterLocation.transform.childCount >= 1)
        {
            Destroy(characterLocation.transform.GetChild(0).gameObject);
        }

        var spawnedChar = Instantiate(character, characterLocation.transform.position, Quaternion.identity, characterLocation.transform);
        spawnedChar.transform.localScale = Vector3.one;
        spawnedChar.GetComponent<Animator>().SetInteger("Animation_int", 2);
    }
    
    
}
