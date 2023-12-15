using System.Security.Cryptography;

namespace GestionnairePhotosConsole;

public static class Detecteur
{
    public static bool AUnDoublon(string original, string imageCourante)
    {
        return Hash(original).SequenceEqual(Hash(imageCourante));
    }

    private static byte[] Hash(string file)
    {
        using var sha256 = SHA256.Create();
        using var input = File.OpenRead(file);

        return sha256.ComputeHash(input);
    }
}