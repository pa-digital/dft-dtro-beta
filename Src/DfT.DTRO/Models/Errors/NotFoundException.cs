﻿using System;

namespace DfT.DTRO.Models.Errors;
public class NotFoundException : Exception
{
    public NotFoundException()
        : base("not found")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}