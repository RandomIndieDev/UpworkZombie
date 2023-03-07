using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNameGenerator : MonoBehaviour
{
    public List<string> firstNames;
    public List<string> lastNames;

    public string GetRandomName()
    {
        return firstNames[Random.Range(0, firstNames.Count)] + " " + lastNames[Random.Range(0, lastNames.Count)];
    }
    
}
