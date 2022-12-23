namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class SignatureViewModel
    {
        ForObjectives ForObjectives { get; set; }
        ForResults ForResults { get; set; }
    
        public SignatureViewModel(Signatures source)
        {
            ForObjectives = source.ForObjectives;
            ForResults = source.ForResults;
        }
    }
}
