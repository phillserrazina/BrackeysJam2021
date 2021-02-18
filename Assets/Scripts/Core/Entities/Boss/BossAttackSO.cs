using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Boss Attack", fileName="New Boss Attack")]
public class BossAttackSO : ScriptableObject
{
    public string Name = "";
    public int AnimationIndex = 0;
}
