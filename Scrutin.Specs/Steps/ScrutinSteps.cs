using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;
using ScrutinApi;

namespace Scrutin.Specs.Steps
{
    [Binding]
    public class ScrutinSteps
    {
        private List<Candidate> _candidats = new();
        private VoteService _voteService = new();
        private VoteResult? _resultat;
        private int _votesBlanchs = 0;
        
        [Given(@"un scrutin avec les candidats suivants")]
        public void GivenUnScrutinAvecLesCandidatsSuivants(Table table)
        {
            _candidats.Clear();
            _votesBlanchs = 0;
            foreach (var row in table.Rows)
            {
                var nom = row["Name"];
                _candidats.Add(new Candidate(nom));
            }
        }
        
        [Given(@"que les votes suivants ont été enregistrés")]
        public void GivenQuelesVotesSuivantsOntEteEnregistres(Table table)
        {
            foreach (var row in table.Rows)
            {
                var nomCandidat = row["Candidate"];
                var votes = int.Parse(row["Votes"]);
                
                var candidat = _candidats.FirstOrDefault(c => c.Name == nomCandidat);
                if (candidat != null)
                {
                    candidat.Votes = votes;
                }
            }
        }

        [Given(@"que (.*) votes blancs ont été enregistrés")]
        public void GivenQueVotesBlancOntEteEnregistres(int nombreVotesBlancs)
        {
            _votesBlanchs = nombreVotesBlancs;
        }

        [Given(@"un scrutin au second tour avec les candidats suivants")]
        public void GivenUnScrutinAuSecondTourAvecLesCandidatsSuivants(Table table)
        {
            _candidats.Clear();
            _votesBlanchs = 0;
            
            foreach (var row in table.Rows)
            {
                var nom = row["Name"];
                var votes = int.Parse(row["Votes"]);
                var candidat = new Candidate(nom);
                candidat.Votes = votes;
                _candidats.Add(candidat);
            }
        }
        
        [When(@"je cloture le scrutin")]
        public void WhenJeClotureLeScrutin()
        {
            _resultat = _voteService.CalculateResult(_candidats, _votesBlanchs);
        }
        
        [Then(@"le vainqueur devrait être ""(.*)""")]
        public void ThenLeVainqueurDevraitEtre(string nomVainqueur)
        {
            Assert.NotNull(_resultat);
            Assert.NotNull(_resultat.Winner);
            Assert.Equal(nomVainqueur, _resultat.Winner.Name);
        }
        
        [Then(@"(.*) devrait avoir (.*)% des voix")]
        public void ThenDevraitAvoirDesVoix(string nomCandidat, double pourcentageAttendu)
        {
            Assert.NotNull(_resultat);
            var pourcentageReel = _resultat.GetCandidatePercentage(nomCandidat);
            Assert.Equal(pourcentageAttendu, pourcentageReel);
        }
        
        [Then(@"le scrutin devrait être terminé en (.*) tours?")]
        public void ThenLeScrutinDevraitEtreTermineEnTour(int nombreTours)
        {
            Assert.NotNull(_resultat);
            Assert.Equal(nombreTours, _resultat.NumberOfRounds);
            Assert.True(_resultat.IsCompleted);
        }
        
        [Then(@"il devrait y avoir un second tour")]
        public void ThenIlDevraitYAvoirUnSecondTour()
        {
            Assert.NotNull(_resultat);
            Assert.False(_resultat.IsCompleted);
            Assert.Null(_resultat.Winner);
            Assert.True(_resultat.SecondRoundCandidates.Count >= 2);
        }
        
        [Then(@"les candidats pour le second tour devraient être ""([^""]*)"" et ""([^""]*)""")]
        public void ThenLesCandidatsPourLeSecondTourDevraientEtre(string candidat1, string candidat2)
        {
            Assert.NotNull(_resultat);
            Assert.Equal(2, _resultat.SecondRoundCandidates.Count);
            
            var nomsSecondTour = _resultat.SecondRoundCandidates.Select(c => c.Name).ToList();
            Assert.Contains(candidat1, nomsSecondTour);
            Assert.Contains(candidat2, nomsSecondTour);
        }

        [Then(@"les candidats pour le second tour devraient être ""([^""]*)"", ""([^""]*)"" et ""([^""]*)""")]
        public void ThenLesCandidatsPourLeSecondTourDevraientEtreTrois(string candidat1, string candidat2, string candidat3)
        {
            Assert.NotNull(_resultat);
            Assert.Equal(3, _resultat.SecondRoundCandidates.Count);
    
            var nomsSecondTour = _resultat.SecondRoundCandidates.Select(c => c.Name).ToList();
            Assert.Contains(candidat1, nomsSecondTour);
            Assert.Contains(candidat2, nomsSecondTour);
            Assert.Contains(candidat3, nomsSecondTour);
        }

        [Then(@"tous les candidats devraient passer au second tour")]
        public void ThenTousLesCandidatsDevraientPasserAuSecondTour()
        {
            Assert.NotNull(_resultat);
            Assert.Equal(_candidats.Count, _resultat.SecondRoundCandidates.Count);
    
            foreach (var candidat in _candidats)
            {
                Assert.Contains(candidat.Name, _resultat.SecondRoundCandidates.Select(c => c.Name));
            }
        }
        
        [Then(@"il ne devrait pas y avoir de vainqueur")]
        public void ThenIlNeDevraitPasYAvoirDeVainqueur()
        {
            Assert.NotNull(_resultat);
            Assert.Null(_resultat.Winner);
            Assert.True(_resultat.IsCompleted);
        }

        [Then(@"il devrait y avoir (.*) votes blancs")]
        public void ThenIlDevraitYAvoirVotesBlancs(int nombreVotesBlancAttendu)
        {
            Assert.NotNull(_resultat);
            Assert.Equal(nombreVotesBlancAttendu, _resultat.BlankVotes);
        }

        [Then(@"le total des suffrages exprimés devrait être (.*)")]
        public void ThenLeTotalDesSuffragesExprimesDevraitEtre(int totalAttendu)
        {
            Assert.NotNull(_resultat);
            Assert.Equal(totalAttendu, _resultat.TotalSuffragesExprimees);
        }
    }
}