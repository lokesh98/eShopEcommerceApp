using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.Utility
{
    public class FileUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUploadHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
        }
        public string UploadImage(IFormFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }
            string uploadToFolder = Path.Combine(_webHostEnvironment.WebRootPath, "product_img");
            if (!Directory.Exists(uploadToFolder))
            {
                Directory.CreateDirectory(uploadToFolder);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string fullFilePath = Path.Combine(uploadToFolder, fileName);
            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return fileName;
        }
    }
}
