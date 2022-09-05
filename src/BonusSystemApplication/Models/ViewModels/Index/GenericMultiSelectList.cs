using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class GenericMultiSelectList<T1,T2> where T2 : SelectBase
    {
        public MultiSelectList MultiSelectList { get; set; }
        public GenericMultiSelectList(List<T1> collection, string[] selectedValues)
        {
            Type listType = typeof(T1);
            Func<T1, string> expr = (T1 param) => string.Empty;

            if (listType.IsEnum)
            {
                expr = (T1 param) => Enum.GetName(typeof(T1), param);
            }
            else if (listType == typeof(int) ||
                     listType == typeof(long))
            {
                expr = (T1 param) => param.ToString();
            }
            else if (listType == typeof(string))
            {
                expr = (T1 param) => param.ToString();
            }
            else
            {
                throw new Exception($"Generic select object error: unexpected type: {typeof(T1)}." +
                                    $"An additional case to operate this type should be added");
            }

            int counterId = 1;
            List<T2> baseSelects = new List<T2>();
            foreach (T1 item in collection)
            {
                string selectName = expr.Invoke(item);
                T2 baseSelect = (T2)Activator.CreateInstance(typeof(T2), new object[] { counterId, selectName } );

                baseSelects.Add(baseSelect);
                counterId++;
            }



            MultiSelectList = new MultiSelectList(baseSelects,
                                                  $"{nameof(SelectBase.Id)}",
                                                  $"{nameof(SelectBase.Name)}",
                                                  selectedValues);
        }
    }
}
