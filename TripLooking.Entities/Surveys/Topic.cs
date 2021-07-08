namespace StudentSatisfaction.Entities.Survey
{
    public sealed class Topic:Entity
    {
        public Topic(string title,string details):base()
        {
            Title = title;
            Details = details;
        }

        public string Title { get; private set; }
        public string Details { get; private set; }

    }
}