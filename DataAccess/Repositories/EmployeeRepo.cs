using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class EmployeeRepo : IEmployee
    {

        private readonly AppDBContext _dbContext;

        public EmployeeRepo(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> AddAsync(Employee employee)
        {
            //check if employee already exists
            var check = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email == employee.Email);
            if (check != null)
            {
                return new ServiceResponse(false, "Employee already exists");
            }

            // check if employee is null
            if (string.IsNullOrEmpty(employee.Name) && string.IsNullOrEmpty(employee.Email))
            {
                return new ServiceResponse(false, "Employee cannot be null");
            }

            //add data to database
            _dbContext.Employees.Add(employee);
            await SaveChangesAsync();

            return new ServiceResponse(true, "Employee added successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            //delete data from database

            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return new ServiceResponse(false, "Employee not found");
            }
            else
            {
                _dbContext.Employees.Remove(employee);
                await SaveChangesAsync();

                return new ServiceResponse(true, "Employee deleted successfully");
            }
        }

        public async Task<List<Employee>> GetAsync()
        {
            return await _dbContext.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _dbContext.Employees.AsNoTracking().FirstAsync(x => x.Id == id);
        }

        public async Task<ServiceResponse> UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await SaveChangesAsync();

            return new ServiceResponse(true, "Employee updated successfully");

        }

        //private method to save changes to database
        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
