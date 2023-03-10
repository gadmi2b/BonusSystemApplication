using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IWorkprojectRepository
    {
        Workproject GetWorkprojectData(long workprojectId);
        List<Workproject> GetWorkprojectsNames();
        bool IsWorkprojectExists(long workprojectId);
    }
}
