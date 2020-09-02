using Microsoft.AspNetCore.Http;
using PayDel.Common.ErrorsAndMessages;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Upload.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Site.Admin.Upload.Service
{
    public class UploadService : IUploadService
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        public UploadService(IUnitOfWork<PayDelDbContext> dbContext)
        {
            _db = dbContext;
        }
        public async Task<FileUploadedDto> UploadFile(IFormFile file, string userId, string WebRootPath, string UrlBegan, string Url)
        {
            return await UploadFileToLocal(file, userId, WebRootPath, UrlBegan, Url);
        }

        public async Task<FileUploadedDto> UploadFileToLocal(IFormFile file, string userId,
            string WebRootPath, string UrlBegan, string Url)
        {
            if (file.Length > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string fileExtention = Path.GetExtension(fileName);
                    string fileNewName = string.Format("{0}{1}", userId, fileExtention);
                    string path = Path.Combine(WebRootPath, Url);
                    string fullPath = Path.Combine(path, fileNewName);

                    var dirRes = CreateDirectory(WebRootPath, Url);
                    if (!dirRes.status)
                    {
                        return new FileUploadedDto()
                        {
                            Status = false,
                            Message = dirRes.message
                        };
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return new FileUploadedDto()
                    {
                        Status = true,
                        LocalUploaded = true,
                        Message = "با موفقیت در لوکال آپلود شد",
                        PublicId = "0",
                        Url = $"{UrlBegan}/{"wwwroot/" + Url.Split('\\').Aggregate("", (current, str) => current + (str + "/")) + fileNewName}"
                    };

                }
                catch (Exception ex)
                {
                    return new FileUploadedDto()
                    {
                        Status = false,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                return new FileUploadedDto()
                {
                    Status = false,
                    Message = "فایلی برای اپلود یافت نشد"
                };
            }
        }

        

        public FileUploadedDto RemoveFileFromLocal(string photoName, string WebRootPath, string filePath)
        {

            string path = Path.Combine(WebRootPath, filePath);
            string fullPath = Path.Combine(path, photoName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return new FileUploadedDto()
                {
                    Status = true,
                    Message = "فایل با موفقیت حذف شد"
                };
            }
            else
            {
                return new FileUploadedDto()
                {
                    Status = true,
                    Message = "فایل وجود نداشت"
                };
            }
        }

        public ReturnMessage CreateDirectory(string WebRootPath, string Url)
        {
            try
            {
                var path = Path.Combine(WebRootPath, Url);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return new ReturnMessage
                {
                    status = true
                };

            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    status = false,
                    message = ex.Message
                };
            }

        }

    }
}
