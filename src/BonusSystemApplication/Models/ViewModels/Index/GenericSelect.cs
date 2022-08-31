using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class GenericSelect<T>
    {
        public List<SelectListItem> SelectListItems { get; set; } = new List<SelectListItem>();
        public GenericSelect(List<T> collection)
        {
            Type listType = typeof(T);
            Func<T, string> expr = (T param) => string.Empty;

            if (listType.IsEnum)
            {
                expr = (T param) => Enum.GetName(typeof(T), param);
            }
            else if (listType == typeof(int) ||
                     listType == typeof(long))
            {
                expr = (T param) => param.ToString();
            }
            else if (listType == typeof(string))
            {
                expr = (T param) => param.ToString();
            }
            else
            {
                throw new Exception($"Generic select object error: unexpected type: {typeof(T)}." +
                                    $"An additional case to operate this type should be added");
            }

            int counter = 1;
            foreach (T item in collection)
            {
                SelectListItem selectItemList = new SelectListItem
                {
                    Value = counter.ToString(),
                    Text = expr.Invoke(item),
                };

                SelectListItems.Add(selectItemList);
                counter++;
            }
        }
    }
}
