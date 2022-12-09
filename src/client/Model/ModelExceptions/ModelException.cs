using System;

namespace TFlic.Model.ModelExceptions;

public class ModelException : Exception
{
    // ReSharper disable once MemberCanBeProtected.Global
    public ModelException(string message) : base(message) { }
}