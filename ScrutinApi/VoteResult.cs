namespace ScrutinApi
{
    public class VoteResult
    {
        public Candidate? Winner { get; }
        public List<Candidate> AllCandidates { get; }
        public List<Candidate> SecondRoundCandidates { get; }
        public int NumberOfRounds { get; }
        public bool IsCompleted { get; }
        public int BlankVotes { get; }
        
        public VoteResult(Candidate? winner, List<Candidate> allCandidates, int numberOfRounds, bool isCompleted, List<Candidate>? secondRoundCandidates = null, int blankVotes = 0)
        {
            Winner = winner;
            AllCandidates = allCandidates ?? new List<Candidate>();
            SecondRoundCandidates = secondRoundCandidates ?? new List<Candidate>();
            NumberOfRounds = numberOfRounds;
            IsCompleted = isCompleted;
            BlankVotes = blankVotes;
        }
        
        public int TotalVotes => AllCandidates.Sum(c => c.Votes);
        public int TotalSuffragesExprimees => TotalVotes + BlankVotes;
        
        public double GetCandidatePercentage(string candidateName)
        {
            var candidate = AllCandidates.FirstOrDefault(c => c.Name == candidateName);
            if (candidate == null || TotalSuffragesExprimees == 0) return 0;
            return Math.Round((double)candidate.Votes / TotalSuffragesExprimees * 100, 0);
        }
    }
}