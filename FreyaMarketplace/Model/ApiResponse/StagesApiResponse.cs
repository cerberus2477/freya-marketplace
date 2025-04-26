using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Model
{
    internal class StagesApiResponse : IApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public List<Stage> Data { get; set; }
    }
}
