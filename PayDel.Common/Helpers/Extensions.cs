using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayDel.Data.Dtos;

namespace PayDel.Common.Helpers
{
    public static class Extensions
    {
        public static void AddAppError(this HttpResponse httpResponse, string message)
        {
            var apiError = new ApiReturn<string>
            {
                Status = false,
                Message = message,
                Result = null
            };

            httpResponse.Headers.Add("App-Error", JsonConvert.SerializeObject(apiError));
            httpResponse.Headers.Add("Access-Control-Expose-Header", "App-Error");
            httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response,
           int currentPage, int itemsPerPage,
           int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage,
             totalItems, totalPages);
            var camelCaseFormater = new JsonSerializerSettings();
            camelCaseFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormater));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

        }

        public static int ToAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if(date.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}
