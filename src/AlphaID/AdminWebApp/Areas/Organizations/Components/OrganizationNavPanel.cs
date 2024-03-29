﻿using IdSubjects;
using Microsoft.AspNetCore.Mvc;

namespace AdminWebApp.Areas.Organizations.Components;

public class OrganizationNavPanel(IOrganizationStore organizationStore) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return this.View(model: organizationStore.Organizations.Count());
    }
}
