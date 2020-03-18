using System.Collections.Generic;
using MvvmHelpers;
using Autumn.Models.NavigationMenu;

namespace Autumn.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}