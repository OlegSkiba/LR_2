namespace WebApplication1
{
    public class TimeOfDayService
    {
        public string GetTimeOfDay()
        {
            var currentHour = DateTime.Now.Hour;

            if (currentHour >= 6 && currentHour < 12)
            {
                return "зараз ранок";
            }
            else if (currentHour >= 12 && currentHour < 18)
            {
                return "зараз день";
            }
            else if (currentHour >= 18 && currentHour < 24)
            {
                return "зараз вечір";
            }
            else
            {
                return "зараз ніч";
            }
        }
    }

}
