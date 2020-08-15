using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MarketWebAPI.Tests.Common
{
    public class MockDataReader : IDataReader
    {
        private int rowCounter = 0;
        private readonly List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();

        public MockDataReader(List<Dictionary<string, object>> records)
        {
            this.records = records;
        }

        public bool Read()
        {
            rowCounter++;

            if (rowCounter <= records.Count)
            {
                return true;
            }

            return false;
        }

        public object this[string name]
        {
            get { return records[rowCounter - 1][name]; }
        }

        public void Dispose()
        {

        }

        public void Close()
        {

        }

        public int GetOrdinal(string name)
        {
            // return an index number of a key in a dictionary
            return records[rowCounter - 1].Keys.ToList().IndexOf(name);
        }

        public object GetValue(int i)
        {
            // get a key based on index number
            string key = records[rowCounter - 1].Keys.ElementAt(i);

            // return a value based on a key
            return records[rowCounter - 1][key];
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public int Depth => throw new NotImplementedException();
        public bool IsClosed => throw new NotImplementedException();
        public int RecordsAffected => throw new NotImplementedException();
        public int FieldCount => throw new NotImplementedException();
        public object this[int i] => throw new NotImplementedException();
        public DataTable GetSchemaTable() => throw new NotImplementedException();
        public bool NextResult() => throw new NotImplementedException();
        public bool GetBoolean(int i) => throw new NotImplementedException();
        public byte GetByte(int i) => throw new NotImplementedException();
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public char GetChar(int i) => throw new NotImplementedException();
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public IDataReader GetData(int i) => throw new NotImplementedException();
        public string GetDataTypeName(int i) => throw new NotImplementedException();
        public DateTime GetDateTime(int i) => throw new NotImplementedException();
        public decimal GetDecimal(int i) => throw new NotImplementedException();
        public double GetDouble(int i) => throw new NotImplementedException();
        public Type GetFieldType(int i) => throw new NotImplementedException();
        public float GetFloat(int i) => throw new NotImplementedException();
        public Guid GetGuid(int i) => throw new NotImplementedException();
        public short GetInt16(int i) => throw new NotImplementedException();
        public int GetInt32(int i) => throw new NotImplementedException();
        public long GetInt64(int i) => throw new NotImplementedException();
        public string GetName(int i) => throw new NotImplementedException();
        public string GetString(int i) => throw new NotImplementedException();
        public int GetValues(object[] values) => throw new NotImplementedException();
    }
}
