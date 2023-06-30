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

        public async Task CreateSelectListsAsync(FormEditViewModel formEditViewModel)
        {
            formEditViewModel.WorkprojectsSelectList = await GetWorkprojectsSelectListAsync(formEditViewModel.AreObjectivesFrozen,
                                                                                            formEditViewModel.Definition.WorkprojectId);
            formEditViewModel.EmployeesSelectList = await GetEmployeesSelectListAsync(formEditViewModel.AreObjectivesFrozen,
                                                                                      formEditViewModel.Definition.EmployeeId);
            formEditViewModel.ManagersSelectList = await GetManagersSelectListAsync(formEditViewModel.AreObjectivesFrozen,
                                                                                    formEditViewModel.Definition.ManagerId);
            formEditViewModel.ApproversSelectList = await GetApproversSelectListAsync(formEditViewModel.AreObjectivesFrozen,
                                                                                      formEditViewModel.Definition.ApproverId);
            formEditViewModel.PeriodsSelectList = GetPeriodsSelectList(formEditViewModel.AreObjectivesFrozen,
                                                                       formEditViewModel.Definition.Period);
        }


        private async Task<List<SelectListItem>> GetWorkprojectsSelectListAsync(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            var rawItems = await _formService.GetWorkprojectIdsNamesAsync();
            var items = rawItems.Select(d => new SelectListItem
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

        private async Task<List<SelectListItem>> GetEmployeesSelectListAsync(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            var rawItems = await _formService.GetUserIdsNamesAsync();
            var items = rawItems.Select(d => new SelectListItem
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

        private async Task<List<SelectListItem>> GetManagersSelectListAsync(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            var rawItems = await _formService.GetUserIdsNamesAsync();
            var items = rawItems.Select(d => new SelectListItem
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

        private async Task<List<SelectListItem>> GetApproversSelectListAsync(bool isDisabled, long? selectedId)
        {
            ArgumentNullException.ThrowIfNull(_formService, nameof(_formService));

            var rawItems = await _formService.GetUserIdsNamesAsync();
            var items = rawItems.Select(d => new SelectListItem
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
