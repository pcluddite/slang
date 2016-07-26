// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    internal class SystemLibrary : Library
    {
        public SystemLibrary()
        {
            Add("GetMonth", GetMonth);
            Add("GetDay", GetDay);
            Add("GetDayOfWeek", GetDayOfWeek);
            Add("GetYear", GetYear);
            Add("GetHour", GetHour);
            Add("GetMinute", GetMinute);
            Add("GetSecond", GetSecond);
            Add("GetMillisecond", GetMillisecond);
        }

        private void GetMonth(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Month;
        }

        private void GetDay(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Day;
        }

        private void GetDayOfWeek(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = (int)DateTime.Now.DayOfWeek;
        }

        private void GetYear(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Year;
        }

        private void GetHour(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Hour;
        }

        private void GetMinute(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Minute;
        }

        private void GetSecond(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Second;
        }

        private void GetMillisecond(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(1);
            _sframe.Data = DateTime.Now.Millisecond;
        }
    }
}