using System.Reflection;
using UnityEngine;

namespace CleanEvents
{
    public class ListenerModel
    {
        public object TargetObject;

        public MethodInfo Method;

        public void Call(EventModel info)
        {
            try
            {
                if (Method.GetParameters().Length == 0)
                    Method.Invoke(TargetObject, null);
                else
                    Method.Invoke(TargetObject, new object[] { info });
            }
            catch
            {
                Debug.LogError($"Method invocation on object '{TargetObject}' @ '{Method.Name}()' failed. Please make sure nothing is null, specially Payload.");
            }
        }
    }
}
