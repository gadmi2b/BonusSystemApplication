using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.Models.Forms.Edit;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.Handlers
{
    public class SelectListsCreator
    {
        IFormsService _formService { get; set; }

        public SelectListsCreator(IFormsService formService)
        {
            _formService = formService;
        }

        public void CreateSelectLists(FormEditViewModel formEditViewModel)
        {
            formEditViewModel.WorkprojectsSelectList = GetWorkprojectsSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                                 formEditViewModel.Definition.WorkprojectId);
            formEditViewModel.EmployeesSelectList = GetEmployeesSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                           formEditViewModel.Definition.EmployeeId);
            formEditViewModel.ManagersSelectList = GetManagersSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                         formEditViewModel.Definition.ManagerId);
            formEditViewModel.ApproversSelectList = GetApproversSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                           formEditViewModel.Definition.ApproverId);
            formEditViewModel.PeriodsSelectList = GetPeriodsSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                       formEditViewModel.Definition.Period);
        }


        private List<SelectListItem> GetWorkprojectsSelectList(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            List<SelectListItem> items = _formService.GetWorkprojectIdsNames()
                            .Select(d => new SelectListItem
                            {
                                Value = d.Key,
                                Text = d.Value,
                                Disabled = isDisabled,
                            })
                            .ToList();
            if (selectedId == null || !isDisabled)
                return items;

            EnableSelectedIdItem(items, selectedId);
            return items;
        }

        private List<SelectListItem> GetEmployeesSelectList(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            List<SelectListItem> items = _formService.GetUserIdsNames()
                            .Select(d => new SelectListItem
                            {
                                Value = d.Key,
                                Text = d.Value,
                                Disabled = isDisabled,
                            })
                            .ToList();
            if (selectedId == null || !isDisabled)
                return items;

            EnableSelectedIdItem(items, selectedId);
            return items;
        }

        private List<SelectListItem> GetManagersSelectList(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            List<SelectListItem> items = _formService.GetUserIdsNames()
                            .Select(d => new SelectListItem
                            {
                                Value = d.Key,
                                Text = d.Value,
                                Disabled = isDisabled,
                            })
                            .ToList();
            if (selectedId == null || !isDisabled)
                return items;

            EnableSelectedIdItem(items, selectedId);
            return items;
        }

        private List<SelectListItem> GetApproversSelectList(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            List<SelectListItem> items = _formService.GetUserIdsNames()
                            .Select(d => new SelectListItem
                            {
                                Value = d.Key,
                                Text = d.Value,
                                Disabled = isDisabled,
                            })
                            .ToList();
            if (selectedId == null || !isDisabled)
                return items;

            EnableSelectedIdItem(items, selectedId);
            return items;
        }

        private List<SelectListItem> GetPeriodsSelectList(bool isDisabled, string? selectedItem)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            List<SelectListItem> items = _formService.GetPeriodNames()
                            .Select(d => new SelectListItem
                            {
                                Value = d,
                                Text = d,
                                Disabled = isDisabled,
                            })
                            .ToList();
            if (selectedItem == null || !isDisabled)
                return items;

            EnableSelectedStringItem(items, selectedItem);
            return items;
        }


        private void EnableSelectedIdItem(List<SelectListItem> items, long? selectedId)
        {
            foreach (var item in items)
            {
                if (long.TryParse(item.Value, out long id))
                {
                    if (id == selectedId)
                    {
                        item.Disabled = false;
                        break;
                    }
                }
            }
        }

        private void EnableSelectedStringItem(List<SelectListItem> items, string? selectedItem)
        {
            foreach (var item in items)
            {
                if (item.Value == selectedItem)
                {
                    item.Disabled = false;
                    break;
                }
            }
        }
    }
}
