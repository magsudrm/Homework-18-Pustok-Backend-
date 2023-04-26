using Pustok.Models;

namespace Pustok.Helpers
{
    public static class FileManager
    {
        public static string Save(IFormFile file, string folder)
        {
            string newFileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folder, newFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return newFileName;
        }

        public static bool Delete(string folder, string fileName)
        {
            string path = folder?.TrimEnd('\\', '/') + "/" + fileName?.TrimStart('\\', '/');

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public static void DeleteAll(string folder, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
            string path = folder?.TrimEnd('\\', '/') + "/" + fileName?.TrimStart('\\', '/');

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }


    }
}
