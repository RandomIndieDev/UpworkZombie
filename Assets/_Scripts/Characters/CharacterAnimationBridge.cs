using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationBridge : MonoBehaviour
{
    private BaseCharacter character;
    
    void Start()
    {
        character = transform.parent.GetComponent<BaseCharacter>();
    }

    void DoAttack()
    {
        character.TriggerAttack = true;
    }

    void PlayFootStep()
    {
        AudioManager.instance.Play("Runner_FootStep");
    }
    
    
}
