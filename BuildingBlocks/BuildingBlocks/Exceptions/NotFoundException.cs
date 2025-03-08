﻿namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object key) : base($"The requested {entityName} with ID '{key}' was not found")
    {
    }
}
