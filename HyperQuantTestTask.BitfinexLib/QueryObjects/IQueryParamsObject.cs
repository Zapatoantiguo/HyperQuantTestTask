using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.QueryObjects
{
    public interface IQueryParamsObject
    {
        public bool IsValid();
        public string ToUrlPart();
    }
}
