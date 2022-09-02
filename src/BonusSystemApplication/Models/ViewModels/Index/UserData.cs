namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class UserDataSinglton
    {
        private static bool isCreated = false;
        public long UserId { get; }
        public IEnumerable<FormGlobalAccess> FormGlobalAccesses { get; }

        public UserDataSinglton(long userId, IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            if (isCreated)
            {
                throw new Exception("Only one instance of object is allowed to be created");
            }

            isCreated = true;
            UserId = userId;
            FormGlobalAccesses = formGlobalAccesses;
        }
    }
}
