using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

public class HDSchedule
{
    public static int DEFAULT_WEEK_LENGTH = 6;
    public static int DEFAULT_DAY_LENGTH = 8;
    public static int A_LUNCH_PERIOD = 4;
    public static int C_LUNCH_PERIOD = 5;
    public Dictionary<string, string> teachers = new Dictionary<string, string>();
    public string[,] classes = new string[DEFAULT_WEEK_LENGTH, DEFAULT_DAY_LENGTH];

    public HDSchedule()
    {
        for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
            for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
                classes[x, y] = "";
    }

    public void clearSchedule()
    {
        teachers.Clear();
        classes = new string[DEFAULT_WEEK_LENGTH, DEFAULT_DAY_LENGTH];
        for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
            for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
                classes[x, y] = "";
    }

    public string[] getAllClassNames()
    {
        HashSet<string> uniqueClasses = new HashSet<string>();
        for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
            for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
                if (!(classes[x, y] == null || classes[x, y].Equals("") || classes[x, y].Trim().Equals("")))
                    uniqueClasses.Add(classes[x, y]);
        string preReturnString = "";
        foreach (string className in uniqueClasses)
            if (!className.Equals("FREE") && !className.Equals("ADVISORY") && !className.Equals("LUNCH"))
                preReturnString += className + "#";
        preReturnString = preReturnString.Substring(0, preReturnString.Length - 1);
        return preReturnString.Split('#');
    }

    public Boolean hasClasses()
    {
        return !classes[0, 0].Equals("");
    }

    public Boolean hasTeachers()
    {
        return teachers.Count() > 0;
    }

    public void setFromDatabaseString(string databaseStr)
    {
        string classesStr = "";
        string teachersStr = "";
        if (databaseStr.Length > 1)
        {
            if (databaseStr.IndexOf('&') == -1)
                classesStr = databaseStr;
            else
            {
                classesStr = databaseStr.Substring(0, databaseStr.IndexOf('&'));
                teachersStr = databaseStr.Substring(databaseStr.IndexOf('&') + 1, databaseStr.Length - databaseStr.IndexOf('&') - 1);
            }
        }

        if (classesStr.Length > 0)
        {
            string[] dayStrings = classesStr.Split('@');
            for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
            {
                string[] periodStrings = dayStrings[x].Split('#');
                for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
                    classes[x, y] = periodStrings[y];
            }
        }

        if (teachersStr.Length > 0)
        {
            teachers.Clear();
            string[] teacherPairStrings = teachersStr.Split('@');
            for (int i = 0; i < teacherPairStrings.Length; i++)
            {
                string classString = teacherPairStrings[i].Substring(0, teacherPairStrings[i].IndexOf('#'));
                string teacherString = teacherPairStrings[i].Substring(teacherPairStrings[i].IndexOf('#') + 1, teacherPairStrings[i].Length - teacherPairStrings[i].IndexOf('#') - 1);
                teacherString = teacherString.Replace('%', ',');
                teachers.Add(classString, teacherString);
            }
        }
    }

    public string getDatabaseString()
    {
        if (!hasClasses())
        {
            return "";
        }

        string toReturn = "";
        for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
        {
            if (x != 0)
                toReturn += "@";
            for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
            {
                if (y != 0)
                    toReturn += "#";
                toReturn += classes[x, y];
            }
        }
        toReturn += "&";
        foreach (KeyValuePair<string, string> teacher in teachers)
        {
            toReturn += teacher.Key + "#" + teacher.Value;
            toReturn += "@";
        }
        toReturn = toReturn.Substring(0, toReturn.Length - 1);
        toReturn = toReturn.Replace(',', '%');
        return toReturn;
    }

    public string getClassesDatabaseString()
    {
        string toReturn = "";
        for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
        {
            if (x != 0)
                toReturn += "@";
            for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
            {
                if (y != 0)
                    toReturn += "#";
                toReturn += classes[x, y];
            }
        }
        return toReturn;
    }

    public string getTeachersDatabaseString()
    {
        string toReturn = "";
        foreach (KeyValuePair<string, string> teacher in teachers)
        {
            toReturn += teacher.Key + "#" + teacher.Value;
            toReturn += "@";
        }
        toReturn = toReturn.Substring(0, toReturn.Length - 1);
        return toReturn;
    }

