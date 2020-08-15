using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CodingChallenge1
{
    public class Challenge
    {
        public static string TestGetMiddleString(string s)
        {
            string result = string.Empty;

            // if even
            if (s.Length % 2 == 0)
            {
                int firstMiddleCharIndex = (s.Length / 2) - 1;

                result = s.Substring(firstMiddleCharIndex, 2);
            }
            else // if odd
            {
                int middleCharIndex = s.Length / 2;

                result = s[middleCharIndex].ToString();
            }

            return result;
        }

        public static int Solution(int value)
        {
            int result = 0;

            for (int i = 0; i < value; i++)
            {
                // if number divisible by 3
                if (i % 3 == 0)
                {
                    result += i;
                }
                else if (i % 5 == 0) // if number divisible by 5
                {
                    result += i;
                }
            }

            return result;
        }

        public static string SortTextNumbersByWeight(string strng)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(strng) && !string.IsNullOrWhiteSpace(strng))
            {
                string[] numbers = strng.Split(' ');

                var dtNumbersWithWeight = new DataTable();
                dtNumbersWithWeight.Columns.Add("Number");
                dtNumbersWithWeight.Columns.Add("Weight", Type.GetType("System.Int32"));

                // for all numbers
                for (int i = 0; i < numbers.Length; i++)
                {
                    string textNumber = numbers[i];
                    int weight = 0;

                    // for all digits in a number
                    for (int s = 0; s < textNumber.Length; s++)
                    {
                        weight += Convert.ToInt32(textNumber[s].ToString());
                    }

                    DataRow dr = dtNumbersWithWeight.NewRow();
                    dr["Number"] = textNumber;
                    dr["Weight"] = weight;
                    dtNumbersWithWeight.Rows.Add(dr);
                }

                var dtNumbersWithWeightSorted = dtNumbersWithWeight.AsEnumerable()
                   .OrderBy(r => r.Field<int>("Weight"))
                   .ThenBy(r => r.Field<string>("Number"))
                   .CopyToDataTable();

                for (int i = 0; i < dtNumbersWithWeightSorted.Rows.Count; i++)
                {
                    result.Append(dtNumbersWithWeightSorted.Rows[i]["Number"].ToString());
                    result.Append((i != dtNumbersWithWeightSorted.Rows.Count - 1) ? " " : string.Empty);
                }
            }

            return result.ToString();
        }

        public static string SortColumnsOfCSVFormattedData(string csvFileContent)
        {
            var result = new StringBuilder();

            string genericColName = "Col";
            string[] lines = csvFileContent.Split(new[] { '\n' });
            var dtCsvData = new DataTable();
            var lineInputs = new List<string[]>();

            // foreach line/row
            for (int i = 0; i < lines.Length; i++)
            {
                // add a new column to datatable
                dtCsvData.Columns.Add(genericColName + i);

                string[] rowValues = lines[i].Split(';');

                // foreach row value
                for (int r = 0; r < rowValues.Length; r++)
                {
                    // if first line
                    if (i == 0)
                    {
                        // create new row
                        DataRow dr = dtCsvData.NewRow();
                        dr[i] = rowValues[r];
                        dtCsvData.Rows.Add(dr);
                    }
                    else
                    {
                        // resuse the existing rows in the datatable
                        dtCsvData.Rows[r][i] = rowValues[r];
                    }
                }
            }

            var dtCsvDataSorted = dtCsvData.AsEnumerable()
                   .OrderBy(r => r.Field<string>(genericColName + "0"))
                   .CopyToDataTable();

            // for all column names in datatable
            for (int c = 0; c < dtCsvDataSorted.Columns.Count; c++)
            {
                lineInputs.Add(dtCsvDataSorted.AsEnumerable().Select(s => s.Field<string>(genericColName + c)).ToArray<string>());
            }

            // for all lines
            for (int l = 0; l < lineInputs.Count; l++)
            {
                var lineOutput = new StringBuilder();

                // for line values
                for (int i = 0; i < lineInputs[l].Length; i++)
                {
                    lineOutput.Append(lineInputs[l][i].ToString());
                    lineOutput.Append((i != lineInputs[l].Length - 1) ? ";" : string.Empty);
                }

                // if not the last line
                if (l != lineInputs.Count - 1)
                {
                    result.AppendLine(lineOutput.ToString());
                }
                else // else if last line
                {
                    result.Append(lineOutput.ToString());
                }
            }

            return result.ToString().Replace("\r\n", "\n");
        }

        public static double ExpressionEvaluator(String expr)
        {
            double result = 0.0;

            if (!string.IsNullOrEmpty(expr))
            {
                // Valid operators: +, -, *, /
                string[] numbersAndOperators = expr.Split(' ');
                List<string> foundOperators = new List<string>();
                List<string> foundNumbers = new List<string>();
                bool operatorFound = false;

                // for all expression characters
                for (int i = 0; i < numbersAndOperators.Length; i++)
                {
                    switch (numbersAndOperators[i])
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            operatorFound = true;
                            foundOperators.Add(numbersAndOperators[i]);
                            break;
                        default: // if not an operator
                            foundNumbers.Add(numbersAndOperators[i]);
                            break;
                    }
                }

                if (operatorFound == false)
                {
                    result = Convert.ToDouble(numbersAndOperators[numbersAndOperators.Length - 1]);
                }
                else // if operators found
                {
                    var sbEquation = new StringBuilder();

                    // for all numbers
                    for (int i = 0; i < foundNumbers.Count; i++)
                    {
                        sbEquation.Append(foundNumbers[i]);

                        if (i < foundOperators.Count())
                        {
                            sbEquation.Append(foundOperators[i]);
                        }
                    }

                    var dt = new DataTable();
                    result = Convert.ToDouble(dt.Compute(sbEquation.ToString(), null));
                }
            }

            return result;
        }
    }
}
