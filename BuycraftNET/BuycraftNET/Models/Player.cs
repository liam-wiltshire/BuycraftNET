namespace BuycraftNET.Models
{
    public class BCPlayer
    {
        private int id;
        private string ign;
        private string uuid;

        public BCPlayer(int id, string ign, string uuid = null)
        {
            this.id = id;
            this.ign = ign;
            if (uuid != null)
            {
                this.uuid = uuid;
            }

        }

        public string getIgn()
        {
            return ign;
        }

        public int getId()
        {
            return id;
        }           
    }
}