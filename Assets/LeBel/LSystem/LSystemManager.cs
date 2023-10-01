using Sirenix.OdinInspector;
using UnityEngine;

namespace Q
{ public class LSystemManager : MonoBehaviour
  { public LSystem lSystem;

    [Button]
    public void StartTime()
    { // Start the growth of the plant, possibly with time-related logic.
    }

    [Button]
    public void StopTime()
    { // Pause or stop the growth.
    }

    [Button]
    public void Reset()
    { lSystem.InitializeSystem(); // Reset the LSystem to its initial state.
    }

    [Button]
    public void Generate()
    { lSystem.Generate(0); // Call the LSystem's generate method.
    }

    [Button]
    public void SwitchRule()
    { // Logic to switch between different rules or patterns.
    }

// You can add more methods and functionalities as needed.
  } }
