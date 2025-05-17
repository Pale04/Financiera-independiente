using Data_Access.Entities;
using Microsoft.Extensions.Logging;

namespace Data_Access
{
    public class DocumentManager
    {
        private ILogger<DocumentManager> _logger;
        private string _path = "creditDocuments/";

        public string? SaveDocument(Document documentInfo, byte[] file)
        {
            string ext = Path.GetExtension(documentInfo.name);
            string date = DateTime.Now.ToString("dd-MM-yy-mm-ss");
            string rnd = new Random().Next(10, 90).ToString();
            string name = $"{documentInfo.creditId}-{date}-{rnd}{ext}";

            try
            {
                File.WriteAllBytes(string.Format("{0}{1}", _path, documentInfo.name), file);
            }
            catch (Exception error)
            {
                _logger.LogError("Error saving file {}: {}", name, error);
                return null;
            }

            return name;
        }
    }
}
