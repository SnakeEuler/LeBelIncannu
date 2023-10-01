using System;
using System.Collections;
using System.Collections.Generic;
using Noise;
using UnityEngine;
using static UnityEngine.Vector3;
using Sirenix.OdinInspector;

namespace Q {
public class QSystem: SerializedMonoBehaviour {
  [Header ("Configuration")]
  [SerializeField]
  private RuleData ruleData;
  [SerializeField]
  private LSystemMesh mesh;
  [SerializeField]
  LineRenderer lineRenderer;
  [SerializeField]
  private List<Vector3> linePoints = new List<Vector3> ();
  [SerializeField]
  private float growthFactor = 1f;// Controls the speed of growth.
  [SerializeField]
  private bool isDebugMode = true;

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

  [Header ("Noise Generation")]
  [SerializeField]
  public INoise NoiseGenerator;

  [SerializeField]
  private ProceduralNoise proceduralNoise;

  private int lastProcessedIndex = 0;// Index of the last processed character.
  private Stack<TransformInfo> transformStack = new Stack<TransformInfo> ();

  void Start () {
    lineRenderer = GetComponent<LineRenderer> ();
    if (lineRenderer == null && isDebugMode) {
      lineRenderer = gameObject.AddComponent<LineRenderer> ();
    }
    InitializeSystem ();
    StartCoroutine (GrowOverTime ());
  }

  private IEnumerator GrowOverTime () {
    Debug.Log ("Grow Over Time");

    // Base step count; might be adjusted based on plant's stage or L-System's complexity
    int baseSteps = DetermineBaseSteps (ruleData.baseSteps);

    for (var i = 0;i < ruleData.numberOfGeneration;i ++) {

      // Adjust step count based on environmental factors
      float environmentalFactor = GetEnvironmentalFactor ();
      int adjustedSteps = Mathf.FloorToInt (baseSteps * environmentalFactor);

      Generate (adjustedSteps);

      // Adjust wait time based on environmental factors and growthFactor
      float growthRate = DetermineGrowthRate ();
      yield return new WaitForSeconds (1f / (growthRate * environmentalFactor));
    }
  }

  private int DetermineBaseSteps (int baseSteps) {
    // Determines the base step count based on the plant's current state or
    // the complexity of the L-System.
    if (currentString.Length > 500) {
      baseSteps = 10;
    } else if (currentString.Length > 1000) {
      baseSteps = 15;
    }
    return baseSteps;
  }

  private float DetermineGrowthRate () {
    // Placeholder logic: younger plants grow faster.
    // For demonstration, we assume a plant with a shorter L-System string is younger.
    if (currentString.Length < 500) {
      return growthFactor * 1.5f;// 50% faster
    }
    return growthFactor;// default growth factor
  }


  private float GetEnvironmentalFactor () {
    // Calculates the environmental factor based on light, water, and nutrients.
    var lightFactor = 0.8f;   // Placeholder value
    var waterFactor = 0.9f;   // Placeholder value
    var nutrientFactor = 0.7f;// Placeholder value

    // Combine the factors; for simplicity, we average them
    var environmentalFactor = (0.5f * lightFactor + 0.3f * waterFactor + 0.2f * nutrientFactor);
    return Mathf.Clamp (environmentalFactor, 0f, 1f);// Ensure the value is between 0 and 1
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
    switch (action) {
      case RuleData.Statement.Action.Move:
        PerformMoveAction ();
        break;
      case RuleData.Statement.Action.RotateX:
        PerformRotateAction (Vector3.right);
        break;
      case RuleData.Statement.Action.RotateY:
        PerformRotateAction (Vector3.up);
        break;
      case RuleData.Statement.Action.RotateZ:
        PerformRotateAction (
        forward);// Assuming 'forward' is defined as a class variable for the local Z axis
        break;
      case RuleData.Statement.Action.Push:
        PerformPushAction ();
        break;
      case RuleData.Statement.Action.Pop:
        PerformPopAction ();
        break;
      case RuleData.Statement.Action.Upwards:
        PerformUpwardsAction ();
        break;

      default:
        throw new ArgumentOutOfRangeException (nameof (action), action, null);
    }
  }
  private void PerformMoveAction () {
    float noiseValue
        = proceduralNoise.SampleNoiseValue (transform.position.x, transform.position.y);
    var randomLength
        = length + (2.0f * noiseValue - 1.0f)
        * lengthRandomness;// Map noiseValue from [0,1] to [-1,1]
    var startCircle = mesh.ComputeCircle (transform.position, transform.forward);
    var direction = transform.TransformDirection (Vector3.forward);
    var endPosition = transform.position + direction * randomLength;
    if (isDebugMode) {
      linePoints.Add (transform.position);// Add the current position
      linePoints.Add (endPosition);       // And the end position
      UpdateLineRenderer ();
    } else {
      var endCircle = mesh.ComputeCircle (endPosition, direction);
      mesh.AddTubeSegment (startCircle, endCircle);
    }
    transform.position = endPosition;
  }
  private void PerformRotateAction (Vector3 axis) {
    float noiseValue = proceduralNoise.SampleNoiseValue (transform.rotation.eulerAngles.x,
    transform.rotation.eulerAngles.y);
    var randomAngleAdjustment
        = (2.0f * noiseValue - 1.0f) * angleRandomness;// Map noiseValue from [0,1] to [-1,1]
    var randomAngle = Mathf.Clamp (angle + randomAngleAdjustment, angle - angleVariance,
    angle + angleVariance);
    transform.Rotate (axis * randomAngle);
  }

  private void PerformPushAction () {
    TransformInfo ti = new TransformInfo ();
    ti.Position = transform.position;
    ti.Rotation = transform.rotation;
    transformStack.Push (ti);
  }

  private void PerformPopAction () {
    if (transformStack.Count > 0) {
      TransformInfo ti = transformStack.Pop ();
      transform.position = ti.Position;
      transform.rotation = ti.Rotation;
    }
  }
  private void PerformUpwardsAction () { transform.Rotate (Vector3.right * angle); }


  private void UpdateLineRenderer () {
    lineRenderer.positionCount = linePoints.Count;
    lineRenderer.SetPositions (linePoints.ToArray ());
  }
}
}
