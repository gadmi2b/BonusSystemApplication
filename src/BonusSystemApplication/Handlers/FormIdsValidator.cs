using BonusSystemApplication.BLL.UserIdentiry;

namespace BonusSystemApplication.Handlers
{
    public class FormIdsValidator
    {
        public List<long> ValidateFormIds(List<long> formIds)
        {
            foreach (long formId in formIds)
            {
                if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
                  formIds.Remove(formId);
            }
            return formIds;
        }
    }
}
