using Crm.Models;

namespace Crm.Services
{
    public interface ICustomerService
    {
        void Add(Customer customer);
        bool Delete(int id);
        List<Customer> GetAll();
        Customer? GetById(int id);
        List<Customer> Search(string? text, string? status);
        bool Update(Customer updated);
    }
}