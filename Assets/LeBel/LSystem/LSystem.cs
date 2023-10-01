using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Vector3;
using Random = UnityEngine.Random;
namespace Q {
public class LSystem: MonoBehaviour {
  [Header ("Configuration")]
  [SerializeField]
  private RuleData ruleData;
  [SerializeField]
  private LSystemMesh mesh;
  [SerializeField]
  private float growthFactor = 1f;// Controls the speed of growth.

  [Header ("LSystem Parameters")]
  [SerializeField]
  private string currentString;
  [SerializeField]
  private float length, angle;
  [SerializeField]
  private float angleRandomness;
  [SerializeField]
  private float angleVariance;
  [SerializeField]
  private float lengthRandomness;
  [SerializeField]
  private float branchingProbability;

  private int lastProcessedIndex = 0;// Index of the last processed character.
  private Stack<TransformInfo> transformStack = new Stack<TransformInfo> ();

  void Start () {
    InitializeSystem ();
    StartCoroutine (GrowOverTime ());
  }

  private IEnumerator GrowOverTime () {
    Debug.Log("Grow Over Time");
    for (var i = 0;i < ruleData.numberOfGeneration;i ++) {
      Generate (5);// Generate 5 steps at a time, adjust as needed.
      yield return new WaitForSeconds (1f / growthFactor);
    }
    mesh.FinalizeMesh ();
  }


  public void InitializeSystem () {
    currentString = ruleData.rules[0].character.ToString ();
    length = ruleData.length;
    angle = ruleData.angle;
    angleRandomness = ruleData.angleRandomness;
    angleVariance = ruleData.angleVariance;
    lengthRandomness = ruleData.lengthRandomness;
    branchingProbability = ruleData.branchingProbability;


  }
  public class TransformInfo {
    public Vector3 Position;
    public Quaternion Rotation;
  }

  public void Generate (int steps) {
    length /= 2.0f;
    string newString = "";

    foreach (var t in ruleData.rules) {
      foreach (var t1 in currentString) {
        if (t.character == t1) {
          newString += t.pattern;
        } else {
          newString += t1;
        }
      }
    }

    currentString = newString;
    Debug.Log (currentString);

    int processedSteps = 0;
    for (int index = lastProcessedIndex;processedSteps < steps && index < currentString.Length;
      index ++, processedSteps ++) {
      var t = currentString[index];
      foreach (var t1 in ruleData.statements) {
        if (t == t1.character) {
          DoAction (t1.action);
        }
      }
    }

    // Update the last processed index for next time
    lastProcessedIndex += processedSteps;
  }


  private void DoAction (RuleData.Statement.Action action) {
    Debug.Log ("Do Action Called: " + action);
    TransformInfo ti;
    switch (action) {
      case RuleData.Statement.Action.Move:
        var randomLength = length + Random.Range (-lengthRandomness, lengthRandomness);
        var startCircle = mesh.ComputeCircle (transform.position, transform.forward);
        var direction = transform.TransformDirection (Vector3.forward);
        var endPosition = transform.position + direction * randomLength;
        var endCircle = mesh.ComputeCircle (endPosition, direction);
        mesh.AddTubeSegment (startCircle, endCircle);
        transform.position
            = endPosition;// Move the LSystem's position, but without visually moving the entire GameObject.
        break;


      case RuleData.Statement.Action.RotateX:
        var randomAngleAdjustmentX = Random.Range (-angleRandomness, angleRandomness);
        var randomAngleX = Mathf.Clamp (angle + randomAngleAdjustmentX, angle - angleVariance,
        angle + angleVariance);
        transform.Rotate (Vector3.right * randomAngleX);
        break;

      case RuleData.Statement.Action.RotateY:
        var randomAngleAdjustmentY = Random.Range (-angleRandomness, angleRandomness);
        var randomAngleY = Mathf.Clamp (angle + randomAngleAdjustmentY, angle - angleVariance,
        angle + angleVariance);
        transform.Rotate (Vector3.up * randomAngleY);
        break;

      case RuleData.Statement.Action.RotateZ:
        var randomAngleAdjustmentZ = Random.Range (-angleRandomness, angleRandomness);
        var randomAngleZ = Mathf.Clamp (angle + randomAngleAdjustmentZ, angle - angleVariance,
        angle + angleVariance);
        transform.Rotate (forward * angle);// Rotate around local Z axis
        break;

      case RuleData.Statement.Action.Push:
        // Save current state (position and rotation).
        ti = new TransformInfo ();
        ti.Position = transform.position;
        ti.Rotation = transform.rotation;
        transformStack.Push (ti);
        break;

      case RuleData.Statement.Action.Pop:
        // Restore last saved state.
        if (transformStack.Count > 0) {
          ti = transformStack.Pop ();
          transform.position = ti.Position;
          transform.rotation = ti.Rotation;
        }
        break;
      default:
        throw new ArgumentOutOfRangeException (nameof (action), action, null);
    }
  }
}
}
