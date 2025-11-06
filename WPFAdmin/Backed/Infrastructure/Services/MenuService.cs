using Backed.Domain.Entities;
using Backed.Domain.Interfaces;
using Backed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backed.Infrastructure.Services;

public class MenuService : IMenuService
{
    private readonly ApplicationDbContext _context;

    public MenuService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Menu>> GetAllAsync()
    {
        return await _context.Menus.ToListAsync();
    }

    public async Task<Menu?> GetByIdAsync(int id)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task<Menu> CreateAsync(Menu menu)
    {
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
        return menu;
    }

    public async Task UpdateAsync(Menu menu)
    {
        var existingMenu = await _context.Menus.FindAsync(menu.Id);
        if (existingMenu != null)
        {
            _context.Entry(existingMenu).CurrentValues.SetValues(menu);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu != null)
        {
            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Menu>> GetActiveMenusAsync()
    {
        return await _context.Menus
            .Where(m => m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenuTreeAsync()
    {
        var menus = await _context.Menus
            .Include(m => m.Children)
            .Where(m => m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();

        // Build the tree structure
        var topLevelMenus = menus.Where(m => m.ParentId == null).ToList();
        var menuDict = menus.ToDictionary(m => m.Id);

        // Clear children collections to ensure clean state
        foreach (var menu in menus)
        {
            menu.Children.Clear();
        }

        foreach (var menu in menus)
        {
            if (menu.ParentId.HasValue && menuDict.ContainsKey(menu.ParentId.Value))
            {
                menuDict[menu.ParentId.Value].Children.Add(menu);
            }
        }

        return topLevelMenus;
    }
}