
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splatoon
{
    public class CsvFile
    {
        const string LineSplit = "\r\n";
        const char ColumnSplit = ',';

        private string mFileName;
        private string[] mTitle;
        private ArrayList mBody;
        private int mCurLine;

        public int CurrentLine { set { mCurLine = value; } get { return mCurLine; } }

        public CsvFile(string filename, string content)
        {
            mFileName = filename;
            string[] str = content.Split(new string[] {LineSplit}, StringSplitOptions.RemoveEmptyEntries);
            mTitle = str[0].Split(new char[] { ColumnSplit });

            mBody = new ArrayList();
            for (int i = 1; i < str.Length; i++)
            {
                string[] line = str[i].Split(new char[] { ColumnSplit });
                mBody.Add(line);
            }

            mBody.TrimToSize();
            mCurLine = -1;
        }

        public bool Next()
        {
            mCurLine++;
            if (mCurLine >= mBody.Count)
                return false;

            return true;
        }

        public T Get<T>(string colname)
        {
            int colindex = Array.IndexOf(mTitle, colname);
            if (colindex < 0)
            {
                return default(T);
            }

            return Get<T>(colindex);
        }

        public T Get<T>(int colindex)
        {
            string value = GetValueString(colindex);
            Type t = typeof(T);
            try
            {
                return (T)Convert.ChangeType(value, t);
            }
            catch (Exception)
            {
                LogSystem.Write(LogLevel.Error, "CsvFile:" + mFileName + " Wrong ColIndex: " + colindex + " At Line: " + mCurLine);
            }

            return default(T);
        }

        private string GetValueString(int colindex)
        {
            string[] line = (string[])mBody[mCurLine];
            if (colindex < 0 || colindex >= line.Length)
            {
                LogSystem.Write(LogLevel.Error, "CsvFile:" + mFileName + " Wrong ColIndex: " + colindex + " At Line: " + mCurLine);
                return null;
            }

            return line[colindex];
        }

        public string GetString(string colname)
        {
            int colindex = Array.IndexOf(mTitle, colname);
            return GetValueString(colindex);
        }

        public int GetInt32(string colname)
        {
            return GetInt32(Array.IndexOf(mTitle, colname));
        }

        public int GetInt32(int colindex)
        {
            try
            {
                return Convert.ToInt32(GetValueString(colindex));
            }
            catch (Exception)
            {
                LogSystem.Write(LogLevel.Error, "CsvFile:" + mFileName + " Wrong ColIndex: " + colindex + " At Line: " + mCurLine);
                return -1;
            }
        }

        public uint GetUInt32(string colname)
        {
            return GetUInt32(Array.IndexOf(mTitle, colname));
        }

        public uint GetUInt32(int colindex)
        {
            try
            {
                return Convert.ToUInt32(GetValueString(colindex));
            }
            catch (Exception)
            {
                LogSystem.Write(LogLevel.Error, "CsvFile:" + mFileName + " Wrong ColIndex: " + colindex + " At Line: " + mCurLine);
                return 0;
            }
        }

        public double GetDouble(string colname)
        {
            return GetDouble(Array.IndexOf(mTitle, colname));
        }

        public double GetDouble(int colindex)
        {
            try
            {
                return Convert.ToDouble(GetValueString(colindex));
            }
            catch (Exception)
            {
                LogSystem.Write(LogLevel.Error, "CsvFile:" + mFileName + " Wrong ColIndex: " + colindex + " At Line: " + mCurLine);
                return 0;
            }
        }

        public float GetFloat(string colname)
        {
            return (float)GetDouble(colname);
        }

        public float GetFloat(int colindex)
        {
            return (float)GetDouble(colindex);
        }
    }
}
