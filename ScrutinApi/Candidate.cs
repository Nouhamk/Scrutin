namespace ScrutinApi
{
    public class Candidate
    {
        public string Name { get; set; }
        public int Votes { get; set; }
        
        public Candidate(string name)
        {
            Name = name;
            Votes = 0;
        }
    }
}