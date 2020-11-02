using System;

namespace Freyr.EditorMethod
{
    [Serializable]
    public class Conditional<T>
    {
        [MethodList(TargetType = typeof(object))]
        public MethodData<bool, T> Condition;
        [MethodList(TargetType = typeof(object))]
        public MethodData<object, T> OnSuccess;

        public bool Try(T value)
        {
            bool successful = Condition.Invoke(value);
            if (successful)
                OnSuccess.Invoke(value);
            return successful;
        }
    }
}