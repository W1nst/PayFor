using System.Collections.Generic;
using System.Threading.Tasks;
using PayFor.Models;

namespace PayFor.Context
{
    public interface IPayForRepository
    {
        //Groups
        Task<Group> GetGroup(int groupId, string userId);
        Task<IEnumerable<Group>> GetUserGroups(string userId);
        Task CreateGroup(Group group, string userId);
        Task<bool> DeleteGroup(int groupId, string userId);
        

        //Payments
        Task<Payment> GetPayment(int pId, string userId);
        Task<IEnumerable<Payment>> GetUserPayments(string userId);
        Task<bool> CreatePayment(Payment payment, string userId);
        Task<bool> DeletePayment(int pId, string userId);
        


        Task<bool> SaveChangesAsync();
    }
}