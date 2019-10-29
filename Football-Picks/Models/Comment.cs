using System;

namespace Football_Picks.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string PlayerName { get; set; }
        public string Feedback_Message { get; set; }
        public DateTime Created { get; set; }

    }
}
