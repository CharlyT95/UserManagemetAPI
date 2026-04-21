namespace Aduanas.Aci.Usuarios.Api.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizarTexto(this string texto)
        {
            return texto?
                .Trim()
                .Replace(" ", "")
                .ToLower();
        }
    }
}
