using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObject : ScriptableObject
{
    // le minimum que tu peux avoir
public int minHealth;
public int minIntelligence;
public int minStrength;

//le maximum que tu peux avoir
public int maxHealth;
public int maxIntelligence;
public int maxStrength;

//combien par palier tu rajoute pour ecraser le minimum
public float multiplierHealthPerPalier;
public float multiplierIntelligencePerPalier;
public float multiplierStrengthPerPalier;

//combien au max tu peux avoir en plus du minimum
public float multiplierHealth;
public float multiplierIntelligence;
public float multiplierStrength;


}
