namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class SignaturesViewModel
    {
        public ForObjectives ForObjectives { get; set; }
        public ForResults ForResults { get; set; }
    
        public SignaturesViewModel(Signatures source)
        {
            ForObjectives = source.ForObjectives;
            ForResults = source.ForResults;
        }
    }
}
