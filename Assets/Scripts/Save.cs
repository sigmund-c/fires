using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<float> livingTargetPositionsX = new List<float>();
    public List<float> livingTargetPositionsY = new List<float>();
    public List<string> enemyTypes = new List<string>();
    public List<float> enemyPositionsX = new List<float>();
    public List<float> enemyPositionsY = new List<float>();
}
