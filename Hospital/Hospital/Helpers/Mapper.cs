using Hospital.Models;

namespace Hospital.Helpers
{
    public class Mapper
    {
        public static string MapStatus(Status status)
        {
            if(status == Status.Planned)
            {
                return "Zaplanowane";
            }

            return "Zakończone";
        }
    }
}