using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<float> livingTargetPositionsX = new List<float>();
    public List<float> livingTargetPositionsY = new List<float>();
    public List<int> livingTargetsTypes = new List<int>();

    public int hits = 0;
    public int shots = 0;
}
