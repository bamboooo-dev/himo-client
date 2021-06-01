using System.IO;
using UnityEngine;

public static class Token
{
  public static string getToken()
  {
    string tokenPath = Application.persistentDataPath + "/access-token.jwt";
    string token = File.ReadAllText(tokenPath);
    return "Bearer " + token;
  }
}
