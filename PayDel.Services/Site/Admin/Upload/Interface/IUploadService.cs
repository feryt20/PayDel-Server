using Microsoft.AspNetCore.Http;
using PayDel.Common.ErrorsAndMessages;
using PayDel.Data.Dtos.Site.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Site.Admin.Upload.Interface
{
    public interface IUploadService
    {
        Task<FileUploadedDto> UploadFile(IFormFile file, string userId, string WebRootPath, string UrlBegan, string UrlUrl);
        Task<FileUploadedDto> UploadFileToLocal(IFormFile file, string userId, string WebRootPath, string UrlBegan, string UrlUrl);
        FileUploadedDto RemoveFileFromLocal(string photoName, string WebRootPath, string filePath);
        ReturnMessage CreateDirectory(string WebRootPath, string UrlUrl);
    }
}
