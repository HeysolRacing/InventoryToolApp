using System;

namespace InventoryTool.Models
{
    public class CalculateDate
    {
        public int Years;
        public int Months;
        public int Days;

        public CalculateDate(DateTime ValDay)
        {
            Count(ValDay);
        }

        public CalculateDate(DateTime ValDay, DateTime CurrentDay)
        {
            Count(ValDay, CurrentDay);
        }

        public CalculateDate Count(DateTime ValDay)
        {
            return Count(ValDay, DateTime.Today);
        }

        public CalculateDate Count(DateTime ValDay, DateTime CurrentDay)
        {
            if ((CurrentDay.Year - ValDay.Year) > 0 ||
                (((CurrentDay.Year - ValDay.Year) == 0) && ((ValDay.Month < CurrentDay.Month) ||
                  ((ValDay.Month == CurrentDay.Month) && (ValDay.Day <= CurrentDay.Day)))))
            {
                int DaysInValDayMonth = DateTime.DaysInMonth(ValDay.Year, ValDay.Month);
                int DaysRemain = CurrentDay.Day + (DaysInValDayMonth - ValDay.Day);

                if (CurrentDay.Month > ValDay.Month)
                {
                    this.Years = CurrentDay.Year - ValDay.Year;
                    this.Months = CurrentDay.Month - (ValDay.Month + 1) + Math.Abs(DaysRemain / DaysInValDayMonth);
                    this.Days = (DaysRemain % DaysInValDayMonth + DaysInValDayMonth) % DaysInValDayMonth;
                }
                else if (CurrentDay.Month == ValDay.Month)
                {
                    if (CurrentDay.Day >= ValDay.Day)
                    {
                        Years = CurrentDay.Year - ValDay.Year;
                        Months = 0;
                        Days = CurrentDay.Day - ValDay.Day;
                    }
                    else
                    {
                        Years = (CurrentDay.Year - 1) - ValDay.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(ValDay.Year, ValDay.Month) - (ValDay.Day - CurrentDay.Day);
                    }
                }
                else
                {
                    Years = (CurrentDay.Year - 1) - ValDay.Year;
                    Months = CurrentDay.Month + (11 - ValDay.Month) + Math.Abs(DaysRemain / DaysInValDayMonth);
                    Days = (DaysRemain % DaysInValDayMonth + DaysInValDayMonth) % DaysInValDayMonth;
                }
            }
            else
            {
                Years = 0;
                Months = 0;
                Days = 0;
            }
            return this;
        }
    }
}