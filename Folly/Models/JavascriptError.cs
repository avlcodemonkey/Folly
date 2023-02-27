﻿using System;

namespace Folly.Models;

public record JavascriptError
{
    public string Message { get; set; }
}

[Serializable]
public class JavaScriptException : Exception
{
    protected JavaScriptException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) => throw new NotImplementedException();

    public JavaScriptException(string message) : base(message)
    {
    }

    public JavaScriptException()
    { }

    public JavaScriptException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
