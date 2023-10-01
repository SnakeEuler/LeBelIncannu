using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LSystem", menuName = "LSystem/LSystemObj")]
public class RuleData: ScriptableObject {
  public string axiom = "";
  public float length = 5, angle = 25;

  public float angleRandomness = 5.0f;

  public float lengthRandomness = .1f;

  public float branchingProbability = .8f;

  public int numberOfGeneration = 3;

  public float angleVariance = 15f;

  public int baseSteps = 5;

  [System.Serializable]
  public class Rules {
    public char character;
    public string pattern;
  }

  [System.Serializable]
  public class Statement {
    public char character;

    public enum Action { Move, Push, Pop, RotateX, RotateY, RotateZ, Upwards }

    public Action action;
  }

  public List<Statement> statements = new List<Statement> ();

  public List<Rules> rules = new List<Rules> ();
}
