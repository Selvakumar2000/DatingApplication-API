using DatingApp.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
                                               int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            //pagination header response as camelCase
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            //custom header
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        //to make this custom header available and exposed the header as part of our response, we use the following cors header
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}