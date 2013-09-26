using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpeedPopQuiz_W8.Common
{
    class Problem
    {
        public String prob { get; set; }

        public String ans { get; set; }

        public String input { get; set; }

        public bool correct { get; set; }

        public Problem(int num1, int num2)
        {
            prob = num1 + " + " + num2;

            ans = (num1 + num2).ToString();
        }
    }
}
