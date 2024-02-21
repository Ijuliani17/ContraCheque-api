namespace Credito.ContraCheque.API.Infrastructure.Extensions
{
    public static class FileExtensions
    {
        public static string ObterConteudoArquivo(this Type type, string path) 
        {
            var fileContent = type.Assembly.GetManifestResourceStream($"{type.Assembly.GetName().Name}.{path}");

            if (fileContent != null)
            {
                using var reader = new StreamReader(fileContent);

                return reader.ReadToEnd();
            }

            return string.Empty;
        }
    }
}
