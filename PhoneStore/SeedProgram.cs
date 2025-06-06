using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneStore.Models;

namespace PhoneStore
{
    public class SeedProgram
    {
        // Removed Main method to avoid entry point conflicts
        
        public static void RunSeed()
        {
            // Chạy seeder
            PermissionSeeder.SeedPermissions();
        }
    }
}
