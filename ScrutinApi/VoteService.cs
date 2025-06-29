namespace ScrutinApi
{
    public class VoteService
    {
        public VoteResult CalculateResult(List<Candidate> candidates, int blankVotes = 0)
        {
            if (candidates == null || candidates.Count == 0)
                throw new ArgumentException("La liste des candidats ne peut pas être vide");
            
            var totalVotes = candidates.Sum(c => c.Votes);
            var totalSuffrages = totalVotes + blankVotes;
            
            if (totalSuffrages == 0)
                throw new InvalidOperationException("Aucun vote n'a été enregistré");

            // Si on a seulement 2 candidats ET qu'ils ont des votes, c'est probablement un second tour
            if (candidates.Count == 2 && candidates.All(c => c.Votes > 0))
            {
                return CalculateSecondRoundResult(candidates, blankVotes);
            }
            
            // Premier tour, Voir si un candidat a plus de 50%
            var winner = FindMajorityWinner(candidates, totalSuffrages);
            if (winner != null)
            {
                // Vainqueur au premier tour
                return new VoteResult(winner, candidates, 1, true, null, blankVotes);
            }
            
            // Pas de vainqueur, préparer le second tour
            var secondRoundCandidates = GetSecondRoundCandidates(candidates);
                
            return new VoteResult(null, candidates, 1, false, secondRoundCandidates, blankVotes);
        }
        
        private List<Candidate> GetSecondRoundCandidates(List<Candidate> candidates)
        {
            // Grouper les candidats par nombre de votes
            var groupedByVotes = candidates
                .GroupBy(c => c.Votes)
                .OrderByDescending(g => g.Key)
                .ToList();
            
            var secondRoundCandidates = new List<Candidate>();
            
            secondRoundCandidates.AddRange(groupedByVotes.First());
            
            // Si on a déjà plus de 2 candidats, on s'arrête
            if (secondRoundCandidates.Count >= 2)
            {
                return secondRoundCandidates;
            }
            
            // Sinon, prendre tous les candidats à égalité pour la 2ème place
            if (groupedByVotes.Count > 1)
            {
                secondRoundCandidates.AddRange(groupedByVotes[1]);
            }
            
            return secondRoundCandidates;
        }
        
        private VoteResult CalculateSecondRoundResult(List<Candidate> candidates, int blankVotes)
        {
            var sortedCandidates = candidates.OrderByDescending(c => c.Votes).ToList();
            var first = sortedCandidates[0];
            
            var winnersCount = candidates.Count(c => c.Votes == first.Votes);
            
            if (winnersCount > 1)
            {
                // Égalité au second tour
                return new VoteResult(null, candidates, 2, true, null, blankVotes);
            }
            
            // Vainqueur au second tour
            return new VoteResult(first, candidates, 2, true, null, blankVotes);
        }
        
        private Candidate? FindMajorityWinner(List<Candidate> candidates, int totalSuffrages)
        {
            return candidates.FirstOrDefault(c => 
                Math.Round((double)c.Votes / totalSuffrages * 100, 0) > 50);
        }
    }
}