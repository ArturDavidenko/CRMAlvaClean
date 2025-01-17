using Microsoft.AspNetCore.Http;

namespace AlvaCleanCRM.Models.DTOs
{
    public class ImageToUploadDto
    {
        public string employeerId { get; set; }

        public IFormFile Image { get; set; }
    }
}
