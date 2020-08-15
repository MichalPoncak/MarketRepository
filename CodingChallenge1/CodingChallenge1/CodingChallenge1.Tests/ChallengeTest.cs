using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CodingChallenge1.Tests
{
    public class ChallengeTest
    {
        public class GetMiddleTest
        {
            [Fact]
            public void TestGetMiddleString()
            {
                Assert.Equal("es", Challenge.TestGetMiddleString("test"));
                Assert.Equal("t", Challenge.TestGetMiddleString("testing"));
                Assert.Equal("dd", Challenge.TestGetMiddleString("middle"));
                Assert.Equal("A", Challenge.TestGetMiddleString("A"));
            }

            [Fact]
            public void TestSolution()
            {
                Assert.Equal(23, Challenge.Solution(10));
            }

            [Fact]
            public void TestSortTextNumbersByWeight()
            {
                Assert.Equal("2000 103 123 4444 99", Challenge.SortTextNumbersByWeight("103 123 4444 99 2000"));
                Assert.Equal("11 11 2000 10003 22 123 1234000 44444444 9999", Challenge.SortTextNumbersByWeight("2000 10003 1234000 44444444 9999 11 11 22 123"));
            }

            [Fact]
            public void BasicTests()
            {
                string preSorting = "myjinxin2015;raulbc777;smile67;Dentzil;SteffenVogel_79\n17945;10091;10088;3907;10132\n2;12;13;48;11";
                string postSorting = "Dentzil;myjinxin2015;raulbc777;smile67;SteffenVogel_79\n3907;17945;10091;10088;10132\n48;2;12;13;11";
                Assert.Equal(postSorting, Challenge.SortColumnsOfCSVFormattedData(preSorting));
            }

            [Fact]
            public void TestExecute()
            {
                Assert.Equal(0, Challenge.ExpressionEvaluator(""));
                Assert.Equal(3, Challenge.ExpressionEvaluator("1 2 3"));
                Assert.Equal(3.5, Challenge.ExpressionEvaluator("1 2 3.5"));
                Assert.Equal(4, Challenge.ExpressionEvaluator("1 3 +"));
                Assert.Equal(3, Challenge.ExpressionEvaluator("1 3 *"));
                Assert.Equal(-2, Challenge.ExpressionEvaluator("1 3 -"));
                Assert.Equal(2, Challenge.ExpressionEvaluator("4 2 /"));
            }
        }
    }
}
