using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PowerupEffect : ScriptableObject
{
    void Apply(GameObject target);
}
