using Isu.Tools;

namespace Isu.Entities
{
    public class CourseNumber
    {
        private int _number;
        public CourseNumber(int number)
        {
            Number = number;
        }

        public int Number
        {
            get => _number;
            private set
            {
                if (Number > 4 || Number < 1)
                    throw new IsuException("Invalid Course Number");
                _number = value;
            }
        }
    }
}