//http://www.hanselman.com/blog/SplittingDateTimeUnitTestingASPNETMVCCustomModelBinders.aspx

using System;
using System.Web.Mvc;

namespace BigBallz.Core.Web.MVC
{
    public class DateAndTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            //Maybe we're lucky and they just want a DateTime the regular way.
            DateTime? dateTimeAttempt = GetA<DateTime>(bindingContext, "DateTime");
            if (dateTimeAttempt != null)
            {
                return dateTimeAttempt.Value;
            }

            //If they haven't set Month,Day,Year OR Date, set "date" and get ready for an attempt
            if (this.MonthDayYearSet == false && this.DateSet == false)
            {
                this.Date = "Date";
            }

            //If they haven't set Hour, Minute, Second OR Time, set "time" and get ready for an attempt
            if (this.HourMinuteSecondSet == false && this.TimeSet == false)
            {
                this.Time = "Time";
            }

            //Did they want the Date *and* Time?
            DateTime? dateAttempt = GetA<DateTime>(bindingContext, this.Date);
            DateTime? timeAttempt = GetA<DateTime>(bindingContext, this.Time);

            //Maybe they wanted the Time via parts
            if (this.HourMinuteSecondSet)
            {
                var hour = GetA<int>(bindingContext, this.Hour);
                var minute = GetA<int>(bindingContext, this.Minute);
                var second = GetA<int>(bindingContext, this.Second);
                if (hour.HasValue && minute.HasValue && second.HasValue)
                {
                    timeAttempt = new DateTime(
                        DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                        hour.Value,
                        minute.Value,
                        second.Value);
                }
            }

            //Maybe they wanted the Date via parts
            if (this.MonthDayYearSet)
            {
                var year = GetA<int>(bindingContext, this.Year);
                var month = GetA<int>(bindingContext, this.Month);
                var day = GetA<int>(bindingContext, this.Day);
                if (year.HasValue && month.HasValue && day.HasValue)
                {
                    dateAttempt = new DateTime(
                        year.Value,
                        month.Value,
                        day.Value,
                        DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                }
            }
            else if (this.MonthYearSet)
            {
                var year = GetA<int>(bindingContext, this.Year);
                var month = GetA<int>(bindingContext, this.Month);
                if (month.HasValue && year.HasValue)
                {
                    dateAttempt = new DateTime(
                        year.Value,
                        month.Value,
                        1,
                        DateTime.MinValue.Hour,
                        DateTime.MinValue.Minute,
                        DateTime.MinValue.Second);
                }
            }

            //If we got both parts, assemble them!
            if (dateAttempt != null && timeAttempt != null)
            {
                return new DateTime(dateAttempt.Value.Year,
                                    dateAttempt.Value.Month,
                                    dateAttempt.Value.Day,
                                    timeAttempt.Value.Hour,
                                    timeAttempt.Value.Minute,
                                    timeAttempt.Value.Second);
            }
            //Only got one half? Return as much as we have!
            return dateAttempt ?? timeAttempt;
        }

        private Nullable<T> GetA<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (String.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult;
            //Try it with the prefix...
            valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            //Didn't work? See if model is DateTime and try it without prefix...
            if (valueResult == null && (bindingContext.ModelType == typeof(T)) || bindingContext.ModelType == typeof(Nullable<T>))
            {
                valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            }
            if (valueResult == null)
            {
                return null;
            }
            return (Nullable<T>)valueResult.ConvertTo(typeof(T));
        }
        public string Date { get; set; }
        public string Time { get; set; }

        public string Month { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }

        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public bool DateSet { get { return !String.IsNullOrEmpty(Date); } }
        public bool MonthDayYearSet { get { return !String.IsNullOrEmpty(Month) && !String.IsNullOrEmpty(Day) && !String.IsNullOrEmpty(Year); } }
        public bool MonthYearSet { get { return !String.IsNullOrEmpty(Month) && !String.IsNullOrEmpty(Year); } }

        public bool TimeSet { get { return !String.IsNullOrEmpty(Time); } }
        public bool HourMinuteSecondSet { get { return !String.IsNullOrEmpty(Hour) && !String.IsNullOrEmpty(Minute) && !String.IsNullOrEmpty(Second); } }

    }

    public class DateAndTimeAttribute : CustomModelBinderAttribute
    {
        private IModelBinder _binder;

        // The user cares about a full date structure and full
        // time structure, or one or the other.
        public DateAndTimeAttribute(string date, string time)
        {
            _binder = new DateAndTimeModelBinder
                          {
                              Date = date,
                              Time = time
                          };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string year, string month, string day,
                                    string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
                          {
                              Day = day,
                              Month = month,
                              Year = year,
                              Hour = hour,
                              Minute = minute,
                              Second = second
                          };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string date, string time,
                                    string year, string month, string day,
                                    string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
                          {
                              Day = day,
                              Month = month,
                              Year = year,
                              Hour = hour,
                              Minute = minute,
                              Second = second,
                              Date = date,
                              Time = time
                          };
        }

        // The user cares about a full date structure and full
        // time structure, or one or the other.
        public DateAndTimeAttribute(string year, string month, string day)
        {
            _binder = new DateAndTimeModelBinder
                          {
                              Year = year,
                              Month = month,
                              Day = day
                          };
        }

        public override IModelBinder GetBinder() { return _binder; }
    }
}