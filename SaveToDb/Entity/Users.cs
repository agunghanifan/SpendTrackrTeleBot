using System.ComponentModel.DataAnnotations;

namespace SpendTrackrTeleBot.SaveToDb.Entity
{
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public required string CHAT_ID { get; set; }
        public required string SHEET_ID { get; set; }
        public required string EMAIL { get; set; }
        public string STATUS_DATA {get; set;} = "Y";
        public DateTime CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
