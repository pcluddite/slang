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

        private object GetMonth(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Month;
        }

        private object GetDay(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Day;
        }

        private object GetDayOfWeek(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return (int)DateTime.Now.DayOfWeek;
        }

        private object GetYear(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Year;
        }

        private object GetHour(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Hour;
        }

        private object GetMinute(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Minute;
        }

        private object GetSecond(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Second;
        }

        private object GetMillisecond(StackData stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Millisecond;
        }
    }
}