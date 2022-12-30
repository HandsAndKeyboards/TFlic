using System;
using System.Diagnostics.CodeAnalysis;

namespace TFlic.Model.Exceptions;

[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
public class ModelException : Exception
{
    public ModelException(string? message, Exception? innerException) : base(message, innerException) { }
}