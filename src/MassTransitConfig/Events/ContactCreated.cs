namespace MasstransitConfig.Events
{
    public class ContactCreated
    {
        public Contact Contact { get; set; }
    }

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Contact(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
