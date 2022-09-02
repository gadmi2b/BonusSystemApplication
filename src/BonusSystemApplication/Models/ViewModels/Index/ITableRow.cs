using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public interface ITableRow
    {
        public static bool GetAccessFilters(Form form, out List<AccessFilter> accessFilters)
        {
            List<AccessFilter> accesses = new List<AccessFilter>();
            // TODO: to determine which access filters form belongs
            Func<Form, bool> delegateLA = ExpressionBuilder.GetExpressionForLocalAccess(1).Compile();
            if (delegateLA.Invoke(form))
            {
                accesses.Add(AccessFilter.Employee);
            }

            accessFilters = accesses;
            return true;
        }
    }
}
