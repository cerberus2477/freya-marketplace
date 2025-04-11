using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Model
{
    public interface IApiResponse
    {
        int Status { get; set; }
        string Message { get; set; }

        string IData { get; set; }
    }

    public interface IData
    {
    }

}
