using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebPortal.Model
{
    public class WebAPIOutput<T>
    {
        private WebAPILog _transResult { get; set; }
        public WebAPILog TransResult
        {
            get
            {
                if (_transResult == null)
                {
                    _transResult = new WebAPILog() { APIStatusCode = 200 };
                }
                return _transResult;
            }
            set { _transResult = value; }
        }

        private List<T> _dataList { get; set; }
        public List<T> DataList
        {
            get
            {
                if (_dataList == null)
                {
                    _dataList = new List<T>();
                }
                return _dataList;
            }
            set { _dataList = value; }
        }
    }
}
