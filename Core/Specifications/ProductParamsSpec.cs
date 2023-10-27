using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductParamsSpec
    {
        public int MaxPageSize = 50;
        public int MinPageIndex = 1;
        public string Sort {set;get;}

        public int PageIndex{
            get => _pageIndex;
            set => _pageIndex = (value < 1 || string.IsNullOrEmpty(value.ToString())) ? MinPageIndex : value;
        }

        private int _pageSize;
        private int _pageIndex {get;set;} = 1;

        public int PageSize
        {
            get => _pageSize = (_pageSize > 0) ? _pageSize :  MaxPageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId{get;set;}

        public int? TypeId{get;set;}

        private string _search;

        public string Search{
            get => _search;
            set => _search =  value.ToLower();
        }
    }
}