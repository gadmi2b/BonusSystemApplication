namespace UsefulCode
{
    // --------------------- HomeController ---------------------
    //public async Task<IActionResult> Index(List<UserRoles> selectedRoles)

    // --------------------- under rework ---------------------

    #region Forms where user has Global access:
    //IEnumerable<Form> globalAccessForms = new List<Form>();
    //foreach(var access in formGlobalAccessRepository.GetAll())
    //{
    //    if(access.UserId == userId)
    //    {
    //        globalAccessForms = formRepository.GetFormsWithGlobalAccess();
    //        break;
    //    }
    //}

    #endregion

    #region HomeIndexViewModel: forms where user has participant role
    //List<HomeIndexViewModel> homeIndexViewModels = new List<HomeIndexViewModel>();

    // TODO: to analyse: split whole process into separate processes:
    //                   - getting forms (from different criteries)
    //                   - creating List of HomeIndexViewModels
    //                   how to check in the View which selectedRoles to show

    //foreach (Form form in formRepository.GetFormsRelatedToUser(userId))
    //{
    //    HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel
    //    {
    //        Id = form.Id,
    //        WorkprojectName = form.Workproject.Name,
    //        EmployeeFullName = $"{form.Employee.LastNameEng} {form.Employee.FirstNameEng}",
    //        Year = form.Year,
    //        Period = form.Period
    //    };

    //    if (form.EmployeeId == userId)
    //    {
    //        homeIndexViewModel.UserRoles.Add(UserRoles.Employee);
    //    }
    //    if (form.ManagerId == userId)
    //    {
    //        homeIndexViewModel.UserRoles.Add(UserRoles.Manager);
    //    }
    //    if (form.ApproverId == userId)
    //    {
    //        homeIndexViewModel.UserRoles.Add(UserRoles.Approver);
    //    }

    //    foreach(FormLocalAccess a in form.FormLocalAccess)
    //    {
    //        if(a.UserId == userId)
    //        {
    //            homeIndexViewModel.UserRoles.Add(UserRoles.LocalAccess);
    //        }
    //    }

    //    homeIndexViewModels.Add(homeIndexViewModel);
    //}
    #endregion

    #region HomeIndexViewModel: forms where user local access
    //foreach (Form f in formRepository.GetFormsWithUserLocalAccess(userId))
    //{
    //    HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel
    //    {
    //        Id = f.Id,
    //        WorkprojectName = f.Workproject.Name,
    //        EmployeeFullName = $"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}",
    //        Year = f.Year,
    //        Period = f.Period
    //    };

    //    viewModelsForIndex.Add(homeIndexViewModel);
    //}
    #endregion

    // --------------------- under rework ---------------------

    //List<HomeIndexViewModel> filtered = homeIndexViewModels.FindAll(vm => vm.UserRoles.Contains(UserRoles.Employee));
    //return View(await viewModelsForIndex.AsNoTracking().ToListAsync());


    // --------------------- FormRepository ---------------------
    // global rework

    //public IEnumerable<Form> GetFormsRelatedToUser(long userId)
    //{
    //    return context.Forms
    //        .Where(f => f.EmployeeId == userId || f.ManagerId == userId || f.ApproverId == userId ||
    //              (f.FormLocalAccess.Where(a => a.UserId == userId).Count() > 0)) // Any() can be used here
    //        .Include(f => f.Employee)
    //        .Include(f => f.Workproject)
    //        .Include(f => f.FormLocalAccess)
    //        .ToList();
    //}

    //public IEnumerable<Form> GetFormsWithGlobalAccess()
    //{
    //    // TODO: to add default filters here due to numerous outputs

    //    List<Form> forms = new List<Form>();
    //    forms = context.Forms
    //        .Include(f => f.Employee)
    //        .Include(f => f.Workproject)
    //        .Include(f => f.FormLocalAccess)
    //        .ToList();
    //    return forms;
    //}

    //-----------------------------------------------------------------
    //foreach (var formGA in formGlobalAccesses)
    //{
    //    IQueryable<Form> query;

    //    if (formGA.DepartmentId == null) // all forms available for user
    //    {
    //        formsQuery = formsQueryInitial;
    //        break;
    //    }
    //    else if (formGA.TeamId == null) // forms with same: department
    //    {
    //        query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId);
    //    }
    //    else if (formGA.WorkprojectId == null) // forms with same: department, team
    //    {
    //        query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId &&
    //                                                    f.Employee.TeamId == formGA.TeamId);
    //    }
    //    else // forms with: same department, team, workproject
    //    {
    //        query = formsQueryInitial.Where(f => f.Employee.DepartmentId == formGA.DepartmentId &&
    //                                                    f.Employee.TeamId == formGA.TeamId &&
    //                                                    f.WorkprojectId == formGA.WorkprojectId);
    //    }

    //    if (formsQuery == null)
    //    {
    //        formsQuery = query;
    //    }
    //    else
    //    {
    //        formsQuery = formsQuery.Union(query);
    //    }
    //}

}