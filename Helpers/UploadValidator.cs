namespace LaudaryMis.Helpers
{
    public static class UploadValidator
    {
        public const long DefaultMaxBytes = 10 * 1024 * 1024; // 10 MB

        public static readonly string[] DocumentExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };

        // Returns an error message, or null when the file is acceptable.
        // Checks the extension whitelist, the size, and that the first bytes
        // match the type the extension claims.
        public static async Task<string?> ValidateAsync(
            IFormFile? file, string[] allowedExtensions, long maxBytes = DefaultMaxBytes)
        {
            if (file == null || file.Length == 0)
                return "Select a file to upload.";

            if (file.Length > maxBytes)
                return $"The file must not be larger than {maxBytes / (1024 * 1024)} MB.";

            var ext = Path.GetExtension(file.FileName ?? "").ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return "Only " + string.Join(", ", allowedExtensions).ToUpperInvariant()
                       + " files can be uploaded.";

            var header = new byte[8];
            int read;
            using (var s = file.OpenReadStream())
            {
                read = await s.ReadAsync(header.AsMemory(0, header.Length));
            }
            if (read < 4) return "The file is not in a valid format.";

            bool ok = ext switch
            {
                ".pdf" => header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46,
                ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
                ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
                _ => false
            };

            return ok ? null : "The file content does not match its extension.";
        }
    }
}
