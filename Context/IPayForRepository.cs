using System.Collections.Generic;
using System.Threading.Tasks;
using PayFor.Models;

namespace PayFor.Context
{
    public interface IPayForRepository
    {
        //Groups
        Task<Group> GetGroup(int id, string userId);
        Task<IEnumerable<Group>> GetUserGroups(string userId);
        Task CreateGroup(Group group, string userId);
        Task<bool> DeleteGroup(int id, string userId);
        

        //Payments
        Task<Payment> GetPayment(int id, string userId);
        Task<IEnumerable<Payment>> GetUserPayments(string userId);
        Task<bool> CreatePayment(Payment payment, string userId);
        Task<bool> DeletePayment(int id, string userId);

        //Payments
        Task<Category> GetCategory (int id, string userId);
        Task<IEnumerable<Category>> GetAllCategories(string userId);
        bool CreateCategory(Category category, string userId);
        Task<bool> DeleteCategory(int id, string userId);
        
        


        Task<bool> SaveChangesAsync();
    }
}