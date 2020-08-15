using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebPortal.Models
{
    public class WebAPIOutputModel<T>
    {
        private WebAPILogModel _transResult { get; set; }
        public WebAPILogModel TransResult
        {
            get
            {
                if (_transResult == null)
                {
                    _transResult = new WebAPILogModel() { APIStatusCode = 200 };
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
