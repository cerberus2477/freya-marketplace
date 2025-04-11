using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreyaMarketplace.Model.ApiResponse
{
    internal class ListingsApiResponse : IApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public ListingsData Data { get; set; }

    }

    public class ListingsData : IData
    {
        public List<Listing> listingList;
    }

}