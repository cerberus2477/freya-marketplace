using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Model
{
    public class ValidationErrorData : IData
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}