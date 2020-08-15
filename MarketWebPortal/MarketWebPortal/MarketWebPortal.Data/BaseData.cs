using MarketWebPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketWebPortal.Data
{
    public class BaseData
    {
        private WebAPILog _transResult { get; set; }
        public WebAPILog TransResult
        {
            get
            {
                if (_transResult == null)
                {
                    _transResult = new WebAPILog();
                    _transResult.SQLCode = 0;
                }
                return _transResult;
            }
            set { _transResult = value; }
        }

        internal string SafeCastStringParam(string Value)
        {
            return (!string.IsNullOrEmpty(Value)) ? Value : string.Empty;
        }

        internal string SafeParseFirstCharStringParam(string Value)
        {
            return (!string.IsNullOrEmpty(Value)) ? Value.FirstOrDefault().ToString() : string.Empty;
        }

        internal string ParseNullableDate(DateTime? DateValue)
        {
            return DateValue.HasValue ? DateValue.Value.ToShortDateString() : null;
        }

        internal string CastInDevKeyParam(string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                return (string.IsNullOrEmpty(Value.Trim()) ? " " : Value);
            }
            else
            {
                return " ";
            }
        }

        internal string ConvertYesNoToActiveFlag(string Value)
        {
            return Value.ToLower().Equals("yes") ? "A" : "I";
        }

        internal bool ConvertYesNoToBoolFlag(string Value)
        {
            return Value.ToLower().FirstOrDefault().Equals('y');
        }

        internal string ConvertBoolToActiveFlag(bool Value)
        {
            return (Value) ? "A" : "I";
        }
    }
}