    public void setFromSiteString(string siteStr)
    {
        List<String> lines = new List<String>();
        {
            String fullText = siteStr;
            String[] lineSplits = fullText.Split(new String[] { "\n" }, StringSplitOptions.None);
            foreach (String line in lineSplits)
                lines.Add(line);

            if (lines[0].Contains("Block"))
                lines.RemoveAt(0);
            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = Regex.Replace(lines[i], "[0-9] ", "");
                lines[i] = Regex.Replace(lines[i], "5A ", "");
                lines[i] = Regex.Replace(lines[i], "5B ", "");
                lines[i] = Regex.Replace(lines[i], "5C ", "");
                lines[i] = lines[i].Replace("HR HR/ADV", "ADVISORY");
                lines[i] = lines[i].Replace("HR/ADV", "ADVISORY");

                lines[i] = Regex.Replace(lines[i], "US[0-9]{3}[a-zA-Z]", "");
                lines[i] = Regex.Replace(lines[i], "FA[0-9]{3}[a-zA-Z]", "");

                lines[i] = Regex.Replace(lines[i], "US[0-9]{3}", "");
                lines[i] = Regex.Replace(lines[i], "FA[0-9]{3}", "");
                lines[i] = lines[i].Replace("5A", "LUNCH\n");
                lines[i] = lines[i].Replace("5B", "LUNCH\n");
                lines[i] = lines[i].Replace("5C", "LUNCH\n");
                lines[i] = Regex.Replace(lines[i], "\t[0-9]", "\tFREE");
                lines[i] = lines[i].Replace("\n", "");
                lines[i] = lines[i].Trim();

                if (!lines[i].Equals(""))
                {
                    String[] tabSplits = lines[i].Split(new String[] { "\t" }, StringSplitOptions.None);
                    lines.RemoveAt(i);
                    for (int x = tabSplits.Count() - 1; x >= 0; x--)
                        lines.Insert(i, tabSplits[x]);
                    i += tabSplits.Count() - 1;
                }
            }

            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Trim().Equals(""))
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }
        }

        if (lines.Count() == 70)
        {
            lines.RemoveAt(63);
            lines.RemoveAt(56);
            lines.RemoveAt(49);

            lines.RemoveAt(48);
            lines.RemoveAt(47);
            lines.RemoveAt(46);
            lines.RemoveAt(45);
            lines.RemoveAt(44);
            lines.RemoveAt(43);
            lines.RemoveAt(42);
            lines.RemoveAt(35);
            lines.RemoveAt(28);
            lines.RemoveAt(21);
            lines.RemoveAt(14);
            lines.RemoveAt(7);
            lines.RemoveAt(6);
            lines.RemoveAt(5);
            lines.RemoveAt(4);
            lines.RemoveAt(3);
            lines.RemoveAt(2);
            lines.RemoveAt(1);
            lines.RemoveAt(0);

            int lineCount = 0;

            for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
            {
                for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
                {
                    classes[x, y] = lines[lineCount];
                    lineCount++;
                }
            }
        }
        else
        {
            classes = new string[DEFAULT_WEEK_LENGTH, DEFAULT_DAY_LENGTH];

            for (int y = 0; y < DEFAULT_DAY_LENGTH; y++)
                for (int x = 0; x < DEFAULT_WEEK_LENGTH; x++)
                    classes[x, y] = "";
        }
    }

    public static string getPeriodNameFromNumber(int number)
    {
        switch (number)
        {
            case 0:
                return "1";
            case 1:
                return "2";
            case 2:
                return "3";
            case 3:
                return "4";
            case 4:
                return "5A";
            case 5:
                return "5C";
            case 6:
                return "6";
            case 7:
                return "7";
            default:
                return "";
        }
    }

    public static int getPeriodNumberFromName(string name)
    {
        switch (name)
        {
            case "1":
                return 0;
            case "2":
                return 1;
            case "3":
                return 2;
            case "4":
                return 3;
            case "5A":
                return 4;
            case "5C":
                return 5;
            case "6":
                return 6;
            case "7":
                return 7;
            default:
                return -1;
        }
    }
}