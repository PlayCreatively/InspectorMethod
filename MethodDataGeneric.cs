using System;


namespace Freyr.EditorMethod
{
    [Serializable]
    public class MethodData<TOut> : MethodData
    {
        public MethodData(string methodName, Type targetType) : base(methodName, targetType) { }
        public MethodData(Func<TOut> method) : base(method.Method) => this.method = method;
        public Func<TOut> GetMethod => method ?? (method = ToDelegate<Func<TOut>>());
        Func<TOut> method;

        public static implicit operator Func<TOut>(MethodData<TOut> methodData) => methodData.GetMethod;
        public static implicit operator MethodData<TOut>(Func<TOut> method) => new MethodData<TOut>(method);

        public TOut Invoke() => GetMethod();
    }

    [Serializable]
    public class MethodData<TIn, TOut> : MethodData
    {
        public MethodData(string methodName, Type targetType) : base(methodName, targetType) { }
        public MethodData(Func<TIn, TOut> method) : base(method.Method) => this.method = method;
        public Func<TIn, TOut> GetMethod => method ?? (method = ToDelegate<Func<TIn, TOut>>());
        Func<TIn, TOut> method;

        public static implicit operator Func<TIn, TOut>(MethodData<TIn, TOut> methodData) => methodData.GetMethod;
        public static implicit operator MethodData<TIn, TOut>(Func<TIn, TOut> method) => new MethodData<TIn, TOut>(method);

        public TOut Invoke(TIn value) => GetMethod(value);
    }

    [Serializable]
    public class MethodData<TIn1, TIn2, TOut> : MethodData
    {
        public MethodData(string methodName, Type targetType) : base(methodName, targetType) { }
        public MethodData(Func<TIn1, TIn2, TOut> method) : base(method.Method) => this.method = method;
        public Func<TIn1, TIn2, TOut> GetMethod => method ?? (method = ToDelegate<Func<TIn1, TIn2, TOut>>());
        Func<TIn1, TIn2, TOut> method;

        public static implicit operator Func<TIn1, TIn2, TOut>(MethodData<TIn1, TIn2, TOut> methodData) => methodData.GetMethod;
        public static implicit operator MethodData<TIn1, TIn2, TOut>(Func<TIn1, TIn2, TOut> method) => new MethodData<TIn1, TIn2, TOut>(method);

        public TOut Invoke(TIn1 value1, TIn2 value2) => GetMethod(value1, value2);
    }

    [Serializable]
    public class MethodData<TIn1, TIn2, TIn3, TOut> : MethodData
    {
        public MethodData(string methodName, Type targetType) : base(methodName, targetType) { }
        public MethodData(Func<TIn1, TIn2, TIn3, TOut> method) : base(method.Method) => this.method = method;
        public Func<TIn1, TIn2, TIn3, TOut> GetMethod => method ?? (method = ToDelegate<Func<TIn1, TIn2, TIn3, TOut>>());
        Func<TIn1, TIn2, TIn3, TOut> method;

        public static implicit operator Func<TIn1, TIn2, TIn3, TOut>(MethodData<TIn1, TIn2, TIn3, TOut> methodData) => methodData.GetMethod;
        public static implicit operator MethodData<TIn1, TIn2, TIn3, TOut>(Func<TIn1, TIn2, TIn3, TOut> method) => new MethodData<TIn1, TIn2, TIn3, TOut>(method);

        public TOut Invoke(TIn1 value1, TIn2 value2, TIn3 value3) => GetMethod(value1, value2, value3);
    }

}
