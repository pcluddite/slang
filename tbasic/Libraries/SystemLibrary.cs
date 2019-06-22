/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
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

        private object GetMonth(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Month;
        }

        private object GetDay(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Day;
        }

        private object GetDayOfWeek(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return (int)DateTime.Now.DayOfWeek;
        }

        private object GetYear(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Year;
        }

        private object GetHour(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Hour;
        }

        private object GetMinute(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Minute;
        }

        private object GetSecond(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Second;
        }

        private object GetMillisecond(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(1);
            return DateTime.Now.Millisecond;
        }
    }
}