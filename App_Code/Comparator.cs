using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public class Comparator
{
    public HDSchedule s1;
    public HDSchedule s2;

    public Comparator(HDSchedule s1, HDSchedule s2)
    {
        this.s1 = s1;
        this.s2 = s2;
    }

    public string[] getCommonClasses()
    {
        string commonClasses = "";
        foreach (KeyValuePair<string, string> pair in s1.teachers)
            if (s2.teachers.Contains(pair) && isSamePeriod(pair.Key))
                commonClasses += pair.Key + "#";
        if (commonClasses.Length > 0)
        {
            commonClasses = commonClasses.Substring(0, commonClasses.Length - 1);
            return commonClasses.Split('#');
        }
        else
            return new string[] { "None" };
    }

    private Boolean isSamePeriod(string className)
    {
        string s1Period = "";
        for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++)
            for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
                if (s1.classes[x, y].Equals(className))
                    s1Period += HDSchedule.getPeriodNameFromNumber(y);

        string s2Period = "";
        for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++)
            for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
                if (s2.classes[x, y].Equals(className))
                    s2Period += HDSchedule.getPeriodNameFromNumber(y);

        return s1Period.Equals(s2Period);
    }

    public string[] getCommonFrees()
    {
        string commonFrees = "";
        char day = 'A';
        for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++, day++)
        {
            if (s1.classes[x, HDSchedule.A_LUNCH_PERIOD].Equals(s2.classes[x, HDSchedule.A_LUNCH_PERIOD]) &&
            s2.classes[x, HDSchedule.A_LUNCH_PERIOD].Equals("LUNCH") && (s1.classes[x, HDSchedule.C_LUNCH_PERIOD].Equals(s2.classes[x, HDSchedule.C_LUNCH_PERIOD]) &&
            s2.classes[x, HDSchedule.C_LUNCH_PERIOD].Equals("LUNCH")))
                //commonFrees += getNth(HDSchedule.getPeriodNameFromNumber(HDSchedule.A_LUNCH_PERIOD)) + " or " +
                //getNth(HDSchedule.getPeriodNameFromNumber(HDSchedule.C_LUNCH_PERIOD)) + " on " + day + " Day#";
                commonFrees += "Double lunch on " + day + " Day#";
            for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
            {
                if (s1.classes[x, y].Equals(s2.classes[x, y]) &&
                s2.classes[x, y].Equals("FREE"))
                    commonFrees += getNth(HDSchedule.getPeriodNameFromNumber(y)) + " on " + day + " Day#";
            }
        }

        if (commonFrees.Length > 0)
        {
            commonFrees = commonFrees.Substring(0, commonFrees.Length - 1);
            return commonFrees.Split('#');
        }
        else
            return new string[] { "None" };
    }

    public string[] getCommonLunches()
    {
        string commonLunches = "";
        char day = 'A';
        for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++, day++)
        {
            if ((s1.classes[x, HDSchedule.A_LUNCH_PERIOD].Equals(s2.classes[x, HDSchedule.A_LUNCH_PERIOD]) &&
            s2.classes[x, HDSchedule.A_LUNCH_PERIOD].Equals("LUNCH")) ||
            (s1.classes[x, HDSchedule.C_LUNCH_PERIOD].Equals(s2.classes[x, HDSchedule.C_LUNCH_PERIOD]) &&
            s2.classes[x, HDSchedule.C_LUNCH_PERIOD].Equals("LUNCH")))
                commonLunches += day + "#";
        }

        if (commonLunches.Length > 0)
        {
            commonLunches = commonLunches.Substring(0, commonLunches.Length - 1);
            return commonLunches.Split('#');
        }
        else
            return new string[] { "None" };
    }

    public Boolean[,] getCommonPeriods()
    {
        Boolean[,] commonPeriods = new Boolean[HDSchedule.DEFAULT_WEEK_LENGTH, HDSchedule.DEFAULT_DAY_LENGTH];
        for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
            for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++)
                if (s1.classes[x, y].Equals(s2.classes[x, y]) && 
                    //!s1.classes[x, y].Equals("ADVISORY") && 
                    ((s1.teachers.Keys.Contains(s1.classes[x, y]) && 
                    s2.teachers.Keys.Contains(s2.classes[x, y])) || 
                    s1.classes[x, y].Equals("FREE") || 
                    s1.classes[x, y].Equals("LUNCH") ||
                    s1.classes[x, y].Equals("ADVISORY"))) 
 	                    commonPeriods[x, y] = true;
                else
                    commonPeriods[x, y] = false;
        return commonPeriods;
    }

    private string getNth(string num)
    {
        if (num.Equals("1"))
            return num + "st";
        else if (num.Equals("2"))
            return num + "nd";
        else if (num.Equals("3"))
            return num + "rd";
        else if (num.Equals("4") || num.Equals("5") || num.Equals("6") || num.Equals("7"))
            return num + "th";
        return num;
    }
}