using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class ObjectiveResultViewModel
    {
        public int Row { get; set; }
        public Objective Objective { get; set; }
        public Result Result { get; set; }

        public ObjectiveResultViewModel() { }
        public ObjectiveResultViewModel(ObjectiveResult source)
        {
            Row = source.Row;
            Objective = source.Objective;
            Result = source.Result;
        }
    }
}
