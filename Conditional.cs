using System;

namespace Freyr.EditorMethod
{
    [Serializable]
    public class Conditional<T>
    {
        [MethodList]
        public MethodData<T, bool> Condition;
        [MethodList]
        public MethodData<T, object> OnSuccess;

        public bool Try(T value)
        {
            bool successful = Condition.Invoke(value);
            if (successful)
                OnSuccess.Invoke(value);
            return successful;
        }
    }
}