using System;

public class RoomNotFoundException : Exception
{
  public RoomNotFoundException()
    : base("The ID's Room is not found") { }

  public RoomNotFoundException(string message)
      : base(message) { }

  public RoomNotFoundException(string message, Exception innerException)
    : base(message, innerException) { }
}
